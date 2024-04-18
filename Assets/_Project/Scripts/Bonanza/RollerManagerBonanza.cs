using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Bonanza.Enum;
using _Project.Scripts.Helpers;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.Bonanza
{
    public class RollerManagerBonanza : MonoBehaviour
    {
        public RollerBonanza[] Rollers;
        [SerializeField] private Button _spinButton;
        [SerializeField] private TextMeshProUGUI _buyCountText;
        [SerializeField] private TextMeshProUGUI _spinCountText;
        private const float _delayBetweenRollersInSeconds = 0.1f;
        private decimal _curBet = 1000;
        private int _curReels = 0;
        private bool _isSpinnig;
        private List<decimal> _amount = new List<decimal>
        {
            1000, 2000, 4000, 6000, 8000, 10000
        };
        private List<decimal> _amountActiveReels = new List<decimal>
        {
            1000, 1000, 1500, 2000, 2500, 3000
        };

        private void OnEnable()
        {
            if (PlayerPrefs.HasKey("Reels"))
            {
                _curReels = PlayerPrefs.GetInt("Reels");
                if (_curReels == 5)
                {
                    _buyCountText.text = "";
                }
                else
                {
                    _buyCountText.text = _amountActiveReels[_curReels].ToString();
                }
            }
            else
            {
                PlayerPrefs.SetInt("Reels", 0);
                PlayerPrefs.Save();
                _curReels = 0;
                _buyCountText.text = _amountActiveReels[_curReels].ToString();
            }

            for (int i = 0; i <= _curReels; i++)
            {
                Rollers[i].ISActive = true;
                Rollers[i].CanvasGroup.alpha = 1f;
            }

            _spinCountText.text = (_curBet * (_curReels + 1)).ToString();
        }

        public void BuyNew()
        {
            if(_isSpinnig) return;
            if(_curReels == 5) return;
            var curReels = PlayerPrefs.GetInt("Reels");
            if (curReels > _curReels)
            {
                _curReels++;
                Rollers[_curReels].ISActive = true;
                Rollers[_curReels].CanvasGroup.alpha = 1f;
                _buyCountText.text = curReels > _curReels ? 0.ToString() : _amountActiveReels[_curReels].ToString();
                _spinCountText.text = (_curBet * (_curReels + 1)).ToString();
                return;
            }
            
            if(PlayerData.Amount.Value < _amountActiveReels[_curReels]) return;
            
            PlayerData.Amount.Value -= _amountActiveReels[_curReels];
            PlayerPrefs.SetString("Coin", PlayerData.Amount.ToString());
            PlayerPrefs.Save();
            _curReels++;
            PlayerPrefs.SetInt("Reels", _curReels);
            PlayerPrefs.Save();
            Rollers[_curReels].ISActive = true;
            Rollers[_curReels].CanvasGroup.alpha = 1f;
            _buyCountText.text = _curReels != 5 ? _amountActiveReels[_curReels].ToString() : "";
           
            _spinCountText.text = (_curBet * (_curReels + 1)).ToString();
        }
        public void Minus()
        {
            if(_isSpinnig) return;
            if(_curReels == 0) return;
            Rollers[_curReels].ISActive = false;
            Rollers[_curReels].CanvasGroup.alpha = 0.5f;
            _buyCountText.text = "0";
            _curReels--;
            _spinCountText.text = (_curBet * (_curReels + 1)).ToString();
        }
        
        public void StartSpin()
        {
            if (_curBet * (_curReels + 1)> PlayerData.Amount.Value)
            {
                return;
            }

            PlayerData.Amount.Value -= _curBet * (_curReels + 1);
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
            _isSpinnig = true;
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
            for (int i = 0; i < Rollers.Length; i++)
            {
                if(!Rollers[i].ISActive) continue;
                if (Rollers[i].Items.Any(_ =>
                        _.transform.localPosition.y is -170f or 0f or 170f && _.Type == RollerItemTypeBonanza.K))
                {
                    PlayerData.Amount.Value += _amount[i];
                    var count = ParseConverter.DecimalParse(PlayerPrefs.GetString("bn2"));
                    count += _amount[i];
                    PlayerPrefs.SetString("bn2", count.ToString());
                    PlayerPrefs.Save();
                    isWin = true;
                }
            }
            if (isWin)
            {
                PlayerData.WinCount.Value++;
            }
            else
            {
                PlayerData.WinCount.Value = 0;
            }
            PlayerPrefs.SetString("Coin", PlayerData.Amount.ToString());
            PlayerPrefs.Save();
            _spinButton.interactable = true;
            _isSpinnig = false;
        }
    }

}