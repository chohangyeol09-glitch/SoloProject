
using UnityEngine;

namespace _02.Scripts.CardSystem.Cards.ActionCards.ActionWeapon
{
    public class AbstractWeaponSO : ScriptableObject
    {
        [field: SerializeField] public GameObject WeaponPrefab { get; private set; }
        [field: SerializeField] public string AttackTrigger { get; private set; }
        [field: SerializeField] public float AttackDuration { get; private set; } = 1f;
        [field: SerializeField] public Vector3 SpawnOffset { get; private set; }
        [field: SerializeField] public AnimationClip AttackClip { get; private set; }
    }
}