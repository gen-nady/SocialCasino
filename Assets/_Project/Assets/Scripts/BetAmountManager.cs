using System.Collections.Generic;
using _Project.Scripts;
using UnityEngine;

public class BetAmountManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> _selected;

    private List<decimal> betAmount = new List<decimal> { 100, 500, 1000 };

    private int cur = 0;
    public static BetAmountManager instance{get; private set;}
    

    void Awake()
    {
        instance = this;
    }

    public void Secelt(int i)
    {
        if(cur == i) return;
        foreach (var s in _selected)
            s.SetActive(false);
        _selected[i].SetActive(true);
        cur = i;
    }
    
    public void PlaceBet()
    {
        if(PlayerData.Amount.Value >= betAmount[cur])
        {
            PlayerData.Amount.Value -= betAmount[cur];
            Debug.Log("BEt = "+ betAmount[cur]);
            BallSpawner.instance.SpawnBall(betAmount[cur]);
        }
    }

}
