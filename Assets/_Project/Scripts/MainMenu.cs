using System;
using System.Collections;
using System.Collections.Generic;
using _Project.Scripts;
using _Project.Scripts.Helpers;
using TMPro;
using UniRx;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _coinText;
    [SerializeField] private List<GameObject> _buttons;
    [SerializeField] private List<GameObject> _panels;
    [SerializeField] private Bonus _bonusPanel;

    private void Start()
    {
        PlayerData.Amount.ObserveEveryValueChanged(_ => _.Value)
            .Subscribe(BalanceChanged)
            .AddTo(this);
        if (PlayerPrefs.HasKey("Coin"))
        {
            PlayerData.Amount.Value = ParseConverter.DecimalParse(PlayerPrefs.GetString("Coin"));
        }
        else
        {
            PlayerData.Amount.Value = 5000;
            PlayerPrefs.SetString("Coin", PlayerData.Amount.ToString());
            PlayerPrefs.Save();
        }
    }

    private void BalanceChanged(decimal amount)
    {
        _coinText.text = amount.ToString();
    }
    
    public void Selected(int i)
    {
        foreach (var b in _buttons)
            b.SetActive(false); 
        foreach (var p in _panels)
            p.SetActive(false);
        
        _buttons[i].SetActive(true);
        _panels[i].SetActive(true);
        if(i == 2)
            _bonusPanel.OpenBonus();
    }
}
