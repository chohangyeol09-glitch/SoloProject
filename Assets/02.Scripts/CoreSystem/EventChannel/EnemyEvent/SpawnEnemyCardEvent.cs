using System.Collections.Generic;
using _02.Scripts.Enemy;

namespace _02.Scripts.CoreSystem.EventChannel.EnemyEvent
{
    public class SpawnEnemyCardEvent : GameEvent
    {
        public List<EnemyCardPlacement> CardPlacements;

        public SpawnEnemyCardEvent Init(List<EnemyCardPlacement> placements)
        {
            CardPlacements = placements;
            return this;
        }
    }

    public class ClearEnemyCardEvent : GameEvent { }
}