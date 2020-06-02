using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using UnityEngine;

namespace Assets.Scripts.Models
{
    public class Account
    {
        public string Name { get; private set; }
        public string PassWord { get; private set; }

        private string AccountsPath = Application.dataPath + "/Accounts/Accounts.json";

        public Account(string Name, string Password)
        {

        }

        //private bool FindAccount(string Name)
        //{
        //    using (StreamReader s = new StreamReader(AccountsPath))
        //    {
        //        bool found = false;
        //        bool endOfFile = false;
        //        while (!endOfFile)
        //        {
        //            string ReadLine
        //        }
        //    }
        //}

        public class SongStats
        {

        }
    }
}
