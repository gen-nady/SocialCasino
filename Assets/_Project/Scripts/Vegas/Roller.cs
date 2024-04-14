using System.Collections;
using System.Collections.Generic;
using _Project.Scripts.Vegas.Enum;
using UnityEngine;

namespace _Project.Scripts.Vegas
{
   public class Roller : MonoBehaviour
    {
        public bool IsSpinning { get; private set; }

        public List<RollerItem> Items;
        
        [SerializeField] private List<Sprite> _spriteAssets;
        private float currentSpinTmeInSeconds = 0.2f;
        private const float DefaultSpinSpeed = 0.2f;
        private const float _startingItemYPosition = -223.1954f;
        private const float _spacingBetweenItems = 220.595327f;
        
        private void Update()
        {
            if (IsSpinning)
            {
                SpinItems();
            }
        }

        public void StartSpin()
        {
            currentSpinTmeInSeconds = DefaultSpinSpeed;
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
            for (int j = 0; j < 3; j++)
            {
                var rnd = Random.Range(0, 3);
                var itemSprite = _spriteAssets[rnd];
                Items[j].Image.sprite = itemSprite;
                Items[j].Type = (RollerItemType) rnd;
            }
            var curPos = _startingItemYPosition;
            for (int i = 0; i < Items.Count; ++i)
            {
                Items[i].transform.localPosition = new Vector3(0, curPos, 0);
                curPos += _spacingBetweenItems;
            }
        }
    }

}