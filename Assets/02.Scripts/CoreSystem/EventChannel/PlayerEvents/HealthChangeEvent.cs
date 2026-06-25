namespace _02.Scripts.CoreSystem.EventChannel.PlayerEvents
{
    public class HealthChangedEvent : GameEvent
    {
        public int MaxHealth;
        public int CurrentHealth;

        public HealthChangedEvent Init(int maxHealth,int currentHealth)
        {
            MaxHealth = maxHealth;
            CurrentHealth = currentHealth; 
            return this;
        }
    }
}