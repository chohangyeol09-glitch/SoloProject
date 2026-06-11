namespace _02.Scripts.CoreSystem.EventChannel.PlayerEvents
{
    public class HealthChangedEvent : GameEvent
    {
        public int CurrentHealth;

        public HealthChangedEvent Init(int health)
        {
            CurrentHealth = health; return this;
        }
    }
}