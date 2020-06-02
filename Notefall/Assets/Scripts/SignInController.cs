using Assets.Scripts.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SignInController : MonoBehaviour
{
    public Button SignInButton;
    public Text UserName;
    public Text Password;
    // Start is called before the first frame update
    void Start()
    {
        SignInButton.onClick.AddListener(() =>
        {
            Account toLog = Account.CreateAccount(UserName.text, Password.text);
            PersistentVariables.ActiveAccount = toLog;
            SceneManager.LoadScene("MainMenu");
        });
    }
}
