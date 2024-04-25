using System;
using System.Collections;
using System.Collections.Generic;
using _Project.Scripts;
using TMPro;
using UnityEngine;

public class Bonus : MonoBehaviour
{
    [SerializeField] private GameObject _getBonus;
    [SerializeField] private GameObject _waitBonus;
    [SerializeField] private TextMeshProUGUI _waitText;
    private DateTime _curDate;
    private float _sec;

    private void Awake()
    {
        _curDate = DateTime.Now;
    }

    private void Update()
    {
        _sec += Time.deltaTime;
        if (_sec > 1)
        {
            _sec = 0;
            _curDate = _curDate.AddSeconds(-1);
            if (_curDate == default)
            {
                _getBonus.SetActive(true);
                _curDate = DateTime.Now;
            }
            _waitText.text = _curDate.ToLongTimeString();
        }
    }

    public void GetBonus()
    {
        PlayerPrefs.SetString("Bonus", DateTime.Now.ToString());
        PlayerData.Amount.Value += 1500;
        PlayerPrefs.SetString("Coin", PlayerData.Amount.ToString());
        PlayerPrefs.Save();
        
        _curDate = default;
        _curDate = _curDate.AddDays(1);
        _curDate = _curDate.AddSeconds(-1);
        _waitText.text = _curDate.ToLongTimeString();
        _getBonus.SetActive(false);
        _waitBonus.SetActive(true);
    }
    
    public void OpenBonus()
    {
        _getBonus.SetActive(false);
        _waitBonus.SetActive(false);
        _curDate = DateTime.Now;
        if (PlayerPrefs.HasKey("Bonus"))
        {
            var time = _curDate - DateTime.Parse(PlayerPrefs.GetString("Bonus"));
            var newtime = new DateTime();
            if (time.TotalDays < 1)
            {
                newtime = newtime.AddDays(1).AddDays(-time.TotalDays);
                _curDate = newtime;
            }
            if (time.TotalDays < 1)
            {
                _waitBonus.SetActive(true);
                _waitText.text = _curDate.ToLongTimeString();
            }
            else
            {
                _getBonus.SetActive(true);
            }
        }
        else
        {
            _getBonus.SetActive(true);
        }
    }
}
