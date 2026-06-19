using _02.Scripts.CardSystem.Cards.ActionCards;

namespace _02.Scripts.CardSystem.Cards.StatCards
{
    public class StatExecuteContext
    {
        public StatCard StatCard;
        public PlayerActionCard TargetActionCard;
        

        public StatExecuteContext(StatCard statCard, PlayerActionCard targetActionCard)
        {
            StatCard = statCard;
            TargetActionCard = targetActionCard;
        }
    }
}