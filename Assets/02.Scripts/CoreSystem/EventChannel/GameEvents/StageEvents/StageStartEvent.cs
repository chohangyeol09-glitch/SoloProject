using _02.Scripts.Enemy;

namespace _02.Scripts.CoreSystem.EventChannel.GameEvents.StageEvents
{
    public class StageStartEvent : GameEvent
    {
        public int StageIndex;
        public EnemyDataSO EnemyData;

        public StageStartEvent Init(int index, EnemyDataSO enemyData)
        {
            StageIndex = index;
            EnemyData = enemyData;
            return this;
        }
    }
}