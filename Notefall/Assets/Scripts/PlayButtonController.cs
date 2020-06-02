using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButtonController : MonoBehaviour
{
    void OnMouseDown()
    {
        if(PersistentVariables.ActiveAccount != null)
        {
            SceneManager.LoadScene("SongSelect");
        }
    }
}
