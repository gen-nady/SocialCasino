using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Json
{
    [Serializable]
    public class UserVm
    {
        public int score;
        public int id;
        public string imageURL;
        public string name;
    }

    [Serializable]
    public class Kekes
    {
        public List<UserVm> users;
    }
}