using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameBehaviour : MonoBehaviour
{
    public Song Song;
    public GameObject note;

    private GameObject[] SpawnLocations;
    private GameObject hitBar;

    private UnityEvent clickEvent = new UnityEvent();
    private UnityEvent spawnEvent = new UnityEvent();

    private float CurrentTime;

    //stores the next note to spawn
    private KeyValuePair<float, int> nextNote;

    private List<GameObject> spawnedNotes;

    void Start()
    {
        //NOTE: this code is all temporary to get note behaviour initialized
        //TODO: Make programatic and not hard coded
        //Initialize notes
        KeyValuePair<float, int>[] newSpawns =
        {
            new KeyValuePair<float, int>(0f, 0),
            new KeyValuePair<float, int>(2.2f, 2),
            new KeyValuePair<float, int>(5.4f, 1),
            new KeyValuePair<float, int>(6.5f, 0),
            new KeyValuePair<float, int>(7.6f, 1),
            new KeyValuePair<float, int>(8.8f, 2),
            new KeyValuePair<float, int>(9.10f, 2),
        };
        Song = new Song(newSpawns.Length, newSpawns, 10);

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
        nextNote = Song.Spawn();
        //add method to event
        spawnEvent.AddListener(SpawnNote);

        //hitbar set
        hitBar = GameObject.Find("HitBar");
    }

    void Update()
    {
        //If we get this number stop the song
        //Its A) The return of Song.Spawn() if there isn't anymore
        //B) If we get this at any time the thing will break anyway
        if(nextNote.Value <= -1)
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
            if (CurrentTime > nextNote.Key)
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
                    Debug.Log("Click!");
                    //if it hit something then do something
                    if (hit.collider != null)
                    {
                        //Debug.Log(hit.transform.gameObject);
                        Destroy(hit.transform.gameObject);
                    }
                }
            }
        }
    }

    void SpawnNote()
    {
        //create a note and then setup next spawn
        GameObject spawned = Instantiate(note, SpawnLocations[nextNote.Value].transform);
        //add spawned to the list
        spawnedNotes.Add(spawned);
        //Set the note's speed to the speed of the song
        spawned.GetComponent<NoteBehaviour>().Speed = Song.SongSpeed;
        //Debug.Log(spawned);
        //setup next spawn
        nextNote = Song.Spawn();
    }
}
