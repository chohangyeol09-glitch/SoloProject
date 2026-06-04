using System;
using _02.Scripts;
using TMPro;
using UnityEngine;

public class TestUIChanger : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI costText;
    
    /*private void Awake()
    {
        PlayerManager.Instance.OnChangeCost += HandleChangeCost;
    }

    private void HandleChangeCost(int value)
    {
        costText.text = "Cost: " + value;
    }*/

}
