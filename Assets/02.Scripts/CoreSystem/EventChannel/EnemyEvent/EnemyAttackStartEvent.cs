namespace _02.Scripts.CoreSystem.EventChannel.EnemyEvent
{
    public class EnemyAttackStartEvent : GameEvent
    {
        public float WeaponAnimDuration;

        public EnemyAttackStartEvent Init(float weaponAnimDuration)
        {
            WeaponAnimDuration = weaponAnimDuration;
            return this;
        }
    }
}