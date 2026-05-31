using _02.Scripts.CardSystem.Cards.ActionCards;

namespace _02.Scripts.CardSystem.Cards.StatCards
{
    public class StatExecuteContext
    {
        public StatCard StatCard;
        public ActionCard TargetActionCard;
        

        public StatExecuteContext(StatCard statCard, ActionCard targetActionCard)
        {
            StatCard = statCard;
            TargetActionCard = targetActionCard;
        }
    }
}