using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CueHideScript : MonoBehaviour
{
    public List<GameObject> objects = new List<GameObject>();
    public bool startCue = false;
    public bool stopCue = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (startCue)
        {
            startCue = false;
            foreach (GameObject gameObject in objects)
            {
                gameObject.SetActive(false);
            }
        }

        if (stopCue)
        {
            stopCue = false;
            foreach(GameObject gameObject in objects)
            {
                gameObject.SetActive(true);
            }
        }
    }

    public void startRunningCue()
    {
        startCue = true;
    }
    public void stopRunningCue()
    {
        stopCue = true;
    }
}
