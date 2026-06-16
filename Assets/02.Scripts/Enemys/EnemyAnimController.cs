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
            gameEventChannel.AddListener<StageClearEvent>(HandleStageClear);
            enemyEventChannel.AddListener<TakeDamageEvent>(HandleTakeDamage);
            enemyEventChannel.AddListener<EnemyDieEndEvent>(HandleDieEnd);
            enemyEventChannel.AddListener<EnemyPatternStartEvent>(HandlePatternStart);
            enemyEventChannel.AddListener<EnemyPatternEffectEvent>(HandlePatternEffect);
            enemyEventChannel.AddListener<EnemyPatternEndEvent>(HandlePatternEnd);
        }

        private void OnDestroy()
        {
            gameEventChannel.RemoveListener<StageStartEvent>(HandleStageStart);
            gameEventChannel.RemoveListener<StageClearEvent>(HandleStageClear);
            enemyEventChannel.RemoveListener<TakeDamageEvent>(HandleTakeDamage);
            enemyEventChannel.RemoveListener<EnemyDieEndEvent>(HandleDieEnd);
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

        private void HandleStageClear(StageClearEvent evt)
            => _animator?.Play("DIE");

        private void HandleDieEnd(EnemyDieEndEvent evt)
        {
            if (_currentModel != null) Destroy(_currentModel);
            _currentModel = null;
            _animator = null;
        }

        private void HandlePatternStart(EnemyPatternStartEvent evt)
        {
            _pendingPatternEvt = evt;
            _animator?.Play(evt.AnimationName);
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
