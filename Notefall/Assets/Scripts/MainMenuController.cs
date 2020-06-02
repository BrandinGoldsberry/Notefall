using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public Button LogIn;
    public Button SignUp;
    public Text UserNameDisplay;
    public Button LogOut;

    // Start is called before the first frame update
    void Start()
    {
        if(PersistentVariables.ActiveAccount != null)
        {
            LogIn.gameObject.SetActive(false);
            SignUp.gameObject.SetActive(false);
            LogOut.onClick.AddListener(() =>
            {
                PersistentVariables.ActiveAccount = null;
                SceneManager.LoadScene("MainMenu");
            });
            UserNameDisplay.text = PersistentVariables.ActiveAccount.Name;
        }
        else
        {
            LogOut.gameObject.SetActive(false);
            LogIn.onClick.AddListener(() =>
            {
                SceneManager.LoadScene("LogIn");
            });
            SignUp.onClick.AddListener(() =>
            {
                SceneManager.LoadScene("SignUp");
            });
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
