using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using _Project.Scripts.Helpers;
using DG.Tweening;
using TMPro;
using UniRx;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.Vegas
{
    public class RollerManager : MonoBehaviour
    {
        public Roller[] Rollers;
        [SerializeField] private Button _spinButton;
        [SerializeField] private TextMeshProUGUI _tetBet;
        [SerializeField] private List<GameObject> _winPanels;
        private const float _delayBetweenRollersInSeconds = 0.1f;
        private decimal _curBet = 1000;

        public void AddBet(int amount)
        {
            if (_curBet + amount < 100)
            {
                _curBet = 100;
            }
            else if (_curBet + amount > PlayerData.Amount.Value)
            {
                if(amount < 0)
                    _curBet += amount;
            }
            else
            {
                _curBet += amount;
            }

            _tetBet.text = _curBet.ToString();
        }
        
        public void StartSpin()
        {
            if (_curBet > PlayerData.Amount.Value)
            {
                return;
            }

            PlayerData.Amount.Value -= _curBet;
            PlayerPrefs.SetString("Coin", PlayerData.Amount.ToString());
            PlayerPrefs.Save();
            var count = PlayerPrefs.GetInt("bn1");
            count++;
            PlayerPrefs.SetInt("bn1", count);
            PlayerPrefs.Save();
            var countSpin = PlayerPrefs.GetInt("bn4perDay");
            countSpin++;
            PlayerPrefs.SetInt("bn4perDay", countSpin);
            PlayerPrefs.Save();
            if (countSpin >= 500)
            {
                PlayerPrefs.SetInt("bn4", 1);
                PlayerPrefs.Save();
            }
            Observable.FromCoroutine(SpinRollers).Subscribe();
        }
        
        private IEnumerator SpinRollers()
        {
            _spinButton.interactable = false;
            // _audioService.Play("Spin Roller", true);
            for (int i = 0; i < Rollers.Length; i++)
            {
                Rollers[i].StartSpin();
            }
            for (uint i = 0; i < Rollers.Length; i++)
            {
                Rollers[i].StartSpinCountdown();
                yield return new WaitWhile(() => Rollers[i].IsSpinning);
                yield return new WaitForSeconds(_delayBetweenRollersInSeconds);
            }

            bool isWin = false;
            for (int j = 0; j < 3; j++)
            {
                if (Rollers[0].Items[j].Type == Rollers[1].Items[j].Type &&
                    Rollers[1].Items[j].Type == Rollers[2].Items[j].Type)
                {
                    _winPanels[2-j].SetActive(true);
                    var s = j;
                    isWin = true;
                    PlayerData.Amount.Value += _curBet * 3;
                    PlayerPrefs.SetString("Coin", PlayerData.Amount.ToString());
                    PlayerPrefs.Save();
                    var count = ParseConverter.DecimalParse(PlayerPrefs.GetString("bn2"));
                    count += _curBet * 3;
                    PlayerPrefs.SetString("bn2", count.ToString());
                    PlayerPrefs.Save();
                    Rollers[0].Items[j].transform.DOScale(1.2f, 0.5f).SetLoops(2, LoopType.Yoyo).OnComplete(() =>
                        {
                            _spinButton.interactable = true;
                            _winPanels[2-s].SetActive(false);
                        });
                    Rollers[1].Items[j].transform.DOScale(1.2f, 0.5f).SetLoops(2, LoopType.Yoyo).OnComplete(() =>
                        {
                            _spinButton.interactable = true;
                            _winPanels[2-s].SetActive(false);
                        });
                    Rollers[2].Items[j].transform.DOScale(1.2f, 0.5f).SetLoops(2, LoopType.Yoyo).OnComplete(() =>
                        {
                            _spinButton.interactable = true;
                            _winPanels[2-s].SetActive(false);
                        });
                }
            }

            if (isWin)
            {
                PlayerData.WinCount.Value++;
            }
            else
            {
                _spinButton.interactable = true;
                PlayerData.WinCount.Value = 0;
            }
          
        }
    }

}