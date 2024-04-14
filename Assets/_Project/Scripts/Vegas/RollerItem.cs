using _Project.Scripts.Vegas.Enum;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.Vegas
{
    public class RollerItem : MonoBehaviour
    {
        public RollerItemType Type;
        public Roller _roller;
        public Image Image;
        private float _moveSpeed = 5000;
        public float _bottomLimit;
        
        public void Spin()
        {
            transform.localPosition -= _moveSpeed * Time.deltaTime * Vector3.up;
            if (transform.localPosition.y < _bottomLimit)
            {
                transform.localPosition = _roller.GetLastItemLocalPosition() + _roller.GetSpacingBetweenItems();
                _roller.MoveFirstItemToTheBack();
            }
        }
    }

}