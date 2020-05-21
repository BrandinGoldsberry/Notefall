using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NoteBehaviour : MonoBehaviour
{
    //NEVER leave this at zero!
    public int Speed { get; set; }

    public event EventHandler<NoteEventArgs> NoteDestoyed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //move relative to speed
        //The delta time multiplier smooths it out so that lag won't cause displacement issues
        //It moves Down the screen so it needs to move in a negative direction
        //I fliped it here so we can think in a positive BPM
        //It would silly if we had -20 beats per minute
        transform.Translate(0, Speed * Time.deltaTime * -1, 0);

        //Basically if you miss a note destory it before it goes too far
        //Helps clean up on screen clutter
        if(transform.position.y <= -20)
        {
            OnDestroyObject(new NoteEventArgs(gameObject));
        }
    }

    public void OnDestroyObject(NoteEventArgs args)
    {
        NoteDestoyed?.Invoke(this, args);
    }
}

public class NoteEventArgs : EventArgs
{
    public GameObject Sender { get; set; }
    public NoteEventArgs(GameObject sender)
    {
        Sender = sender;
    }
}
