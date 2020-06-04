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
    public Button BackToMenu;

    private string activeSong = "Payphone";

    void Start()
    {
        BackToMenu.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("MainMenu");
        });
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
        //Set to always true for debug purposes
        if (/*PersistentVariables.ActiveAccount != null && PersistentVariables.ActiveAccount.Stats != null*/ true)
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

    int speedCount = 0;
    void Update()
    {
        Record.transform.Rotate(new Vector3(0, 0, 1), Space.World);
        if(speedCount != 0)
        {
            Record.transform.Rotate(new Vector3(0, 0, speedCount/5), Space.World);
            speedCount -= 2;
        }
    }

    void AddToList(Song song, int listNum)
    {
        Vector3 newSpawn = Content.transform.position;
        newSpawn.y -= ((Screen.height / 6 * (listNum)) + 200);

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
            speedCount = 100;
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
                    if(stat.Name == activeSong && stat.Played)
                    {
                        DisplayAccuracy.text = "Accuracy: " + string.Format("{0:P2}.", stat.Accuracy);
                        DisplayScore.text = "Score: " + stat.Score;
                        songFound = true;
                        break;
                    }
                }
                if(!songFound)
                {
                    DisplayAccuracy.text = "Accuracy: N/A";
                    DisplayScore.text = "Score: N/A";
                }
            }
        });
        Button activateSong = newListing.GetComponent<Button>();
        activateSong.onClick.AddListener(songSet);

        Button[] buttons = newListing.GetComponentsInChildren<Button>();
        buttons[1].onClick.AddListener(() =>
        {
            SceneManager.LoadScene(song.Name);
        });
    }
}
