using _02.Scripts.CardSystem.Cards.ActionCards;
using _02.Scripts.SlotSystem.Slots;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace _02.Scripts.Enemys.Boss.BossPatterns
{
    [CreateAssetMenu(fileName = "SummonCardPattern", menuName = "Enemy/Pattern/SummonCard")]
    public class SummonCardPatternSO : AbstractEnemyPatternSO
    {
        [field: SerializeField] public ActionCardDataSO CardData { get; private set; }
        [field: SerializeField] public int AttackValue { get; private set; }
        [field: SerializeField] public int DefenseValue { get; private set; }

        protected override UniTask ExecutePattern(EnemyPatternContext context)
        {
            Debug.Log($"SUMMON| Execute: {context.CurrentTurn}");
            context.SlotLogic.SpawnEnemyCardInFirstEmptySlot(CardData, AttackValue, DefenseValue);
            return UniTask.CompletedTask;
        }
    }
}