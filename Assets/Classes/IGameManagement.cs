using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameManagement
{
    //Fields for scenes
    SceneDetails[] scenesArray
    {
        get;
        set;
    }

    SceneDetails sceneToChangeTo
    {
        get;
        set;
    }

    bool isSceneChanging
    {
        get;
        set;
    }


    //Fields for objects
    GameObject[] movableObjects
    {
        get;
        set;
    }

    GameObject currentObject
    {
        get;
        set;
    }
    
    string currentRotationAxis
    {
        get;
        set;
    }

    //Fields for cues
    GameObject[] pendingCues {
        get;
        set;
    }

    GameObject[] completedCues
    {
        get;
        set;
    }

    //Fields for button presses
    bool xNeg
    {
        get;
        set;
    }

    bool xPos
    {
        get;
        set;
    }

    bool zNeg
    {
        get;
        set;
    }

    bool zPos
    {
        get;
        set;
    }

    bool yNeg
    {
        get;
        set;
    }

    bool yPos
    {
        get;
        set;
    }

    bool clockRot
    {
        get;
        set;
    }

    bool antiClockRot
    {
        get;
        set;
    }
}
