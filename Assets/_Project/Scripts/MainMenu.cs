using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using _Project.Scripts;
using _Project.Scripts.Helpers;
using _Project.Scripts.Json;
using TMPro;
using UniRx;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public static string URI = "https://social-slots-backend-55a413a96252.herokuapp.com/";
    public static string Token;
    public static HttpClient Client;
    [SerializeField] private List<User> _users;
    [SerializeField] private TextMeshProUGUI _username1;
    [SerializeField] private TextMeshProUGUI _username2;
    [SerializeField] private TextMeshProUGUI _username3;
    [SerializeField] private TextMeshProUGUI _coin1;
    [SerializeField] private TextMeshProUGUI _coin2;
    [SerializeField] private TextMeshProUGUI _coin3;
    [SerializeField] private TMP_InputField _inputField;
    [SerializeField] private GameObject _loading;
    [SerializeField] private GameObject _socSlot;
    [SerializeField] private GameObject _close;
    [SerializeField] private GameObject _profile;
    [SerializeField] private TextMeshProUGUI _coinText;
    [SerializeField] private List<GameObject> _buttons;
    [SerializeField] private List<GameObject> _panels;
    [SerializeField] private Bonus _bonusPanel;
    [SerializeField] private GameObject _bottomPanels;
    [SerializeField] private List<CanvasGroup> _ach;
    [SerializeField] private List<GameObject> _forPlinko;

    private async void Awake()
    {
        PlayerData.Amount.ObserveEveryValueChanged(_ => _.Value)
            .Subscribe(BalanceChanged)
            .AddTo(this);  
        PlayerData.WinCount.ObserveEveryValueChanged(_ => _.Value)
            .Subscribe(WinCount)
            .AddTo(this);
        if (PlayerPrefs.HasKey("Coin"))
        {
            PlayerData.Amount.Value = ParseConverter.DecimalParse(PlayerPrefs.GetString("Coin"));
        }
        else
        {
            PlayerData.Amount.Value = 5000;
            PlayerPrefs.SetString("Coin", PlayerData.Amount.ToString());
            PlayerPrefs.Save();
        }
        if (!PlayerPrefs.HasKey("bn1"))
        {
            PlayerPrefs.SetInt("bn1", 0);
            PlayerPrefs.Save();
        }
        if (!PlayerPrefs.HasKey("bn2"))
        {
            PlayerPrefs.SetString("bn2", "0");
            PlayerPrefs.Save();
        }
        if (!PlayerPrefs.HasKey("bn3"))
        {
            PlayerPrefs.SetInt("bn3", 0);
            PlayerPrefs.Save();
        }
        if (!PlayerPrefs.HasKey("bn4"))
        {
            PlayerPrefs.SetInt("bn4", 0);
            PlayerPrefs.Save();
        }
        if (!PlayerPrefs.HasKey("bn4perDay"))
        {
            PlayerPrefs.SetInt("bn4perDay", 0);
            PlayerPrefs.Save();
        }

        if (PlayerPrefs.HasKey("LastOpenDay"))
        {
            var date = DateTime.Parse(PlayerPrefs.GetString("LastOpenDay"));
            if (date.Day < DateTime.Now.Day && date.Month <= DateTime.Now.Day && date.Year <= DateTime.Now.Year)
            {
                PlayerPrefs.SetInt("bn4perDay", 0);
                PlayerPrefs.Save();
            }
            PlayerPrefs.SetString("LastOpenDay", DateTime.Now.ToString());
            PlayerPrefs.Save();
        }
        else
        {
            PlayerPrefs.SetString("LastOpenDay", DateTime.Now.ToString());
            PlayerPrefs.Save();
        }

        //await Task.Delay(4000);
     
        Client = new HttpClient();
        var authenticationString = $"admin:ClfQnfaBqnttZxt";
        var base64String = Convert.ToBase64String(
            System.Text.Encoding.ASCII.GetBytes(authenticationString));

        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64String);
        var response = await Client.PostAsync($"{URI}login", null);
        var responseBody = await response.Content.ReadAsStringAsync();
        var json = JsonUtility.FromJson<TokenJson>(responseBody);
        Token = json.token;
        Debug.Log(Token);
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer" ,Token);
        if (PlayerPrefs.HasKey("User"))
        {
            var user = PlayerPrefs.GetInt("User");
            var  response1 = await Client.GetAsync($"{URI}api/players/{user}");
            var responseBody1 = await response1.Content.ReadAsStringAsync();
            var json1 = JsonUtility.FromJson<UserVm>(responseBody1);
            PlayerData.Amount.Value = json1.score;
            PlayerData.Id = json1.id;
            _inputField.text = json1.name ?? "UserName";
        }
        else
        {
            var s = new UserVm
            {
                score = ((int)PlayerData.Amount.Value)
            };
            var  response1 = await Client.PostAsync($"{URI}api/players",  new StringContent(JsonUtility.ToJson(s), Encoding.UTF8, "application/json"));
            var responseBody1 = await response1.Content.ReadAsStringAsync();
            var json1 = JsonUtility.FromJson<UserVm>(responseBody1);
            PlayerPrefs.SetInt("User", json1.id);
            PlayerPrefs.Save();
            PlayerData.Id = json1.id;
            PlayerData.Amount.Value = json1.score;
        }
        _loading.SetActive(false);
    }
    [Serializable]
    public class SS
    {
        public int score;
    }
    
    public void ClisrWarning()
    {
        _socSlot.SetActive(false);
    }
    
    private void WinCount(int count)
    {
        if (count > 9)
        {
            PlayerPrefs.SetInt("bn3", 1);
            PlayerPrefs.Save();
        }
    }
    
    
    private async void BalanceChanged(decimal amount)
    {
        _coinText.text = CountValuesConverter.From1000toK(amount);
        S(amount);
    }

    private async void S(decimal amount)
    {
        var s = new UserVm
        {
            name = _inputField.text,
            score = (int)amount
        };
        if(PlayerData.Id == 0)  return;
        var  response1 = await Client.PutAsync($"{URI}api/players/{PlayerData.Id}",  new StringContent(JsonUtility.ToJson(s), Encoding.UTF8, "application/json"));
    }

    public async void ChangeName()
    {
        var s = new UserVm
        {
            name = _inputField.text,
            score = (int)PlayerData.Amount.Value
        };
        if(PlayerData.Id == 0)  return;
        var  response1 = await Client.PutAsync($"{URI}api/players/{PlayerData.Id}",  new StringContent(JsonUtility.ToJson(s), Encoding.UTF8, "application/json"));
    }
    
    public async void Selected(int i)
    {
        if (i == 9)
        {
            _panels[i].SetActive(true);
            _close.SetActive(true);
            _profile.SetActive(false);
            _bottomPanels.SetActive(false);
            foreach (var plin in _forPlinko)
            {
                plin.SetActive(false);
            }
            return;
        }
        else
        {
            foreach (var plin in _forPlinko)
            {
                plin.SetActive(true);
            }
        }
        if (i == 8)
        {
            _panels[i].SetActive(true);
            _close.SetActive(true);
            _profile.SetActive(false);
            _bottomPanels.SetActive(false);
            return;
        }
        if (i == 7)
        {
            _panels[i].SetActive(true);
            _close.SetActive(true);
            _profile.SetActive(false);
            _bottomPanels.SetActive(false);
            return;
        }
        if (i == 6)
        {
            _panels[i].SetActive(true);
            _close.SetActive(true);
            _profile.SetActive(false);
            _bottomPanels.SetActive(false);
            return;
        }
        if (i == 5)
        {
            _panels[i].SetActive(true);
            _close.SetActive(true);
            _profile.SetActive(false);
            _bottomPanels.SetActive(false);
            return;
        }
        _bottomPanels.SetActive(true);
        if (i == 4)
        {
            _panels[i].SetActive(true);
            return;
        }
        if (i == 3)
        {
            _panels[4].SetActive(false);
            _panels[i].SetActive(true);
            var bn1 = PlayerPrefs.GetInt("bn1");
            var bn2 = PlayerPrefs.GetString("bn2");
            var bn3 = PlayerPrefs.GetInt("bn3");
            var bn4 = PlayerPrefs.GetInt("bn4");
            if (bn1 > 1000)
                _ach[0].alpha = 1;
            if (ParseConverter.DecimalParse(bn2) >= 1000000)
                _ach[1].alpha = 1;
            if (bn3 == 1)
                _ach[2].alpha = 1;
            if (bn4 == 1)
                _ach[3].alpha = 1;
            
                
            return;
        }
        foreach (var b in _buttons)
            b.SetActive(false); 
        foreach (var p in _panels)
            p.SetActive(false);
        
        _buttons[i].SetActive(true);
        _panels[i].SetActive(true);

        _close.SetActive(i is 0 or 2);
        _profile.SetActive(i == 1);
        if(i == 2)
            _bonusPanel.OpenBonus();
        if (i == 0)
        {
            _panels[0].SetActive(false);
            var  response1 = await Client.GetAsync($"{URI}api/players");
            var responseBody1 = await response1.Content.ReadAsStringAsync();
            responseBody1 = "{\"users\":" + responseBody1 + "}";
            var json1 = JsonUtility.FromJson<Kekes>(responseBody1);
            var users = json1.users.OrderByDescending(_ => _.score).ToList();
            for (int j = 0; j < _users.Count; j++)
            {
                if (j >= users.Count)
                {
                    _users[j]._userName.text = string.Empty;
                    _users[j]._coin.text = string.Empty;
                    _users[j]._coinG.SetActive(false);
                }
                else
                {
                    var s = users[j].name == string.Empty ? "UserName" : users[j].name ;
                    _users[j]._userName.text = $"{j + 1}. {s}";
                    _users[j]._coin.text = users[j].score.ToString();
                    _users[j]._coinG.SetActive(true);
                }
              
            }

            if (users.Count > 0)
            {
               _username1.text = users[0].name == string.Empty ? "UserName" : users[0].name ;
               _coin1.text = users[0].score.ToString();
            }
            if (users.Count > 1)
            {
                _username2.text = users[1].name  == string.Empty ? "UserName" : users[1].name ;
                _coin2.text = users[1].score.ToString();
            }  
            if (users.Count > 2)
            {
                _username3.text = users[2].name  == string.Empty ? "UserName" : users[2].name ;
                _coin3.text = users[2].score.ToString();
            }
            _panels[0].SetActive(true);
            //json
        }
    }
}
