using _Project.Scripts.Bonanza.Enum;
using _Project.Scripts.Vegas.Enum;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.Bonanza
{
    public class RollerItemBonanza : MonoBehaviour
    {
        public RollerItemTypeBonanza Type;
        public RollerBonanza _roller;
        public Image Image;
        private float _moveSpeed = 5000;
        private float _bottomLimit = -170f;
        
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