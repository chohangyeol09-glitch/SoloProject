using UnityEngine;

namespace _02.Scripts.Card
{
    [CreateAssetMenu(fileName = "CardData", menuName = "Scriptable Objects/CardData")]
    public class CardData : ScriptableObject
    {
        [field: SerializeField] public string CardName { get; set; }
        [field: SerializeField] public int Damage { get; set; }
    }
}
