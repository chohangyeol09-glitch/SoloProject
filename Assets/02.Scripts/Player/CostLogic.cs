using System;
using _02.Scripts.CoreSystem.EventChannel;
using _02.Scripts.CoreSystem.EventChannel.GameEvents;
using _02.Scripts.CoreSystem.ModuleSystem;
using UnityEngine;

namespace _02.Scripts.Player
{
    public class CostLogic : MonoBehaviour, IModule
    {
        [SerializeField] private EventChannelSO playerChannel;
        public event Action<int> OnCostChanged;

        private ModuleOwner _owner;

        public void Initialize(ModuleOwner owner)
        {
            _owner = owner;
            playerChannel.AddListener<SpendCostEvent>(HandleSpendCost);
            playerChannel.AddListener<GainCostEvent>(HandleGainCost);
            playerChannel.AddListener<RecoverCostEvent>(HandleRecover);
        }

        private void OnDestroy()
        {
            playerChannel.RemoveListener<SpendCostEvent>(HandleSpendCost);
            playerChannel.RemoveListener<GainCostEvent>(HandleGainCost);
            playerChannel.RemoveListener<RecoverCostEvent>(HandleRecover);
        }

        private void HandleSpendCost(SpendCostEvent evt)
        {
            if (PlayerDataManager.Instance.CurrentCost < evt.Amount)
            {
                evt.OnResult?.Invoke(false);
                return;
            }
            PlayerDataManager.Instance.CurrentCost -= evt.Amount;
            OnCostChanged?.Invoke(PlayerDataManager.Instance.CurrentCost);
            evt.OnResult?.Invoke(true);
        }

        private void HandleGainCost(GainCostEvent evt)
        {
            PlayerDataManager.Instance.CurrentCost = Mathf.Min(
                PlayerDataManager.Instance.CurrentCost + evt.Amount,
                PlayerDataManager.Instance.MaxCost
            );
            OnCostChanged?.Invoke(PlayerDataManager.Instance.CurrentCost);
        }

        private void HandleRecover(RecoverCostEvent evt)
        {
            PlayerDataManager.Instance.CurrentCost =
                evt.OverrideAmount ?? PlayerDataManager.Instance.MaxCost;
            OnCostChanged?.Invoke(PlayerDataManager.Instance.CurrentCost);
        }
    }
}