using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts
{
    public class Profile : MonoBehaviour
    {
        [SerializeField] private List<GameObject> _items;

        public void OpenItem(int i)
        {
            _items[i].SetActive(true);
        }

        public void Close()
        {
            foreach (var i in _items)
            {
                i.SetActive(false);
            }
        }
    }
}