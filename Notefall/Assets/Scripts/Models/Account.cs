using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using UnityEngine;

namespace Assets.Scripts.Models
{
    [Serializable]
    public class Account
    {
        public string Name;
        public string PassWord;
        public SongStats[] Stats;

        private static string AccountsPath = Application.dataPath + "/Accounts/";

        public Account() { }

        public static Account FindAccount(string Name, string Password)
        {
            string accPath = AccountsPath + Name + ".json";
            if(File.Exists(accPath))
            {
                Account ret = JsonUtility.FromJson<Account>(File.ReadAllText(accPath));
                if(ret.PassWord == Password)
                {
                    return ret;
                }
                else
                {
                    throw new WrongPasswordException();
                }
            }
            else
            {
                throw new AccountDoesNotExistException(Name);
            }
        }

        public static Account CreateAccount(string Name, string Password)
        {
            string accPath = AccountsPath + Name + ".json";
            if(String.IsNullOrWhiteSpace(Name) || String.IsNullOrWhiteSpace(Password))
            {
                throw new ArgumentException("Name and Password cannot be null");
            }
            if (!File.Exists(accPath))
            {
                Account account = new Account()
                {
                    Name = Name,
                    PassWord = Password
                };
                string newJson = JsonUtility.ToJson(account);
                File.WriteAllText(accPath, newJson);
                return account;
            }
            else
            {
                throw new AccountExistsException(Name);
            }
        }

        public static void UpdateAccount(Account account)
        {
            string accPath = AccountsPath + account.Name + ".json";
            string newJson = JsonUtility.ToJson(account);
            File.WriteAllText(accPath, newJson);
        }
    }

    [Serializable]
    public class SongStats
    {
        public string Name;
        public float Accuracy;
        public int Score;
    }

    public class AccountExistsException : Exception
    {
        public AccountExistsException() { }

        public AccountExistsException(string AccountName) : base("Account, " + AccountName + " already exists!") { }
    }

    public class AccountDoesNotExistException : Exception
    {
        public AccountDoesNotExistException() { }

        public AccountDoesNotExistException(string AccountName) : base("Account, " + AccountName + " does not exist!") { }
    }

    public class WrongPasswordException : Exception
    {
        public WrongPasswordException() : base("Password incorrect!") { }
    }
}
