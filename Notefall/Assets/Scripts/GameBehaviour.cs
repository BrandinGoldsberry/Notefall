using Assets.Scripts.Models;
using Assets.Scripts.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameBehaviour : MonoBehaviour
{
    public Song Song;
    public GameObject note;
    public AudioSource NotePlayer;
    public AudioClip[] NoteSounds;
    public AudioSource SongPlayer;

    public GameObject EndPanel;
    public Text FinalScore;
    public Text FinalAccuracy;
    public Text FinalCombo;
    public Text Rating;
    public Button BackToSongList;

    public string SongName;

    private GameObject[] SpawnLocations;
    private GameObject hitBar;

    private UnityEvent clickEvent = new UnityEvent();
    private UnityEvent spawnEvent = new UnityEvent();

    private float CurrentTime;

    //stores the next note to spawn
    private NoteTime nextNote;

    private List<GameObject> spawnedNotes;

    private int curSnare;

    private int score = 0;
    private float multiplier = 1;
    private int highestCombo = 0;
    private int comboCount = 0;
    private int totalHit;
    private Text scoreDisplay;
    private Text multiplierDisplay;

    void Start()
    {
        BackToSongList.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("SongSelect");
        });

        Song = SongLoader.LoadSong(SongName);
        Song.FixNoteNum();
        scoreDisplay = GameObject.Find("ScoreDisplay").GetComponent<Text>();
        multiplierDisplay = GameObject.Find("MulitplierDisplay").GetComponent<Text>();


        //keep track of existing notes
        spawnedNotes = new List<GameObject>();

        //We will most likely be creating the spawn nodes in code rather than fetching them
        //Even more likely we will store raw positions in Vector3 rather than even needing GameObjects
        GameObject holdMyNotes = GameObject.Find("HoldMyNotes");
        SpawnLocations = new GameObject[]
        {
            Instantiate(new GameObject(), Camera.main.ViewportToWorldPoint(new Vector3(0.2f, 1f, 0)), holdMyNotes.transform.rotation, holdMyNotes.transform),
            Instantiate(new GameObject(), Camera.main.ViewportToWorldPoint(new Vector3(0.4f, 1f, 0)), holdMyNotes.transform.rotation, holdMyNotes.transform),
            Instantiate(new GameObject(), Camera.main.ViewportToWorldPoint(new Vector3(0.6f, 1f, 0)), holdMyNotes.transform.rotation, holdMyNotes.transform),
            Instantiate(new GameObject(), Camera.main.ViewportToWorldPoint(new Vector3(0.8f, 1f, 0)), holdMyNotes.transform.rotation, holdMyNotes.transform),
        };

        //initialize spawn
        nextNote = Song.Spawn();
        //add method to event
        //spawnEvent.AddListener(Camera.main.gameObject.GetComponent<DebugText>().UpdateDebugText);
        spawnEvent.AddListener(SpawnNote);

        //hitbar set
        hitBar = GameObject.Find("HitBar");
        if (SongPlayer == null)
        {

        }
        SongPlayer.Play();
    }

    //debugvar
    //private int clicks;
    void Update()
    {
        //If we get this number stop the song
        //Its A) The return of Song.Spawn() if there isn't anymore
        //B) If we get this at any time the thing will break anyway
        if (nextNote.Time <= -1 && spawnedNotes.Count < 1)
        {
            //TODO: Write code that jumps to end scene of the song
            //We can do this by creating a Unity Scene with a UI that has variables for setting up the values
            //Scratch that I have a better way
            EndPanel.SetActive(true);
            float finalAccuracy = (float)totalHit / Song.NoteNum;
            FinalAccuracy.text = string.Format("Accuracy: {0:P2}.", finalAccuracy);
            if(finalAccuracy > 0.99f)
            {
                //ss
                Rating.text = "SS";
                Rating.color = new Color(0.956f, 0.945f, 0.043f);
            }
            else if (finalAccuracy > 0.95f)
            {
                //s
                Rating.text = "S";
                Rating.color = new Color(0.878f, 0.878f, 0.847f);
            }
            else if (finalAccuracy > 0.90f)
            {
                //a
                Rating.text = "A";
                Rating.color = new Color(0.203f, 0.952f, 0.043f);
            }
            else if (finalAccuracy > 0.80f)
            {
                //b
                Rating.text = "B";
                Rating.color = new Color(0.992f, 0.976f, 0.317f);
            }
            else if (finalAccuracy > 0.70f)
            {
                //c
                Rating.text = "C";
                Rating.color = new Color(0.988f, 0.721f, 0.321f);
            }
            else if (finalAccuracy > 0.60f) {
                //d
                Rating.text = "D";
                Rating.color = new Color(1f, 0.309f, 0.309f);
            }
            else if (finalAccuracy > 0.50f)
            {
                //e
                Rating.text = "E";
                Rating.color = new Color(0.874f, 0.435f, 0.435f);
            }
            else 
            {
                //f
                Rating.text = "F";
                Rating.color = new Color(0.729f, 0.580f, 0.580f);
            }
            FinalCombo.text = "Hi-Combo: " + highestCombo.ToString();
            FinalScore.text = "Score: " + score.ToString();
            scoreDisplay.gameObject.SetActive(false);
            multiplierDisplay.gameObject.SetActive(false);
            if(PersistentVariables.ActiveAccount != null)
            {
                Account curAcc = PersistentVariables.ActiveAccount;
                foreach(SongStats stats in curAcc.Stats)
                {
                    if(stats.Name == Song.Name)
                    {
                        stats.Played = true;
                        if(highestCombo > stats.LongestCombo)
                        {
                            stats.LongestCombo = highestCombo;
                        }
                        if (score > stats.Score)
                        {
                            stats.Score = score;
                        }
                        if (finalAccuracy > stats.Accuracy)
                        {
                            stats.Accuracy = finalAccuracy;
                        }
                        Account.UpdateAccount(curAcc);
                    }
                }
            }
        }
        else
        {
            //Redundancy for cleaner code
            CurrentTime = Time.timeSinceLevelLoad;
            //Debug.Log("CT:" + CurrentTime + " NN: " + nextNote.Key + "TF: " + (CurrentTime > nextNote.Key));

            //On next frame where time is greater than the target time spawn the next note
            if (nextNote.Time > -1 && CurrentTime > nextNote.Time)
            {

                spawnEvent.Invoke();
            }
            //get if left mouse was click (should work on mobile too)
            if (Input.GetMouseButtonDown(0))
            {
                //Get the position of the mouse in the world from where it is on screen
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                //Check if player clicked within margin of error of the hitbar
                if (hitBar.transform.position.y < mousePos.y + Song.MinAccuracy && hitBar.transform.position.y > mousePos.y - Song.MinAccuracy)
                {
                    //Grab ONLY the x and y position, its a 2d game
                    Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
                    //send an invisible line out to hit a game object
                    RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
                    //Debug.Log("Click! " + clicks++);
                    //if it hit note then do something
                    if (hit.collider != null && hit.collider.name.Contains("Note"))
                    {
                        NotePlayer.PlayOneShot(NoteSounds[curSnare]);
                        curSnare++;
                        if (curSnare == NoteSounds.Length) curSnare = 0;
                        //Debug.Log(hit.transform.gameObject);
                        //this is stupid
                        hit.transform.gameObject.GetComponent<NoteBehaviour>().OnDestroyObject(new NoteEventArgs(hit.transform.gameObject, true));
                    }
                }
            }
        }
    }

    void SpawnNote()
    {
        //create a note and then setup next spawn
        //Debug.Log(nextNote.Value);
        Vector3 spawnNotePos = SpawnLocations[nextNote.SpawnLocation].transform.position;
        Quaternion spawnNoteRot = SpawnLocations[nextNote.SpawnLocation].transform.rotation;
        GameObject spawned = Instantiate(note, spawnNotePos, spawnNoteRot, SpawnLocations[nextNote.SpawnLocation].transform);
        spawned.name = "Note " + Song.spawned;
        spawned.transform.position = spawnNotePos;
        //add spawned to the list
        spawnedNotes.Add(spawned);
        //Set the note's speed to the speed of the song
        spawned.GetComponent<NoteBehaviour>().Speed = Song.SongSpeed;
        spawned.GetComponent<NoteBehaviour>().NoteDestoyed += OnDestroyedNote;
        spawned.GetComponent<NoteBehaviour>().NoteMissed += OnMissedNote;
        //Debug.Log(spawned);
        //setup next spawn
        nextNote = Song.Spawn();
    }

    void OnMissedNote(object sender, NoteEventArgs e)
    {
        multiplier = 1f;
        scoreDisplay.text = score.ToString();
        comboCount = 0;
        multiplierDisplay.text = comboCount.ToString() + "x";
    }

    void OnDestroyedNote(object sender, NoteEventArgs e)
    {
        Debug.Log(e.WasHit);
        if (e.WasHit)
        {
            comboCount++;
            highestCombo = comboCount > highestCombo ? comboCount : highestCombo;
            multiplier += 0.1f;
            totalHit++;
            score += Mathf.FloorToInt(multiplier * 100);
            scoreDisplay.text = score.ToString();
            multiplierDisplay.text = comboCount.ToString() + "x";
        }
        //else
        //{
        //    multiplier = 1f;
        //    scoreDisplay.text = score.ToString();
        //    multiplierDisplay.text = multiplier.ToString();
        //}
        spawnedNotes.Remove(e.Sender);
        Destroy(e.Sender);
    }
}
