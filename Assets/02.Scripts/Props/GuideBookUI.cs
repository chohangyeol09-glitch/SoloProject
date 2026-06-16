using _02.Scripts.CardSystem;
using _02.Scripts.CardSystem.Cards.ActionCards.EffectSO;
using _02.Scripts.CoreSystem.ModuleSystem;
using _02.Scripts.InteractionSystem.Interactions;
using EPOOutline;
using UnityEngine;

namespace _02.Scripts.Props
{
    public class GuideBookUI : ModuleOwner
    {
        public PropInteraction PropInteraction { get; private set; }

        [SerializeField] private Transform layout;
        [SerializeField] private Transform GuideUI;
        [SerializeField] private GameObject contentPrefab;
        [SerializeField] private CardGradePoolSO actionEffectList;

        private Outlinable _outline;
        protected override void InitializeModules()
        {
            base.InitializeModules();
            PropInteraction = GetModule<PropInteraction>();
            _outline = GetComponent<Outlinable>();
            CreateContent();
        }

        protected override void AfterInitializeModules()
        {
            base.AfterInitializeModules();
            PropInteraction.OnClick += HandleClick;
        }

        private void HandleClick()
        {
            GuideUI.gameObject.SetActive(true);
        }

        public void CanvasClose()
        {
            GuideUI.gameObject.SetActive(false);
        }

        private void CreateContent()
        {
            foreach (AbstractActionEffectSO data in actionEffectList.GetEffectsByGrade(CardGrade.BRONZE))
                CreateContent(data);

            foreach (AbstractActionEffectSO data in actionEffectList.GetEffectsByGrade(CardGrade.SILVER))
                CreateContent(data);
            
            foreach (AbstractActionEffectSO data in actionEffectList.GetEffectsByGrade(CardGrade.GOLD))
                CreateContent(data);
            
            GuideUI.gameObject.SetActive(false);
        }

        private void CreateContent(AbstractActionEffectSO data)
        {
            GameObject content = Instantiate(contentPrefab, layout);
            content.GetComponent<GuideContentUI>().SetUI(data);
        }
    }
}