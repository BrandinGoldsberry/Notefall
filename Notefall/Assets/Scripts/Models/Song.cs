using Assets.Scripts.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Song
{
    //Name of song
    public string Name;
    //Number of notes to spawn
    public int NoteNum;
    //An array of values that store information about when to spawn a note in which location
    public NoteTime[] SpawnTime_SpawnLoc;
    //Sets how fast notes will fall
    //We need to come up with a chart to get this to convert to BPM
    public int SongSpeed;

    //How accurate a player needs to be to hit a note and count it
    //Default: 0.25
    public float MinAccuracy;

    //The song keeps track of how many notes it has spawned
    public int spawned { private set; get; }

    public Song()
    {
        MinAccuracy = 0.50f;
        spawned = 0;
    }

    public Song(NoteTime[] SpawnTimes, int SongSpeed)
    {
        this.SongSpeed = SongSpeed;
        SpawnTime_SpawnLoc = SpawnTimes;
        this.NoteNum = SpawnTimes.Length;
        spawned = 0;
        MinAccuracy = 0.25f;
    }

    public void FixNoteNum()
    {
        NoteNum = SpawnTime_SpawnLoc.Length;
    }

    //Gives the next note to spawn and increments spawn count
    public NoteTime Spawn()
    {
        //Basically if there isn't anymore to spawn return something so we don't get out of index errors
        if(spawned >= NoteNum)
        {
            return new NoteTime(-1f, -1);
        }
        NoteTime ret = SpawnTime_SpawnLoc[spawned];
        spawned++;
        return ret;
    }
}
