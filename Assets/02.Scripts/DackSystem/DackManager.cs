using System.Collections.Generic;
using _02.Scripts.CardSystem.Cards;
using _02.Scripts.CardSystem.Cards.StatCards;
using UnityEngine;

public class DackManager : MonoBehaviour
{
    [SerializeField] private GameObject statCardPrefab;
    [SerializeField] private StartStatCardListSO startStatCardListSo;
    private List<StatCardDataSO> _allDack = new();
    private List<StatCardDataSO> _currentDack;


#if UNITY_EDITOR

    [SerializeField] private StatCardDataSO testAddCard;
    
    [ContextMenu("TestStart")]
    public void TestStart()
    {
        foreach (StatCardDataSO statData in startStatCardListSo.StatCards)
        {
            GameObject obj = Instantiate(statCardPrefab);
            StatCard statCard = obj.GetComponent<StatCard>();
            statCard.SetStatData(statData);
            
            _allDack.Add(statData);
        }
        
        _allDack.Add(testAddCard);
        
        _allDack = ShuffleList<StatCardDataSO>(_allDack);
        
    }

    [ContextMenu("TestDrow")]
    public void TestDrow()
    {
        GameObject obj = Instantiate(statCardPrefab);
        StatCard statCard = obj.GetComponent<StatCard>();
        statCard.SetStatData(_allDack[0]);
        _allDack.RemoveAt(0);
    }
    private List<T> ShuffleList<T>(List<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            
            (list[k], list[n]) = (list[n], list[k]);
        }
        
        return list;
    }
}
    
#endif

