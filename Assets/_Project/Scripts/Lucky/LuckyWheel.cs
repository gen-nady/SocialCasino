using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace _Project.Scripts.Lucky
{
    public class LuckyWheel : MonoBehaviour
    {
        [SerializeField] private AnimationCurve _rotateCurve;
        [SerializeField] private Transform _luckyWheel;
        [SerializeField] private Button _stopSpinButton;
        
        private float _spinTime;
        private const float CIRCLE_ANGLE = 360;
        private float _angleOfCell;
        private Coroutine _spinCoroutine;
        private bool _isSpinStopped;
        private const float FIRST_SPIN_TIME = 3f;
        private const float UNEXPECTED_STOP_TIME = 1f;
        private int _sectorID;
        private Vector2 _defaultWinSize;
        private decimal _curBet = 1000;
        private List<decimal> kef = new List<decimal> {10, 20, 50, 100, 0, 0.5m, 1, 2, 5};
        public void Spin()
        {
            _stopSpinButton.interactable = false;
            PlayerData.Amount.Value -= _curBet;
            StartCoroutine(RotateWheel());
        }

        private IEnumerator RotateWheel()
        {
            var startAngle = _luckyWheel.eulerAngles.z;
            var curTime = 0f;
            var accelerationFactor = 1f;
            var curSpinTime = FIRST_SPIN_TIME;
            var s = Random.Range(0, 9);
            Debug.Log(s);
            var currentAngleCell = 40 * s;
            if (currentAngleCell != 0)
                currentAngleCell = 360 - currentAngleCell;
            var wantedAngle = 3 * CIRCLE_ANGLE + currentAngleCell - _angleOfCell / 2 + startAngle;
            
            while (curTime < curSpinTime)
            {
                yield return new WaitForEndOfFrame();
                curTime += Time.deltaTime * accelerationFactor;
                if (_isSpinStopped)
                {
                    if (Math.Abs(accelerationFactor - 1f) == 0 && curTime + UNEXPECTED_STOP_TIME < curSpinTime)
                    {
                        accelerationFactor = curSpinTime - curTime;
                    }
                }
                var curAngle = (wantedAngle * -_rotateCurve.Evaluate(curTime / curSpinTime));
                _luckyWheel.eulerAngles = new Vector3(0, 0, curAngle + startAngle);
            }

            PlayerData.Amount.Value += _curBet * kef[s];
            
            _stopSpinButton.interactable = true;
        }


        [SerializeField] private TextMeshProUGUI _tetBet;
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
        
        public void StopCoroutineWheel()
        {
            if(_spinCoroutine != null)
                StopCoroutine(_spinCoroutine);
        }
    }
}
