using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using _02.Scripts.CardSystem.Cards.ActionCards;
using _02.Scripts.CardSystem.Cards.ActionCards.EffectSO;
using _02.Scripts.CoreSystem.EventChannel;
using _02.Scripts.CoreSystem.EventChannel.CardEvent.ActionCardEvents;
using _02.Scripts.CoreSystem.EventChannel.EnemyEvents;
using _02.Scripts.CoreSystem.ModuleSystem;
using _02.Scripts.Enemys;
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
            => ExecuteAndNotify(evt.OnComplete).Forget();

        private async UniTaskVoid ExecuteAndNotify(Action onComplete)
        {
            await ExecuteCardsInOrder();
            onComplete?.Invoke();
        }

        private async UniTask ExecuteCardsInOrder()
        {
            foreach (PlayerSlot slot in _slotLogic.PlayerSlots)
            {
                if (slot.CurrentCard == null) continue;
                await ExecuteCard(slot);
            }

            if (Enemy.Instance.IsDead) return;   

            await RaiseEnemyActionStart();

            foreach (EnemySlot slot in _slotLogic.EnemySlots)
            {
                if (slot.CurrentCard == null) continue;
                await ExecuteCard(slot);
            }

            await RaiseEnemyActionEnd(); //end삭제하기? 
        }

        private UniTask RaiseEnemyActionStart()
        {
            UniTaskCompletionSource tcs = new UniTaskCompletionSource();
            enemyChannel.RaiseEvent(new EnemyActionStartEvent().Init(() => tcs.TrySetResult()));
            return tcs.Task;
        }

        private UniTask RaiseEnemyActionEnd()
        {
            UniTaskCompletionSource tcs = new UniTaskCompletionSource();
            enemyChannel.RaiseEvent(new EnemyActionEndEvent().Init(() => tcs.TrySetResult()));
            return tcs.Task;
        }

        private async UniTask ExecuteCard(AbstractSlot slot)
        {
            ActionCard card = slot.CurrentCard;
            List<AbstractSlot> targets = _slotLogic.GetTargetSlots(slot, card.ActionCardData.TargetRangeType);

            EffectExecuteContext context = new EffectExecuteContext(card, targets);

            foreach (AbstractActionEffectSO effect in card.GetBeforeEffects())
                if (effect.IsActivate(context))
                    await effect.Apply(context, targets);

            await card.ActionCardData.Action.Execute(card, targets);

            foreach (AbstractActionEffectSO effect in card.GetAfterEffects())
                if (effect.IsActivate(context))
                    await effect.Apply(context, targets);
        }

        
    }
}