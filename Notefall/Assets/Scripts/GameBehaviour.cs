using Assets.Scripts.Models;
using Assets.Scripts.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameBehaviour : MonoBehaviour
{
    public Song Song;
    public GameObject note;
    public AudioSource NotePlayer;
    public AudioClip[] NoteSounds;

    public string JSONSong = "F:\\Songs\\SongTest.json";
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
    private Text scoreDisplay;
    private Text multiplierDisplay;

    void Start()
    {
        string JSONSong = Application.dataPath + "\\Songs\\" + SongName + ".json";
        Song = SongLoader.LoadSong(JSONSong);
        scoreDisplay = GameObject.Find("ScoreDisplay").GetComponent<Text>();
        multiplierDisplay = GameObject.Find("MulitplierDisplay").GetComponent<Text>();


        //keep track of existing notes
        spawnedNotes = new List<GameObject>();

        //We will most likely be creating the spawn nodes in code rather than fetching them
        //Even more likely we will store raw positions in Vector3 rather than even needing GameObjects
        Transform spawnLocs = GameObject.Find("NoteSpawnLocs").transform;
        SpawnLocations = new GameObject[]
        {
            spawnLocs.GetChild(0).gameObject,
            spawnLocs.GetChild(1).gameObject,
            spawnLocs.GetChild(2).gameObject
        };

        //initialize spawn
        Debug.Log(Song.SpawnTime_SpawnLoc[0]);
        nextNote = Song.Spawn();
        //add method to event
        //spawnEvent.AddListener(Camera.main.gameObject.GetComponent<DebugText>().UpdateDebugText);
        spawnEvent.AddListener(SpawnNote);

        //hitbar set
        hitBar = GameObject.Find("HitBar");
    }

    //debugvar
    //private int clicks;
    void Update()
    {
        //If we get this number stop the song
        //Its A) The return of Song.Spawn() if there isn't anymore
        //B) If we get this at any time the thing will break anyway
        if(nextNote.Time < -1 && spawnedNotes.Count < 1)
        {
            //TODO: Write code that jumps to end scene of the song
            //We can do this by creating a Unity Scene with a UI that has variables for setting up the values
        } 
        else
        {
            //Redundancy for cleaner code
            CurrentTime = Time.time;
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
                if(hitBar.transform.position.y < mousePos.y + Song.MinAccuracy && hitBar.transform.position.y > mousePos.y - Song.MinAccuracy)
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
        GameObject spawned = Instantiate(note, SpawnLocations[nextNote.SpawnLocation].transform);
        spawned.name = "Note " + Song.spawned;
        //add spawned to the list
        spawnedNotes.Add(spawned);
        //Set the note's speed to the speed of the song
        spawned.GetComponent<NoteBehaviour>().Speed = Song.SongSpeed;
        spawned.GetComponent<NoteBehaviour>().NoteDestoyed += OnDestroyedNote;
        //Debug.Log(spawned);
        //setup next spawn
        nextNote = Song.Spawn();
    }

    void OnDestroyedNote(object sender, NoteEventArgs e)
    {
        if(e.WasHit)
        {
            multiplier += 0.1f;
            score += Mathf.FloorToInt(multiplier * 100);
            scoreDisplay.text = score.ToString();
            multiplierDisplay.text = multiplier.ToString();
        }
        else
        {
            multiplier = 1f;
            scoreDisplay.text = score.ToString();
            multiplierDisplay.text = multiplier.ToString();
        }
        spawnedNotes.Remove(e.Sender);
        Destroy(e.Sender);
    }
}
