using System;
using System.Collections.Generic;
using _Project.Scripts;
using _Project.Scripts.Helpers;
using TMPro;
using UniRx;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.SocialPlatforms.GameCenter;
using Task = System.Threading.Tasks.Task;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject _loading;
    [SerializeField] private GameObject _socSlot;
    [SerializeField] private GameObject _close;
    [SerializeField] private GameObject _profile;
    [SerializeField] private TextMeshProUGUI _coinText;
    [SerializeField] private List<GameObject> _buttons;
    [SerializeField] private List<GameObject> _panels;
    [SerializeField] private Bonus _bonusPanel;
    [SerializeField] private GameObject _bottomPanels;
    [SerializeField] private List<CanvasGroup> _ach;

    private async void Awake()
    {
        PlayerData.Amount.ObserveEveryValueChanged(_ => _.Value)
            .Subscribe(BalanceChanged)
            .AddTo(this);  
        PlayerData.WinCount.ObserveEveryValueChanged(_ => _.Value)
            .Subscribe(WinCount)
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
        if (!PlayerPrefs.HasKey("bn1"))
        {
            PlayerPrefs.SetInt("bn1", 0);
            PlayerPrefs.Save();
        }
        if (!PlayerPrefs.HasKey("bn2"))
        {
            PlayerPrefs.SetString("bn2", "0");
            PlayerPrefs.Save();
        }
        if (!PlayerPrefs.HasKey("bn3"))
        {
            PlayerPrefs.SetInt("bn3", 0);
            PlayerPrefs.Save();
        }
        if (!PlayerPrefs.HasKey("bn4"))
        {
            PlayerPrefs.SetInt("bn4", 0);
            PlayerPrefs.Save();
        }
        if (!PlayerPrefs.HasKey("bn4perDay"))
        {
            PlayerPrefs.SetInt("bn4perDay", 0);
            PlayerPrefs.Save();
        }

        if (PlayerPrefs.HasKey("LastOpenDay"))
        {
            var date = DateTime.Parse(PlayerPrefs.GetString("LastOpenDay"));
            if (date.Day < DateTime.Now.Day && date.Month <= DateTime.Now.Day && date.Year <= DateTime.Now.Year)
            {
                PlayerPrefs.SetInt("bn4perDay", 0);
                PlayerPrefs.Save();
            }
            PlayerPrefs.SetString("LastOpenDay", DateTime.Now.ToString());
            PlayerPrefs.Save();
        }
        else
        {
            PlayerPrefs.SetString("LastOpenDay", DateTime.Now.ToString());
            PlayerPrefs.Save();
        }

        await Task.Delay(4000);
        _loading.SetActive(false);
    }

    public void ClisrWarning()
    {
        _socSlot.SetActive(false);
    }
    
    private void WinCount(int count)
    {
        if (count > 9)
        {
            PlayerPrefs.SetInt("bn3", 1);
            PlayerPrefs.Save();
        }
    }
    
    private void BalanceChanged(decimal amount)
    {
        _coinText.text = CountValuesConverter.From1000toK(amount);
    }
    
    public void Selected(int i)
    {
        
        if (i == 6)
        {
            _panels[i].SetActive(true);
            _close.SetActive(true);
            _profile.SetActive(false);
            _bottomPanels.SetActive(false);
            return;
        }
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
            var bn1 = PlayerPrefs.GetInt("bn1");
            var bn2 = PlayerPrefs.GetString("bn2");
            var bn3 = PlayerPrefs.GetInt("bn3");
            var bn4 = PlayerPrefs.GetInt("bn4");
            if (bn1 > 1)
                _ach[0].alpha = 1;
            if (ParseConverter.DecimalParse(bn2) >= 1000000)
                _ach[1].alpha = 1;
            if (bn3 == 1)
                _ach[2].alpha = 1;
            if (bn4 == 1)
                _ach[3].alpha = 1;
            
                
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
