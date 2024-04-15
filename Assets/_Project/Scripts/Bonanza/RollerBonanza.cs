using System.Collections;
using System.Collections.Generic;
using _Project.Scripts.Bonanza.Enum;
using UnityEngine;

namespace _Project.Scripts.Bonanza
{
   public class RollerBonanza : MonoBehaviour
    {
        public bool IsSpinning { get; private set; }
        public bool ISActive;
        public List<RollerItemBonanza> Items;
        public CanvasGroup CanvasGroup;
        
        [SerializeField] private List<Sprite> _spriteAssets;
        [SerializeField] private List<Sprite> _spriteAssetsK;
        private float currentSpinTmeInSeconds = 0.2f;
        private const float _startingItemYPosition = -170f;
        private const float _spacingBetweenItems = 170f;
        
        private void Update()
        {
            if (IsSpinning)
            {
                SpinItems();
            }
        }

        public void StartSpin()
        {
            currentSpinTmeInSeconds = Random.Range(2,5) / 10f;
            IsSpinning = true;
        }

        public void StartSpinCountdown()
        {
            StartCoroutine(StopSpinAfterDelay(currentSpinTmeInSeconds));
        }

        public void MoveFirstItemToTheBack()
        {
            var firstItem = Items[0];
            Items.Add(firstItem);
            Items.RemoveAt(0);
        }

        public Vector3 GetSpacingBetweenItems()
        {
            return Vector3.up * _spacingBetweenItems;
        }

        public Vector3 GetLastItemLocalPosition()
        {
            return Items[Items.Count - 1].transform.localPosition;
        }

        private void SpinItems()
        {
            for (int i = 0; i < Items.Count; ++i)
            {
                Items[i].Spin();
            }
        }

        private IEnumerator StopSpinAfterDelay(float delayInSeconds)
        {
            yield return new WaitForSeconds(delayInSeconds);
          
            IsSpinning = false;
            //_audioService.Play("Stop Roller");
            CenterItemsOnScreenIfNecessary();
        }

        private void CenterItemsOnScreenIfNecessary()
        {
            var curPos = _startingItemYPosition;
            for (int i = 0; i < Items.Count; ++i)
            {
                Items[i].transform.localPosition = new Vector3(0, curPos, 0);
                curPos += _spacingBetweenItems;
            }
        }
    }

}