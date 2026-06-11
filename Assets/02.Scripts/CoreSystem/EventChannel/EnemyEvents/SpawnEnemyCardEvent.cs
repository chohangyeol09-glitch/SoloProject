using System.Collections.Generic;
using _02.Scripts.Enemys;

namespace _02.Scripts.CoreSystem.EventChannel.EnemyEvents
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