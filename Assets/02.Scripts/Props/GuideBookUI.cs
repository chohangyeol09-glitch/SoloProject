using _02.Scripts.CardSystem;
using _02.Scripts.CardSystem.Cards.ActionCards.EffectSO;
using _02.Scripts.CoreSystem.EventChannel;
using _02.Scripts.CoreSystem.EventChannel.GameEvents;
using _02.Scripts.CoreSystem.ModuleSystem;
using _02.Scripts.InteractionSystem.Interactions;
using EPOOutline;
using TMPro;
using UnityEngine;

namespace _02.Scripts.Props
{
    public class GuideBookUI : ModuleOwner
    {

        [SerializeField] private EventChannelSO cardEvent;
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private TextMeshProUGUI descriptionText;
        
        private Outlinable _outline;
        protected override void InitializeModules()
        {
            base.InitializeModules();
            _outline = GetComponent<Outlinable>();
            cardEvent.AddListener<InfoShowEvent>(HandleInfoShow);
            cardEvent.AddListener<InfoHideEvent>(HandleInfoHide);
            
        }

        private void HandleInfoShow(InfoShowEvent obj)
        {
            titleText.text = obj.Title;
            descriptionText.text = obj.Description;
        }

        private void HandleInfoHide(InfoHideEvent obj)
        {
            titleText.text = "";
            descriptionText.text = "";
        }
    }
}