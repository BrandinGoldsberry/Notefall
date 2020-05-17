﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Song
{
    //Number of notes to spawn
    public int NoteNum;
    //An array of values that store information about when to spawn a note in which location
    public KeyValuePair<float, int>[] SpawnTime_SpawnLoc;
    //Sets how fast notes will fall
    //We need to come up with a chart to get this to convert to BPM
    public int SongSpeed;

    //How accurate a player needs to be to hit a note and count it
    //Default: 0.25
    public float MinAccuracy;

    //The song keeps track of how many notes it has spawned
    public int spawned { private set; get; }

    public Song(int NoteNum, KeyValuePair<float, int>[] SpawnTimes, int SongSpeed)
    {
        this.SongSpeed = SongSpeed;
        this.NoteNum = NoteNum;
        SpawnTime_SpawnLoc = SpawnTimes;
        spawned = 0;
        MinAccuracy = 0.25f;
    }

    //Gives the next note to spawn and increments spawn count
    public KeyValuePair<float, int> Spawn()
    {
        //Basically if there isn't anymore to spawn return something so we don't get out of index errors
        if(spawned >= NoteNum)
        {
            return new KeyValuePair<float, int>(-1f, -1);
        }
        KeyValuePair<float, int> ret = SpawnTime_SpawnLoc[spawned];
        spawned++;
        return ret;
    }
}
