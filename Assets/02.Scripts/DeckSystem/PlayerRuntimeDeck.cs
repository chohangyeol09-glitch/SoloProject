using System;
using System.Collections.Generic;
using _02.Scripts.CardSystem.Cards.StatCards;

namespace _02.Scripts.DeckSystem
{
    public class PlayerRuntimeDeck
    {
        public List<StatCardDataSO> Cards { get; private set; } = new();
        public event Action<int> OnDeckCountChanged;

        public void Initialize(StartStatCardListSO statCards)
        {
            Cards.Clear();
            foreach (StatCardDataSO statCard in statCards.StatCards)
                Cards.Add(statCard);
            OnDeckCountChanged?.Invoke(Cards.Count);
        }

        public void AddCard(StatCardDataSO statCard)
        {
            Cards.Add(statCard);
            OnDeckCountChanged?.Invoke(Cards.Count);
        }

        public void RemoveCard(StatCardDataSO statCard)
        {
            Cards.Remove(statCard);
            OnDeckCountChanged?.Invoke(Cards.Count);
        }
    }
}