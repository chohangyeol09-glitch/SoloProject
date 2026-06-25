using UnityEngine;

namespace _02.Scripts.Enemys.Boss
{
    public abstract class AbstractEnemyConditionSO : ScriptableObject
    {
        [field: SerializeField] public bool TriggerOnce { get; private set; }
        [field: SerializeField] public string Description { get; private set; }

        public abstract bool IsActivate(EnemyPatternContext context);

        // 스테이지 시작 시 호출 — 다단계 조건 등의 런타임 상태를 초기화한다.
        public virtual void ResetRuntimeState() { }

        // 조건이 충족되어 패턴 발동이 확정될 때 호출 — 다단계 조건이 단계를 소비한다.
        public virtual void NotifyTriggered(EnemyPatternContext context) { }

        public virtual string GetDescription() => Description;
    }
}   