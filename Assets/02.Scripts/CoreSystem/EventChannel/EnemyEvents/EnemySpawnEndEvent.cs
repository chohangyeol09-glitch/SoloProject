using UnityEngine;

namespace _02.Scripts.CoreSystem.EventChannel.EnemyEvents
{
    public class EnemySpawnEndEvent : GameEvent
    {
        public Animator Animator;

        public EnemySpawnEndEvent Init(Animator animator)
        {
            Animator = animator;
            return this;
        }
    }
}