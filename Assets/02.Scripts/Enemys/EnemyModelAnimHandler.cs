using _02.Scripts.CoreSystem.EventChannel;
using _02.Scripts.CoreSystem.EventChannel.EnemyEvents;
using _02.Scripts.CoreSystem.EventChannel.EnemyEvents.BossEvents;
using UnityEngine;

namespace _02.Scripts.Enemys
{
    public class EnemyModelAnimHandler : MonoBehaviour
    {
        [SerializeField] private EventChannelSO enemyChannel;

        public void EndSpawn() => enemyChannel?.RaiseEvent(new EnemySpawnEndEvent().Init(GetComponent<Animator>()));
        
        public void EndHit() => enemyChannel?.RaiseEvent(new EnemyHitEndEvent());

        public void EndDie()
        {
            enemyChannel?.RaiseEvent(new EnemyDieEndEvent());
        }

        public void OnPatternEffect() 
        {
            Debug.Log("OnPatternEffect");    
            enemyChannel?.RaiseEvent(new EnemyPatternEffectEvent());
        }

        public void OnPatternEnd()
        {
            Debug.Log("OnPatternEnd");
            enemyChannel?.RaiseEvent(new EnemyPatternEndEvent());
        }
    }
}