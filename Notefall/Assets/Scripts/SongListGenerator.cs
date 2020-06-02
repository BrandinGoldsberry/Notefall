using Assets.Scripts.Models;
using Assets.Scripts.Util;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SongListGenerator : MonoBehaviour
{
    public Image MainImage;
    public Text SongName;
    public Text DisplayScore;
    public Text DisplayAccuracy;
    public GameObject Content;
    public GameObject SongPrefab;
    public Sprite[] SongImages;
    public AudioSource[] Songs;
    public GameObject AudioSourceSpawner;
    public GameObject Record;

    private string activeSong = "Payphone";

    void Start()
    {
        string SongPath = Application.dataPath + "/Songs";
        DirectoryInfo info = new DirectoryInfo(SongPath);
        int listNum = 0;
        foreach(FileInfo file in info.GetFiles())
        {
            if(file.Extension == ".json")
            {
                AddToList(SongLoader.LoadSong(file.FullName), listNum);
                listNum++;
            }
        }
        if (PersistentVariables.ActiveAccount != null && PersistentVariables.ActiveAccount.Stats != null)
        {
            foreach (SongStats stat in PersistentVariables.ActiveAccount.Stats)
            {
                if (stat.Name == activeSong)
                {
                    DisplayAccuracy.text = "Accuracy: " + stat.Accuracy;
                    DisplayScore.text = "Score: " + stat.Score;
                    break;
                }
            }
        }
        else
        {
            SceneManager.LoadScene("MainMenu");
        }
        MainImage.preserveAspect = true;
    }



    void AddToList(Song song, int listNum)
    {
        Vector3 newSpawn = Content.transform.position;
        newSpawn.y -= ((100 * listNum) + 50);

        GameObject newListing = Instantiate(SongPrefab, newSpawn, Content.transform.rotation, Content.transform);
        newListing.name = song.Name;

        Text listingName = newListing.transform.GetChild(4).gameObject.GetComponent<Text>();
        listingName.text = song.Name;

        Image listingImage = newListing.transform.GetChild(2).gameObject.GetComponent<Image>();
        foreach(Sprite sprite in SongImages)
        {
            if(sprite.name == song.Name)
            {
                listingImage.sprite = sprite;
                break;
            }
        }

        UnityAction songSet = new UnityAction(() =>
        {
            activeSong = song.Name;
            SongName.text = activeSong;
            if (AudioSourceSpawner.transform.childCount > 0)
            {
                Destroy(AudioSourceSpawner.transform.GetChild(0).gameObject);
            }
            foreach (AudioSource item in Songs)
            {
                if(item.clip.name == song.Name)
                {
                    AudioSource spawned = Instantiate(item, AudioSourceSpawner.transform);
                    spawned.Play();
                    break;
                }
            }
            foreach (Sprite sprite in SongImages)
            {
                if (sprite.name == song.Name)
                {
                    MainImage.sprite = sprite;
                    break;
                }
            }
            if(PersistentVariables.ActiveAccount.Stats != null)
            {
                bool songFound = false;
                foreach (SongStats stat in PersistentVariables.ActiveAccount.Stats)
                {
                    if(stat.Name == activeSong)
                    {
                        DisplayAccuracy.text = "Accuracy: " + stat.Accuracy;
                        DisplayScore.text = "Score: " + stat.Score;
                        songFound = true;
                        break;
                    }
                }
                if(!songFound)
                {
                    DisplayAccuracy.text = "Accuracy: Not Played";
                    DisplayScore.text = "Score: Not Played";
                }
            }
        });
        Button activateSong = newListing.GetComponent<Button>();
        activateSong.onClick.AddListener(songSet);
    }
}
