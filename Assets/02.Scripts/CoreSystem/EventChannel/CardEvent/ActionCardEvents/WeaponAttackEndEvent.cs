namespace _02.Scripts.CoreSystem.EventChannel.CardEvent.ActionCardEvents
{
    public class WeaponAttackEndEvent : GameEvent
    {
        public int Id;

        public WeaponAttackEndEvent Init(int id)
        {
            Id = id; return this;
        }
    }
}