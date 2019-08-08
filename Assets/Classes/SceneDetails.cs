using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneDetails

{
    private String sceneName;
    private String scenePath;

    public SceneDetails(String name,String path)
    {
        this.sceneName = name;
        this.scenePath = path;
    }

    public String SceneName
    {
        get { return this.sceneName; }
        set { this.sceneName = value; }
    }

    public String ScenePath
    {
        get { return this.scenePath; }
        set { this.scenePath = value; }
    }
}
