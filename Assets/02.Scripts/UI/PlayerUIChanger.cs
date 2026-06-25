using System;
using _02.Scripts.CoreSystem.EventChannel;
using _02.Scripts.CoreSystem.EventChannel.GameEvents;
using _02.Scripts.CoreSystem.EventChannel.PlayerEvents;
using TMPro;
using UnityEngine;

namespace _02.Scripts.UI
{
    public class PlayerUIChanger : MonoBehaviour
    {
        [SerializeField] private EventChannelSO playerEventChannel;
        //[SerializeField] private EventChannelSO cardChannel;
        [SerializeField] private TextMeshProUGUI costText;
        [SerializeField] private TextMeshProUGUI healthText;
        [SerializeField] private TextMeshProUGUI deckCountText;
        //[SerializeField] private TextMeshProUGUI deckPileText;
        //[SerializeField] private TextMeshProUGUI discardPileText;
        

        private void Awake()
        {
            playerEventChannel.AddListener<CostChangedEvent>(HandleChangeCost);
            playerEventChannel.AddListener<HealthChangedEvent>(HandleChangeHealth);
            playerEventChannel.AddListener<DeckCountChangedEvent>(HandleDeckCountChanged);
            //cardChannel.AddListener<PileCountChangedEvent>(HandlePileCountChanged);
            
        }
        
        private void OnDestroy()
        {
            playerEventChannel.RemoveListener<CostChangedEvent>(HandleChangeCost);
            playerEventChannel.RemoveListener<HealthChangedEvent>(HandleChangeHealth);
            playerEventChannel.RemoveListener<DeckCountChangedEvent>(HandleDeckCountChanged);
            //cardChannel.RemoveListener<PileCountChangedEvent>(HandlePileCountChanged);
        }

        private void HandleChangeCost(CostChangedEvent evt)
            => costText.text = evt.CurrentCost.ToString();

        private void HandleChangeHealth(HealthChangedEvent evt)
            => healthText.text = evt.CurrentHealth + "/" + evt.MaxHealth;

        private void HandleDeckCountChanged(DeckCountChangedEvent evt)
        {
            if (deckCountText != null)
                deckCountText.text = evt.CurrentCount.ToString();
        }
        
        /*private void HandlePileCountChanged(PileCountChangedEvent evt)
        {
            Debug.Log(evt.DeckCount);
            deckPileText.text = evt.DeckCount.ToString();
            discardPileText.text = evt.DiscardCount.ToString();
        }*/
    }
}