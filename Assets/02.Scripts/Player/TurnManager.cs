using _02.Scripts.CoreSystem.EventChannel;
using _02.Scripts.CoreSystem.EventChannel.GameEvents;
using UnityEngine;

namespace _02.Scripts.Player
{
    public class TurnManager : MonoBehaviour
    {
        public int TurnCount { get; private set; }

        [SerializeField] private EventChannelSO turnEventChannel;

        [ContextMenu("TurnStart")]
        public void TurnStart()
        {
            TurnCount = 1;
            Debug.Log("Turn Start");
            turnEventChannel.RaiseEvent(new TurnStartEvent().Init(TurnCount));
        }
    }
}