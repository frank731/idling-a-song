using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    public List<GameObject> textBoxes;
    private int index = 0;
    private float lastClick = 0;
    public void OnClick()
    {
        if (Time.time >= lastClick + 1.5f)
        {
            lastClick = Time.time;
            textBoxes[index].SetActive(false);
            index++;
            if (index < textBoxes.Count)
            {
                textBoxes[index].SetActive(true);
            }
        }
    }
}
