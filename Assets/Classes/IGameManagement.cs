using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public interface IGameManagement
{
    //Fields for scenes
    SceneDetails[] ScenesArray
    {
        get;
        set;
    }

    String SceneToChangeTo
    {
        get;
        set;
    }

    bool IsSceneChanging
    {
        get;
        set;
    }

    string CurrentSceneName
    {
        get;
        set;
    }


    //Fields for objects
    GameObject[] MovableObjects
    {
        get;
        set;
    }

    String CurrentObject
    {
        get;
        set;
    }
    
    string CurrentRotationAxis
    {
        get;
        set;
    }

    //Fields for cues
    String PendingCues {
        get;
        set;
    }

    String CompletedCues
    {
        get;
        set;
    }

    //Fields for button presses
    bool Xneg
    {
        get;
        set;
    }

    bool Xpos
    {
        get;
        set;
    }

    bool Zneg
    {
        get;
        set;
    }

    bool Zpos
    {
        get;
        set;
    }

    bool Yneg
    {
        get;
        set;
    }

    bool Ypos
    {
        get;
        set;
    }

    bool ClockRot
    {
        get;
        set;
    }

    bool AntiClockRot
    {
        get;
        set;
    }

    void Listen();
}
