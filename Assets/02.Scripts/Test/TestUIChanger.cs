using System;
using _02.Scripts;
using TMPro;
using UnityEngine;

public class TestUIChanger : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private TextMeshProUGUI healthText;
    
    private void Awake()
    {
        PlayerManager.Instance.OnChangeCost += HandleChangeCost;
        PlayerManager.Instance.OnChangeHealth += HandleChangeHealth;
    }

    private void HandleChangeCost(int value)
    {
        costText.text = "Cost: " + value;
    }

    private void HandleChangeHealth(int value)
    {
        healthText.text = "Health: " + value;
    }
}
