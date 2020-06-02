using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovingGradient : MonoBehaviour
{
    public Color PrimaryColor;
    public Color SecondaryColor;

    private Material mat;
    private float timeOffset = 0.8f;

    // Start is called before the first frame update
    void Start()
    {
        mat = gameObject.GetComponent<Renderer>().material;
        float height = 2.0f * Mathf.Tan(0.5f * Camera.main.fieldOfView * Mathf.Deg2Rad);
        float width = height * Screen.width / Screen.height;
        Vector3 newScale = new Vector3(height, 1f, width);
        gameObject.transform.localScale = newScale;
        InvokeRepeating("UpdateTex", 0f, 0.02f);
    }

    // Update is called once per frame
    void UpdateTex()
    {
        mat.mainTexture = GetTexture(1000);
        timeOffset += 0.001f;
        if(timeOffset >= 1)
        {
            timeOffset = -1f;
        }
    }

    private Color Evaluate(float time)
    {
        return Color.Lerp(PrimaryColor, SecondaryColor, time);
    }

    private Texture2D GetTexture(int width)
    {
        Texture2D texture = new Texture2D(width, 1);
        Color[] colors = new Color[width];
        for (int i = 0; i < width; i++)
        {
            float time = i / (width - 1f);
            float inputVal = Mathf.Sin((time + timeOffset) * Mathf.PI);
            colors[i] = Evaluate(inputVal);
        }
        texture.SetPixels(colors);
        texture.Apply();
        return texture;
    }
}
