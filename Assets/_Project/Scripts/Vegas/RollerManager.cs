using System.Collections;
using System.Text;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.Vegas
{
    public class RollerManager : MonoBehaviour
    {
        public Roller[] Rollers;
        [SerializeField] private Button _spinButton;
        [SerializeField] private TextMeshProUGUI _tetBet;
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
                //ничего
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
            for (int j = 0; j < 3; j++)
            {
                if (Rollers[0].Items[j].Type == Rollers[1].Items[j].Type &&
                    Rollers[1].Items[j].Type == Rollers[2].Items[j].Type)
                {
                    PlayerData.Amount.Value += _curBet * 3;
                }
            }
            _spinButton.interactable = true;
        }
    }

}