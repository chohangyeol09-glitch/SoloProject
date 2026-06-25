using Cysharp.Threading.Tasks;
using _02.Scripts.CoreSystem;
using _02.Scripts.CoreSystem.EventChannel;
using _02.Scripts.CoreSystem.EventChannel.EnemyEvents;
using _02.Scripts.CoreSystem.EventChannel.EnemyEvents.BossEvents;
using _02.Scripts.CoreSystem.EventChannel.GameEvents.StageEvents;
using _02.Scripts.CoreSystem.EventChannel.PlayerEvents;
using UnityEngine;

namespace _02.Scripts.Enemys
{
    public class EnemyAnimController : MonoBehaviour
    {
        [SerializeField] private EventChannelSO gameEventChannel;
        [SerializeField] private EventChannelSO enemyEventChannel;
        [SerializeField] private Transform spawnPoint;
        private GameObject _currentModel;
        private Animator _animator;
        private EnemyPatternStartEvent _pendingPatternEvt;
        private bool _patternEffectFired;
        private bool _patternEndFired;

        private void Awake()
        {
            gameEventChannel.AddListener<StageStartEvent>(HandleStageStart);
            enemyEventChannel.AddListener<TakeDamageEvent>(HandleTakeDamage);
            enemyEventChannel.AddListener<EnemyPatternStartEvent>(HandlePatternStart);
            enemyEventChannel.AddListener<EnemyPatternEffectEvent>(HandlePatternEffect);
            enemyEventChannel.AddListener<EnemyPatternEndEvent>(HandlePatternEnd);
        }

        private void OnDestroy()
        {
            gameEventChannel.RemoveListener<StageStartEvent>(HandleStageStart);
            enemyEventChannel.RemoveListener<TakeDamageEvent>(HandleTakeDamage);
            enemyEventChannel.RemoveListener<EnemyPatternStartEvent>(HandlePatternStart);
            enemyEventChannel.RemoveListener<EnemyPatternEffectEvent>(HandlePatternEffect);
            enemyEventChannel.RemoveListener<EnemyPatternEndEvent>(HandlePatternEnd);
        }

        private void HandleStageStart(StageStartEvent evt)
        {
            if (_currentModel != null) Destroy(_currentModel);
            if (evt.EnemyData.ModelPrefab == null) return;

            _currentModel = Instantiate(evt.EnemyData.ModelPrefab, spawnPoint.position, spawnPoint.rotation);
            _animator = _currentModel.GetComponent<Animator>();
            _animator?.Play("SPAWN");
        }

        private void HandleTakeDamage(TakeDamageEvent evt)
            => _animator?.Play("HIT");

        // 죽음 연출: DIE 재생 → 끝까지(Animator 상태로) 대기 → 모델 삭제. StageManager 시퀀스가 await로 호출.
        public async UniTask PlayDeath()
        {
            if (_animator == null) return;

            _animator.speed = PresentationControl.Speed;
            _animator.Play("DIE");

            // DIE 상태로 진입할 때까지
            await UniTask.WaitUntil(() => _animator == null || _animator.GetCurrentAnimatorStateInfo(0).IsName("DIE"));

            // DIE가 끝날 때까지. normalizedTime은 animator.speed를 자동 반영하므로 배속에도 정확.
            // (DIE가 끝나거나 다른 상태로 빠지면 빠져나옴 → hang 없음)
            await UniTask.WaitWhile(() =>
            {
                if (_animator == null) return false;
                AnimatorStateInfo st = _animator.GetCurrentAnimatorStateInfo(0);
                return st.IsName("DIE") && st.normalizedTime < 1f;
            });

            if (_currentModel != null) Destroy(_currentModel);
            _currentModel = null;
            _animator = null;
        }

        private void HandlePatternStart(EnemyPatternStartEvent evt)
        {
            _pendingPatternEvt = evt;
            _patternEffectFired = false;
            _patternEndFired = false;

            bool canPlay = _animator != null
                           && !string.IsNullOrEmpty(evt.AnimationName)
                           && _animator.HasState(0, Animator.StringToHash(evt.AnimationName));

            if (canPlay)
            {
                _animator.speed = PresentationControl.Speed;
                _animator.Play(evt.AnimationName);
                // 애니메이션 이벤트(OnPatternEffect/OnPatternEnd)가 들어오기를 기다리되,
                // 누락된 경우에도 멈추지 않도록 클립 종료를 직접 감시해 폴백 처리한다.
                WatchPattern(evt, evt.AnimationName).Forget();
            }
            else
            {
                // 애니가 없거나 이름이 잘못된 경우: 멈추지 않게 효과+종료를 즉시 처리
                InvokePatternEffect(evt);
                InvokePatternEnd(evt);
            }
        }

        // 클립이 끝났는데도 애니메이션 이벤트가 안 들어오면 효과/종료를 폴백으로 호출한다.
        private async UniTaskVoid WatchPattern(EnemyPatternStartEvent evt, string animName)
        {
            // 상태 진입 대기 (전환 지연 대비, 안전 타임아웃 포함)
            float enterTimeout = 1f;
            float elapsed = 0f;
            while (_animator != null && _pendingPatternEvt == evt
                   && !_animator.GetCurrentAnimatorStateInfo(0).IsName(animName))
            {
                if (elapsed >= enterTimeout) break;
                elapsed += Time.deltaTime;
                await UniTask.Yield();
            }

            // 클립 종료까지 대기 (상태를 벗어나거나 normalizedTime이 1에 도달하면 종료)
            await UniTask.WaitWhile(() =>
            {
                if (_animator == null || _pendingPatternEvt != evt) return false;
                AnimatorStateInfo st = _animator.GetCurrentAnimatorStateInfo(0);
                return st.IsName(animName) && st.normalizedTime < 1f;
            });

            // 이미 다음 패턴으로 교체됐거나 종료가 처리됐으면 무시
            if (_pendingPatternEvt != evt) return;

            InvokePatternEffect(evt);
            InvokePatternEnd(evt);
        }

        private void HandlePatternEffect(EnemyPatternEffectEvent evt)
            => InvokePatternEffect(_pendingPatternEvt);

        private void HandlePatternEnd(EnemyPatternEndEvent evt)
            => InvokePatternEnd(_pendingPatternEvt);

        // 효과/종료는 애니메이션 이벤트와 폴백 양쪽에서 호출될 수 있으므로 1회만 실행되도록 보장한다.
        private void InvokePatternEffect(EnemyPatternStartEvent evt)
        {
            if (evt == null || evt != _pendingPatternEvt || _patternEffectFired) return;
            _patternEffectFired = true;
            evt.OnPatternEffect?.Invoke();
        }

        private void InvokePatternEnd(EnemyPatternStartEvent evt)
        {
            if (evt == null || evt != _pendingPatternEvt || _patternEndFired) return;
            _patternEndFired = true;
            _pendingPatternEvt = null;
            evt.OnPatternEnd?.Invoke();
        }
    }
}
