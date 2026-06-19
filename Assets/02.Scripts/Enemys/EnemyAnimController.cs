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

            await UniTask.Yield();   // Play("DIE")가 반영될 때까지 한 프레임 대기

            if (_animator != null)
            {
                AnimatorStateInfo st = _animator.GetCurrentAnimatorStateInfo(0);
                float seconds = st.length / Mathf.Max(0.01f, _animator.speed);
                await UniTask.Delay((int)(seconds * 1000f));   // DIE 길이만큼(배속 반영) 대기
            }

            if (_currentModel != null) Destroy(_currentModel);
            _currentModel = null;
            _animator = null;
        }

        private void HandlePatternStart(EnemyPatternStartEvent evt)
        {
            _pendingPatternEvt = evt;

            bool canPlay = _animator != null
                           && !string.IsNullOrEmpty(evt.AnimationName)
                           && _animator.HasState(0, Animator.StringToHash(evt.AnimationName));

            if (canPlay)
            {
                _animator.speed = PresentationControl.Speed;
                _animator.Play(evt.AnimationName);
            }
            else
            {
                // 애니가 없거나 이름이 잘못된 경우: 멈추지 않게 효과+종료를 즉시 처리
                evt.OnPatternEffect?.Invoke();
                evt.OnPatternEnd?.Invoke();
                _pendingPatternEvt = null;
            }
        }

        private void HandlePatternEffect(EnemyPatternEffectEvent evt)
        {
            _pendingPatternEvt?.OnPatternEffect?.Invoke();
        }

        private void HandlePatternEnd(EnemyPatternEndEvent evt)
        {
            var pending = _pendingPatternEvt;
            _pendingPatternEvt = null;
            pending?.OnPatternEnd?.Invoke();
        }
    }
}
