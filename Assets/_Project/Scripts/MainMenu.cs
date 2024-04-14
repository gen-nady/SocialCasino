using System.Collections.Generic;
using _Project.Scripts;
using _Project.Scripts.Helpers;
using TMPro;
using UniRx;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject _close;
    [SerializeField] private GameObject _profile;
    [SerializeField] private TextMeshProUGUI _coinText;
    [SerializeField] private List<GameObject> _buttons;
    [SerializeField] private List<GameObject> _panels;
    [SerializeField] private Bonus _bonusPanel;
    [SerializeField] private GameObject _bottomPanels;

    private void Awake()
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
        if (i == 5)
        {
            _panels[i].SetActive(true);
            _close.SetActive(true);
            _profile.SetActive(false);
            _bottomPanels.SetActive(false);
            return;
        }
        _bottomPanels.SetActive(true);
        if (i == 4)
        {
            _panels[i].SetActive(true);
            return;
        }
        if (i == 3)
        {
            _panels[4].SetActive(false);
            _panels[i].SetActive(true);
            return;
        }
        foreach (var b in _buttons)
            b.SetActive(false); 
        foreach (var p in _panels)
            p.SetActive(false);
        
        _buttons[i].SetActive(true);
        _panels[i].SetActive(true);

        _close.SetActive(i is 0 or 2);
        _profile.SetActive(i == 1);
        if(i == 2)
            _bonusPanel.OpenBonus();
    }
}
