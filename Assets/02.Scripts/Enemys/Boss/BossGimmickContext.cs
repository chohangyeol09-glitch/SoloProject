using _02.Scripts.CoreSystem.EventChannel;
using _02.Scripts.SlotSystem;

namespace _02.Scripts.Enemys.Boss
{
    public class BossGimmickContext
    {
        public int CurrentTurn;
        public int CurrentHealth;
        public int MaxHealth;
        public SlotLogic SlotLogic;
    }
}