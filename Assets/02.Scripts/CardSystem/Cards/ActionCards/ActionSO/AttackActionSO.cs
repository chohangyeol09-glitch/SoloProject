using System;
using System.Collections.Generic;
using _02.Scripts.CardSystem.Cards.ActionCards.ActionWeapon;
using _02.Scripts.CoreSystem.EventChannel;
using _02.Scripts.CoreSystem.EventChannel.CardEvent.ActionCardEvents;
using _02.Scripts.CoreSystem.EventChannel.EnemyEvent;
using _02.Scripts.SlotSystem;
using UnityEngine;

namespace _02.Scripts.CardSystem.Cards.ActionCards.ActionSO
{
    [CreateAssetMenu(fileName = "AttackAction", menuName = "Card/Action/AttackAction")]
    public class AttackActionSO : AbstractActionSO
    {
        [SerializeField] private EventChannelSO enemyEventChannel;

        private static int _attackIdCounter = 0;

        public override void Execute(ActionCard card, List<AbstractSlot> targets, Action onComplete = null)
        {
            if (targets == null || targets.Count == 0 || card.AttackValue <= 0)
            {
                onComplete?.Invoke();
                return;
            }

            AbstractWeaponSO abstractWeapon = card.ActionCardData.AbstractWeapon;
            if (abstractWeapon == null || abstractWeapon.WeaponPrefab == null)
            {
                onComplete?.Invoke();
                return;
            }

            ExecuteNext(card, targets, abstractWeapon, 0, onComplete);
        }

        private void ExecuteNext(ActionCard card, List<AbstractSlot> targets, AbstractWeaponSO weapon, int index,
            Action onComplete)
        {
            if (index >= targets.Count)
            {
                onComplete?.Invoke();
                return;
            }

            AbstractSlot target = targets[index];
            int attackId = _attackIdCounter++;

            if (card is EnemyActionCard)
                enemyEventChannel.RaiseEvent(new EnemyAttackStartEvent().Init(weapon.AttackDuration));

            Vector3 spawnPos = target.transform.position + weapon.SpawnOffset;
            GameObject weaponObj = Instantiate(weapon.WeaponPrefab, spawnPos, Quaternion.identity);
            WeaponAnimHandler handler = weaponObj.GetComponent<WeaponAnimHandler>();
            Animator animator = weaponObj.GetComponent<Animator>();
            handler.SetId(attackId);

            void OnHit(WeaponHitEvent evt)
            {
                if (evt.Id != attackId) return;
                enemyEventChannel.RemoveListener<WeaponHitEvent>(OnHit);

d                if (target.CurrentCard != null)
                    target.CurrentCard.TakeDamage(card.AttackValue);
                else
                {
                    if (target.SlotType == SlotType.Player)
                        card.RaisePlayerDamage(card.AttackValue);
                    else if (target.SlotType == SlotType.Enemy)
                        card.RaiseEnemyDamage(card.AttackValue);
                }
            }
            
            void OnAttackEnd(WeaponAttackEndEvent evt)
            {
                if (evt.Id != attackId) return;
                enemyEventChannel.RemoveListener<WeaponAttackEndEvent>(OnAttackEnd);

                Destroy(weaponObj);
                ExecuteNext(card, targets, weapon, index + 1, onComplete);
            }

            enemyEventChannel.AddListener<WeaponHitEvent>(OnHit);
            enemyEventChannel.AddListener<WeaponAttackEndEvent>(OnAttackEnd);

            animator?.SetTrigger(weapon.AttackTrigger);
        }
    }
}