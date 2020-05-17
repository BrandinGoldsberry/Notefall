using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugText : MonoBehaviour
{
    public Canvas Canvas;

    private List<KeyValuePair<GameObject, GameObject>> KeyNote_ValueText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(KeyNote_ValueText != null && KeyNote_ValueText.Count > 0)
        {
            foreach (KeyValuePair<GameObject, GameObject> item in KeyNote_ValueText)
            {
                if (item.Value.gameObject != null)
                {
                    item.Value.gameObject.transform.position = item.Key.gameObject.transform.position;
                }
            }
        }
    }

    public void UpdateDebugText()
    {
        if (KeyNote_ValueText != null)
        {
            KeyNote_ValueText.Clear();
        }
        else
        {
            KeyNote_ValueText = new List<KeyValuePair<GameObject, GameObject>>();
        }
        GameObject[] notes = GameObject.FindGameObjectsWithTag("Note");
        foreach(GameObject note in notes) 
        {
            GameObject newText = new GameObject();
            RectTransform rectTransform = newText.AddComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(0, 0);
            rectTransform.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

            Text text = newText.AddComponent<Text>();
            text.text = note.name;
            text.fontSize = 14;
            text.color = Color.white;
            text.font = new Font("Arial");
            KeyNote_ValueText.Add(new KeyValuePair<GameObject, GameObject>(note, newText));
        }
    }
}
