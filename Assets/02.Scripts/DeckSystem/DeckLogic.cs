using System.Collections.Generic;
using _02.Scripts.CardSystem.Cards.StatCards;
using _02.Scripts.CoreSystem.EventChannel;
using _02.Scripts.CoreSystem.EventChannel.CardEvent.StatCardEvents;
using _02.Scripts.CoreSystem.EventChannel.GameEvents;
using _02.Scripts.CoreSystem.EventChannel.GameEvents.StageEvents;
using _02.Scripts.CoreSystem.ModuleSystem;
using _02.Scripts.Players;
using UnityEngine;

namespace _02.Scripts.DeckSystem
{
    public class DeckLogic : MonoBehaviour, IModule, IAfterInitializeModule
    {
        [SerializeField] private EventChannelSO cardChannel;
        [SerializeField] private EventChannelSO gameEvent;

        private List<StatCardDataSO> _deckPile    = new();
        private List<StatCardDataSO> _discardPile = new();
        
        public HandLogic HandLogic { get; private set; }

        private ModuleOwner _owner;
        public void Initialize(ModuleOwner owner)
        {
            _owner = owner;
            cardChannel.AddListener<DiscardCardEvent>(HandleDiscard);
            gameEvent.AddListener<StageStartEvent>(HandleStartStage);
        }
        
        public void AfterInitialize()
        {
            HandLogic = _owner.transform.GetComponent<GameManager>().HandLogic;
               
        }

        private void HandleStartStage(StageStartEvent evt)
        {
            _deckPile.Clear();
            _discardPile.Clear();
            foreach (StatCardDataSO card in Player.Instance.RuntimeDeck.Cards)
                _deckPile.Add(card);

            Shuffle(_deckPile);
            RaiseDeckCountChanged();
        }

        private void OnDestroy()
        {
            cardChannel.RemoveListener<DiscardCardEvent>(HandleDiscard);
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

            RaiseDeckCountChanged();
            return drawn;
        }

        private void HandleDiscard(DiscardCardEvent evt)
        {
            _discardPile.Add(evt.cardData);
            RaiseDeckCountChanged();
        }

        private void Refill()
        {
            _deckPile.AddRange(_discardPile);
            _discardPile.Clear();
            Shuffle(_deckPile);
        }

        private void RaiseDeckCountChanged()
        {
            cardChannel.RaiseEvent(new PileCountChangedEvent().Init(_deckPile.Count, _discardPile.Count));
            
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
        /*[ContextMenu("TestInitStage")]
        private void TestInitStage() => InitializeStage();*/

        [Header("Test")] [SerializeField] private StatCardDataSO data;
        [ContextMenu("AddDeck")]
        private void TestAddDeck()
        {
            Player.Instance.RuntimeDeck.AddCard(data);
        }
#endif

        
    }
}
    

