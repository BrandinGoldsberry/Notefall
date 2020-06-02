using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private Button LogIn;
    [SerializeField] private Button SignUp;

    // Start is called before the first frame update
    void Start()
    {
        LogIn.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("LogIn");
        });
        SignUp.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("SignUp");
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
