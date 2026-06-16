using UnityEngine;

namespace _02.Scripts.CoreSystem.EventChannel.GameEvents
{
    public class InfoShowEvent : GameEvent
    {
        public string Title;
        public string Description;

        public InfoShowEvent Init(string title, string description)
        {
            Title = title;
            Description = description;
            return this;
        }
    }
}