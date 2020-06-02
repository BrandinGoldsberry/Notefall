﻿using Assets.Scripts.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LogInController : MonoBehaviour
{
    public Button LoginButton;
    public Text UserName;
    public Text Password;
    // Start is called before the first frame update
    void Start()
    {
        LoginButton.onClick.AddListener(() =>
        {
            Account toLog = Account.FindAccount(UserName.text, Password.text);
            PersistentVariables.ActiveAccount = toLog;
            SceneManager.LoadScene("MainMenu");
        });
    }
}
