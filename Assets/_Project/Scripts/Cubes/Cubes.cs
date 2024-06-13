using System.Collections;
using System.Collections.Generic;
using _Project.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Cubes : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private List<Sprite> _cubes;
    [SerializeField] private Button _but;
    [SerializeField] private List<Button> buttons;
    [SerializeField] private List<GameObject> images;
    private decimal _curBet = 1000;

    [SerializeField] private TextMeshProUGUI _tetBet;

    public void Selected(int i)
    {
        foreach (var image in images)
        {
            image.SetActive(false);
        }
        images[i].SetActive(true);
    }
    
    
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

    private bool isWait;
    public void SpinStart()
    {
        if(isWait) return;
        PlayerData.Amount.Value -= _curBet;
        _but.interactable = false;
        foreach (var button in buttons)
        {
            button.interactable = false;
        }
        isWait = true;
        StartCoroutine(Spin());
    }
    
    private IEnumerator Spin()
    {
        var t = 0.05f;
        var count = 0f;
        int rand = 0;
        while (t < 0.7f)
        {
            if (count > 3)
            {
                count = 0;
                t *= 2;
            }
            rand = Random.Range(0, 6);
            _image.sprite = _cubes[rand];
            count++;
            yield return new WaitForSecondsRealtime(t);
        }

        for (int i = 0; i < images.Count; i++)
        {
            if (images[i].activeInHierarchy)
            {
                if (i < 6)
                {
                    if (i == rand)
                    {
                        PlayerData.Amount.Value += _curBet * 6;
                    }
                }
                else
                {
                    if (i == 6)
                    {
                        if(rand % 2 == 1)
                            PlayerData.Amount.Value += _curBet * 2;
                    }
                    else  if (i == 7)
                    {
                        if(rand % 2 == 0)
                            PlayerData.Amount.Value += _curBet * 2;
                    }
                }
                break;
            }
        }
        isWait = false;
        _but.interactable = true;
        foreach (var button in buttons)
        {
            button.interactable = true;
        }
    }

}
