using System.Collections.Generic;
using _02.Scripts.CardSystem.Cards.StatCards;

namespace _02.Scripts.DeckSystem
{
    public class PlayerRuntimeDeck
    {
        public List<StatCardDataSO> Cards { get; private set; } = new();

        public void Initialize(StartStatCardListSO statCards)
        {
            Cards.Clear();
            foreach (StatCardDataSO statCard in statCards.StatCards)
                Cards.Add(statCard);
        }
        
        public void AddCard(StatCardDataSO statCard) => Cards.Add(statCard);
        public void RemoveCard(StatCardDataSO statCard) => Cards.Remove(statCard);
    }
}