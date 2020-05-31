using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SongListGenerator : MonoBehaviour
{
    public GameObject Content;

    void Start()
    {
        string SongPath = Application.streamingAssetsPath + "\\Songs";
        DirectoryInfo info = new DirectoryInfo(SongPath);
        Debug.Log(info.GetFiles().Length);
    }
}
