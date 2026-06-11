using _02.Scripts.CoreSystem.EventChannel;
using _02.Scripts.CoreSystem.EventChannel.EnemyEvents;
using _02.Scripts.CoreSystem.EventChannel.GameEvents.StageEvents;
using _02.Scripts.CoreSystem.EventChannel.PlayerEvents;
using UnityEngine;

namespace _02.Scripts.Enemys
{
    public class EnemyModelController : MonoBehaviour
    {
        [SerializeField] private EventChannelSO gameEventChannel;
        [SerializeField] private EventChannelSO enemyEventChannel;
        [SerializeField] private EventChannelSO playerChannel; 
        [SerializeField] private Transform spawnPoint;
        private GameObject _currentModel;
        private Animator _animator;
        private EnemyAttackStartEvent _pendingAttackEvt; 

        private void Awake()
        {
            gameEventChannel.AddListener<StageStartEvent>(HandleStageStart);
            gameEventChannel.AddListener<StageClearEvent>(HandleStageClear);
            enemyEventChannel.AddListener<TakeDamageEvent>(HandleTakeDamage);
            enemyEventChannel.AddListener<EnemyAttackStartEvent>(HandleAttackStart); 
            enemyEventChannel.AddListener<EnemyAttackEndEvent>(HandleAttackEnd);    
            enemyEventChannel.AddListener<EnemyDieEndEvent>(HandleDieEnd);
        }

        private void OnDestroy()
        {
            gameEventChannel.RemoveListener<StageStartEvent>(HandleStageStart);
            gameEventChannel.RemoveListener<StageClearEvent>(HandleStageClear);
            enemyEventChannel.RemoveListener<TakeDamageEvent>(HandleTakeDamage);
            enemyEventChannel.RemoveListener<EnemyAttackStartEvent>(HandleAttackStart);
            enemyEventChannel.RemoveListener<EnemyAttackEndEvent>(HandleAttackEnd);
            enemyEventChannel.RemoveListener<EnemyDieEndEvent>(HandleDieEnd);
        }

        private void HandleStageStart(StageStartEvent evt)
        {
            if (_currentModel != null) Destroy(_currentModel);
            if (evt.EnemyData.ModelPrefab == null) return;

            _currentModel = Instantiate(evt.EnemyData.ModelPrefab, spawnPoint.position, spawnPoint.rotation);
            _animator = _currentModel.GetComponent<Animator>();
            _animator?.SetTrigger("SPAWN");
        }

        private void HandleTakeDamage(TakeDamageEvent evt)
            => _animator?.SetTrigger("HIT");

        private void HandleStageClear(StageClearEvent evt)
            => _animator?.SetTrigger("DIE");

        private void HandleDieEnd(EnemyDieEndEvent evt)
        {
            if (_currentModel != null) Destroy(_currentModel);
            _currentModel = null;
            _animator = null;
        }

        private void HandleAttackStart(EnemyAttackStartEvent evt)
        {
            _pendingAttackEvt = evt;

            if (_animator == null)
            {
                playerChannel.RaiseEvent(new TakeDamageEvent().Init(evt.TotalDamage));
                evt.OnAttackEnd?.Invoke();
                _pendingAttackEvt = null;
                return;
            }

            _animator.SetTrigger("ATTACK");
        }

        private void HandleAttackEnd(EnemyAttackEndEvent evt)
        {
            if (_pendingAttackEvt == null) return;

            playerChannel.RaiseEvent(new TakeDamageEvent().Init(_pendingAttackEvt.TotalDamage));
            _pendingAttackEvt.OnAttackEnd?.Invoke(); 
            _pendingAttackEvt = null;
        }
    }
}