using System.Collections.Generic;
using _02.Scripts.CardSystem;

namespace _02.Scripts.CoreSystem.EventChannel.GameEvents.StageEvents
{
    public class StageClearEvent : GameEvent
    {
        public List<RewardEntry> Rewards;

        public StageClearEvent Init(List<RewardEntry> rewards)
        {
            Rewards = rewards;
            return this;
        }
    }
}
