using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JukeBoxFloat : MonoBehaviour
{
    public float FloatSpeed = 0.01f;
    public int Top = -5;
    public int Bottom = -20;

    private bool isGoUp = false;

    // Update is called once per frame
    void Update()
    {
        Vector3 newPos = gameObject.transform.position;
        if(newPos.y <= Bottom)
        {
            isGoUp = true;
        } 
        else if(newPos.y >= Top)
        {
            isGoUp = false;
        }
        gameObject.transform.Translate(new Vector3(0, FloatSpeed * (isGoUp ? 1 : -1), 0), Space.World);
    }
}
