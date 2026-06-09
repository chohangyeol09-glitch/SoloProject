using _02.Scripts.CoreSystem.EventChannel;
using _02.Scripts.CoreSystem.EventChannel.EnemyEvent;
using _02.Scripts.CoreSystem.EventChannel.GameEvents;
using _02.Scripts.CoreSystem.EventChannel.GameEvents.StageEvents;
using UnityEngine;

namespace _02.Scripts.Enemy
{
    public class EnemyModelController : MonoBehaviour
    {
        [SerializeField] private EventChannelSO gameEventChannel;
        [SerializeField] private EventChannelSO enemyEventChannel;
        [SerializeField] private Transform spawnPoint;

        private GameObject _currentModel;
        private Animator _animator;

        private void Awake()
        {
            gameEventChannel.AddListener<StageStartEvent>(HandleStageStart);
            gameEventChannel.AddListener<StageClearEvent>(HandleStageClear);
            enemyEventChannel.AddListener<TakeDamageEvent>(HandleTakeDamage);
            enemyEventChannel.AddListener<EnemyDieEndEvent>(HandleDieEnd);
            enemyEventChannel.AddListener<EnemyAttackStartEvent>(HandleAttackStart); 
        }

        private void OnDestroy()
        {
            gameEventChannel.RemoveListener<StageStartEvent>(HandleStageStart);
            gameEventChannel.RemoveListener<StageClearEvent>(HandleStageClear);
            enemyEventChannel.RemoveListener<TakeDamageEvent>(HandleTakeDamage);
            enemyEventChannel.RemoveListener<EnemyDieEndEvent>(HandleDieEnd);
            enemyEventChannel.RemoveListener<EnemyAttackStartEvent>(HandleAttackStart);
        }


        private void HandleStageStart(StageStartEvent evt)
        {
            if (_currentModel != null)
                Destroy(_currentModel);

            if (evt.EnemyData.ModelPrefab == null) return;

            _currentModel = Instantiate(
                evt.EnemyData.ModelPrefab,
                spawnPoint.position,
                spawnPoint.rotation
            );
            _animator = _currentModel.GetComponent<Animator>();
            _animator?.SetTrigger("SPAWN");
        }

        private void HandleTakeDamage(TakeDamageEvent evt)
        {
            _animator?.SetTrigger("HIT");
        }

        private void HandleStageClear(StageClearEvent evt)
        {
            _animator?.SetTrigger("DIE");
        }

        private void HandleDieEnd(EnemyDieEndEvent evt)
        {
            if (_currentModel != null)
                Destroy(_currentModel);
            _currentModel = null;
            _animator = null;
        }
        
        private void HandleAttackStart(EnemyAttackStartEvent evt)
        {
            if (_animator == null) return;

            _animator.SetTrigger("ATTACK");
        }
    }
}