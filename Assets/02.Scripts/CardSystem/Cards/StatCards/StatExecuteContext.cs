using _02.Scripts.CardSystem.Cards.ActionCards;

namespace _02.Scripts.CardSystem.Cards.StatCards
{
    public class StatExecuteContext
    {
        public StatCard StatCard;
        public ActionCard ActionCard;

        public StatExecuteContext(StatCard statCard, ActionCard actionCard)
        {
            StatCard = statCard;
            ActionCard = actionCard;
        }
    }
}