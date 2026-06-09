namespace _02.Scripts.CoreSystem.EventChannel.CardEvent.ActionCardEvents
{
    public class WeaponHitEvent : GameEvent
    {
        public int Id;

        public WeaponHitEvent Init(int id)
        {
            Id = id; return this;
        }
    }
}