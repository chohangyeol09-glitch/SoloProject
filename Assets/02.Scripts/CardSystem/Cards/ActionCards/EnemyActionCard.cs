using System.Collections;
using System.Collections.Generic;
using _02.Scripts.CardSystem.Cards.ActionCards.EffectSO;
using _02.Scripts.CoreSystem.EventChannel;
using _02.Scripts.CoreSystem.EventChannel.CardEvent.StatCardEvents;
using _02.Scripts.CoreSystem.EventChannel.EnemyEvents;
using _02.Scripts.UI;
using UnityEngine;

namespace _02.Scripts.CardSystem.Cards.ActionCards
{
    public class EnemyActionCard : ActionCard
    {
        [SerializeField] private EventChannelSO slotChannel;

        private ActionCardUIChanger _uiChanger;

        protected override void InitializeModules()
        {
            base.InitializeModules();
            _uiChanger = GetModule<ActionCardUIChanger>();
        }

        public void GrantEffect(AbstractActionEffectSO effect)
        {
            if (effect == null) return;
            AddRuntimeEffect(effect, effect.Grade);
            _uiChanger?.AddEffectIcon(effect, effect.Grade);
        }

        protected override void OnDefenseZero()
        {
            StartCoroutine(DestroyAfterShake());
        }

        private IEnumerator DestroyAfterShake()
        {
            yield return new WaitForSeconds(0.5f);
            slotChannel?.RaiseEvent(new CardPickUpEvent().Init(this));
            enemyChannel?.RaiseEvent(new EnemyCardDestroyedEvent().Init(this));
            Destroy(gameObject);
        }
    }
}