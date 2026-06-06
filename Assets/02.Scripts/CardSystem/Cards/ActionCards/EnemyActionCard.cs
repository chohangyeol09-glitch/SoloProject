using System.Collections;
using System.Collections.Generic;
using _02.Scripts.CoreSystem.EventChannel;
using _02.Scripts.CoreSystem.EventChannel.CardEvent.StatCardEvents;
using UnityEngine;

namespace _02.Scripts.CardSystem.Cards.ActionCards
{
    public class EnemyActionCard : ActionCard
    {
        [SerializeField] private EventChannelSO slotChannel;

        protected override void OnDefenseZero()
        {
            StartCoroutine(DestroyAfterShake());
        }

        private IEnumerator DestroyAfterShake()
        {
            yield return new WaitForSeconds(0.5f);
            slotChannel?.RaiseEvent(new CardPickUpEvent().Init(this));
            Destroy(gameObject);
        }
    }
}