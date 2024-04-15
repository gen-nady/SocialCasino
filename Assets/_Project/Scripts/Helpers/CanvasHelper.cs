using System;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.Helpers
{
    public class CanvasHelper : MonoBehaviour
    {
        private CanvasScaler _canvasHelper;
        
        private void OnEnable()
        {
            _canvasHelper = GetComponent<CanvasScaler>();
            ChangeMatch();
        }
        
  

        private void ChangeMatch()
        {
            _canvasHelper.matchWidthOrHeight = Math.Abs((float) Screen.height / Screen.width - 2.4f);
        }
    }
}