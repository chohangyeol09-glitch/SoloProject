using System;
using System.Collections;
using System.Collections.Generic;
using _02.Scripts.CardSystem.Cards.ActionCards;
using _02.Scripts.CardSystem.Cards.ActionCards.EffectSO;
using _02.Scripts.CoreSystem.EventChannel;
using _02.Scripts.CoreSystem.EventChannel.CardEvent.ActionCardEvents;
using _02.Scripts.CoreSystem.EventChannel.EnemyEvents;
using _02.Scripts.CoreSystem.ModuleSystem;
using _02.Scripts.SlotSystem.Slots;
using UnityEngine;

namespace _02.Scripts.SlotSystem
{
    public class BattleLogic : MonoBehaviour, IModule, IAfterInitializeModule
    {
        [SerializeField] private EventChannelSO turnChannel;
        [SerializeField] private EventChannelSO enemyChannel;
        
        private SlotLogic _slotLogic;
        private ModuleOwner _owner;


        public void Initialize(ModuleOwner owner)
        {
            _owner = owner;
            turnChannel.AddListener<ExecuteCardsEvent>(HandleExecuteCards);
        }
        
        public void AfterInitialize()
        {
            _slotLogic = _owner.transform.GetComponent<GameManager>().SlotLogic;
        }

        private void OnDestroy()
        {
            turnChannel.RemoveListener<ExecuteCardsEvent>(HandleExecuteCards);
        }

        private void HandleExecuteCards(ExecuteCardsEvent evt)
            => StartCoroutine(ExecuteAndNotify(evt.OnComplete));

        private IEnumerator ExecuteAndNotify(Action onComplete)
        {
            yield return StartCoroutine(ExecuteCardsInOrder());
            onComplete?.Invoke();
        }

        private IEnumerator ExecuteCardsInOrder()
        {
            foreach (PlayerSlot slot in _slotLogic.PlayerSlots)
            {
                if (slot.CurrentCard == null) continue;
                bool done = false;
                ExecuteCard(slot, () => done = true);
                yield return new WaitUntil(() => done);
            }

            bool actionStartDone = false;
            enemyChannel.RaiseEvent(new EnemyActionStartEvent().Init(() => actionStartDone = true));
            yield return new WaitUntil(() => actionStartDone);

            int totalPlayerDamage = 0;
            foreach (EnemySlot slot in _slotLogic.EnemySlots)
            {
                if (slot.CurrentCard == null) continue;
                bool done = false;
                ExecuteCard(slot, () => done = true, overflow => totalPlayerDamage += overflow);
                yield return new WaitUntil(() => done);
            }

            if (totalPlayerDamage > 0)
            {
                bool attackDone = false;
                enemyChannel.RaiseEvent(new EnemyAttackStartEvent().Init(
                    totalPlayerDamage,
                    () => attackDone = true
                ));
                yield return new WaitUntil(() => attackDone);
            }

            bool actionEndDone = false;
            enemyChannel.RaiseEvent(new EnemyActionEndEvent().Init(() => actionEndDone = true));
            yield return new WaitUntil(() => actionEndDone);
        }
        
        private void ExecuteCard(AbstractSlot slot, Action onComplete, Action<int> onPlayerDamage = null)
        {
            ActionCard card = slot.CurrentCard;
            List<AbstractSlot> targets = _slotLogic.GetTargetSlots(slot, card.ActionCardData.TargetRangeType);

            EffectExecuteContext context = new EffectExecuteContext(card, targets);

            foreach (AbstractActionEffectSO effect in card.GetBeforeEffects())
                if (effect.IsActivate(context))
                    effect.Apply(context, targets);

            card.ActionCardData.Action.Execute(card, targets, () =>
            {
                List<AbstractActionEffectSO> afterEffects = new List<AbstractActionEffectSO>();
                foreach (AbstractActionEffectSO e in card.GetAfterEffects())
                    if (e.IsActivate(context)) afterEffects.Add(e);

                ApplyEffectsSequential(afterEffects, context, targets, 0, onComplete);
            }, onPlayerDamage);
        }

        private void ApplyEffectsSequential(List<AbstractActionEffectSO> effects, EffectExecuteContext context, List<AbstractSlot> targets, int index, Action onComplete)
        {
            if (index >= effects.Count) { onComplete?.Invoke(); return; }
            effects[index].Apply(context, targets, () => ApplyEffectsSequential(effects, context, targets, index + 1, onComplete));
        }

        
    }
}