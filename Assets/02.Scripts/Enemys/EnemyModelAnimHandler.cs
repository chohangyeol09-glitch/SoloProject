using _02.Scripts.CoreSystem.EventChannel;
using _02.Scripts.CoreSystem.EventChannel.EnemyEvents;
using UnityEngine;

namespace _02.Scripts.Enemys
{
    public class EnemyModelAnimHandler : MonoBehaviour
    {
        [SerializeField] private EventChannelSO enemyChannel;

        public void EndSpawn() => enemyChannel?.RaiseEvent(new EnemySpawnEndEvent());
        public void EndAttack() => enemyChannel?.RaiseEvent(new EnemyAttackEndEvent());
        public void EndHit() => enemyChannel?.RaiseEvent(new EnemyHitEndEvent());
        public void EndDie() => enemyChannel?.RaiseEvent(new EnemyDieEndEvent());
    }
}