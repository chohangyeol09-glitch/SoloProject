using System.Collections.Generic;
using _02.Scripts.CardSystem.Cards.StatCards;
using _02.Scripts.CoreSystem.EventChannel;
using _02.Scripts.CoreSystem.EventChannel.GameEvents.StatCardEvent;
using _02.Scripts.CoreSystem.ModuleSystem;
using _02.Scripts.Player;
using UnityEditor;
using UnityEngine;

namespace _02.Scripts.DeckSystem
{
    public class DeckLogic : MonoBehaviour, IModule
    {
        [SerializeField] private EventChannelSO cardChannel;

        private List<StatCardDataSO> _deckPile    = new();
        private List<StatCardDataSO> _discardPile = new();

        public void Initialize(ModuleOwner owner)
        {
            cardChannel.AddListener<DiscardCardEvent>(HandleDiscard);
        }
        
        private void OnDestroy()
        {
            cardChannel.RemoveListener<DiscardCardEvent>(HandleDiscard);
        }
        
        public void InitializeStage()
        {
            _deckPile.Clear();
            _discardPile.Clear();

            foreach (StatCardDataSO data in PlayerDataManager.Instance.RuntimeDeck.Cards)
                _deckPile.Add(data);

            Shuffle(_deckPile);
        }

        public List<StatCardDataSO> DrawCards(int count)
        {
            var drawn = new List<StatCardDataSO>();
            for (int i = 0; i < count; i++)
            {
                if (_deckPile.Count == 0)
                {
                    if (_discardPile.Count == 0) break;
                    Refill();
                }
                drawn.Add(_deckPile[0]);
                _deckPile.RemoveAt(0);
            }
            return drawn;
        }

        private void HandleDiscard(DiscardCardEvent evt)
            => _discardPile.Add(evt.cardData);

        private void Refill()
        {
            _deckPile.AddRange(_discardPile);
            _discardPile.Clear();
            Shuffle(_deckPile);
        }

        private void Shuffle(List<StatCardDataSO> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = Random.Range(0, n + 1);
                (list[k], list[n]) = (list[n], list[k]);
            }
        }
#if UNITY_EDITOR
        [ContextMenu("TestInitStage")]
        private void TestInitStage() => InitializeStage();

        [Header("Test")] [SerializeField] private StatCardDataSO data;
        [ContextMenu("AddDeck")]
        private void TestAddDeck()
        {
            PlayerDataManager.Instance.RuntimeDeck.AddCard(data);
        }
#endif
    }
}
    

