using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour,IGameManagement
{
    private SceneDetails[] scenesArray;
    private String scenesArrayString;
    private String sceneToChangeTo;
    private bool isSceneChanging;

    private GameObject[] movableObjects;
    private String movableObjectsListString;

    private String currentObject;
    private string currentRotationAxis;

    private String pendingCues;
    private String completedCues;
    public GameObject[] pendingCuesArray;

    private bool xNeg;
    private bool xPos;
    private bool zNeg;
    private bool zPos;
    private bool yNeg;
    private bool yPos;
    private bool clockRot;
    private bool antiClockRot;
    static TcpClient client;
    private string currentSceneName;

    //These things are static but I should figure out
    //a better way to implement them
    static TcpListener server;

    #region Interface implementation
    public SceneDetails[] ScenesArray {
        get
        {
            return scenesArray;
        }
        set {
            scenesArray = value;
        } }
    public String SceneToChangeTo { get
        {
            return sceneToChangeTo;
        } set {
            SceneToChangeTo = value;
        }
    }
    public bool IsSceneChanging
    {
        get
        {
            return isSceneChanging;
        }
        set
        {
            isSceneChanging = value;
        }
    }
    public GameObject[] MovableObjects {
        get
        {
            return movableObjects;
        }
        set
        {
            movableObjects = value;
        } }
    public String CurrentObject {
        get
        {
            return currentObject;
        }
        set
        {
            currentObject = value;
        }
    }
    public string CurrentRotationAxis {
        get
        {
            return currentRotationAxis;
        }
        set
        {
            currentRotationAxis = value;
        }
    }
    public String PendingCues {
        get
        {
            return pendingCues;
        }
        set
        {
            pendingCues = value;
        }
    }
    public String CompletedCues {
        get {
            return completedCues;
        }
        set
        {
            completedCues = value;
        }
    }
    public bool Xneg {
        get
        {
            return xNeg;
        }
        set
        {
            xNeg = value;
        }
    }
    public bool Xpos {
        get
        {
            return xPos;
        }
        set
        {
            xPos = value;
        }
    }
    public bool Zneg {
        get
        {
            return zNeg;
        }
        set
        {
            zNeg = value;
        }
    }
    public bool Zpos {
        get
        {
            return zPos;
        }
        set
        {
            zPos= value;
        }
    }
    public bool Yneg {
        get
        {
            return yNeg;
        }
        set
        {
            yNeg = value;
        }
    }
    public bool Ypos {
        get
        {
            return yPos;
        }
        set
        {
            yPos = value;
        }
    }
    public bool ClockRot {
        get
        {
            return clockRot;
        }
        set
        {
            clockRot = value;
        }
    }
    public bool AntiClockRot {
        get
        {
            return antiClockRot;
        }
        set
        {
            antiClockRot = value;
        }
    }

    public string CurrentSceneName {
        get { return currentSceneName; }
        set {
            currentSceneName = value;
        }
    }
    #endregion

    /// <summary>
    /// A method for listener implementation
    /// </summary>
    public void Listen()
    {
        Debug.Log("Listener Execution Initialized");
        #region Setup socket and server if do not exist
        if (client ==null || !client.Connected)
        {
            if (server == null)
            {
                server = new TcpListener(IPAddress.Parse("192.168.137.1"), 3000);
                server.Start();
                client = server.AcceptTcpClient();

            }
        }
        #endregion

        Debug.Log("Preparing network stream for listening");
        Byte[] bytes = new Byte[256];
        String data = null;

        while (true)
        {
            NetworkStream stream = client.GetStream();
            int i;

            byte[] sceneNameBytes = convertTextToBytes(currentSceneName);
            stream.Write(sceneNameBytes, 0, sceneNameBytes.Length);
            while ((i=stream.Read(bytes,0,bytes.Length)) != 0)
            {
                data = convertBytesToText(bytes, 0, i);

                //Printing received data
                Debug.Log(String.Format("Received: {0}", data));

                if (data.Contains("select_movable"))
                {
                    currentObject = data.Replace("selected_movable:", "");
                }
                else if (data.Contains("selected_scene:"))
                {
                    sceneToChangeTo = data.Replace("selected_scene:", "");
                    IsSceneChanging = true;
                }
                else if (data.Contains("cue_screen_opened"))
                {

                } 
                else if (data.Contains("cue_completed_read"))
                {
                    byte[] pendingCueBytes = convertTextToBytes(pendingCues);
                    stream.Write(pendingCueBytes, 0, pendingCueBytes.Length);
                }
                else if (data.Contains("pending_cue_selected:"))
                {
                    String cueToRun = data.Replace("pending_cue_selected:", "");
                    GameObject cueObject = GameObject.Find(cueToRun);
                    gameObject.GetComponent<CueHideScript>().startRunningCue();
                    sendCueToCompleted(cueToRun);
                } 
                else if (data.Contains("completed_cue_selected:"))
                {
                    String cueToRun = data.Replace("completed_cue_selected:", "");
                    GameObject gameObject = GameObject.Find(cueToRun);
                    gameObject.GetComponent<CueHideScript>().stopRunningCue();
                }
                else
                {
                    switch (data)
                    {
                        case "get_all_scenes:":
                            byte[] scenesListBytes = convertTextToBytes(scenesArrayString);
                            stream.Write(scenesListBytes, 0, scenesListBytes.Length);
                            break;
                        case "object_control_opened":
                            byte[] movableObjectsBytes = convertTextToBytes(movableObjectsListString);
                            stream.Write(movableObjectsBytes, 0, movableObjectsBytes.Length);
                            break;
                        case "x_pos_up":
                            Xpos = false;
                            break;
                        case "x_pos_down":
                            Xpos = true;
                            break;
                        case "x_neg_up":
                            Xneg = false;
                            break;
                        case "x_neg_down:":
                            Xneg = true;
                            break;
                        case "y_pos_up":
                            Ypos = false;
                            break;
                        case "y_pos_down":
                            Ypos = true;
                            break;
                        case "y_neg_up":
                            Yneg = false ;
                            break;
                        case "y_neg_down":
                            Yneg = true;
                            break;
                        case "z_pos_up":
                            Zpos = false;
                            break;
                        case "z_pos_down":
                            Zpos = true;
                            break;
                        case "z_neg_up":
                            Zneg = false;
                            break;
                        case "z_neg_down":
                            Zneg = true;
                            break;
                        case "select_x_axis":
                            CurrentRotationAxis = "x";
                            break;
                        case "select_y_axis":
                            CurrentRotationAxis = "y";
                            break;
                        case "select_z_axis":
                            CurrentRotationAxis = "z";
                            break;
                        case "clock_down":
                            ClockRot = true;
                            break;
                        case "clock_up":
                            ClockRot = false;
                            break;
                        case "anti_clock_down":
                            AntiClockRot = true;
                            break;
                        case "anti_clock_up":
                            AntiClockRot = false;
                            break;
                        default:
                            break;
                    }
                }
            }

        }

        client.Close();
        server.Stop();
        
    }

    private string convertBytesToText(byte[] bytes, int v, int i)
    {
        return System.Text.Encoding.ASCII.GetString(bytes, v, i);
    }

    // Start is called before the first frame update
    void Start()
    {
        CurrentSceneName = SceneManager.GetActiveScene().name;
        Debug.Log("Started the current scene: " + CurrentSceneName);

        //Get all movable objects in the scene
        movableObjectsListString = getMovableList();
        currentObject = null;

        //Get a list of all scenes in the build
        currentSceneName = SceneManager.GetActiveScene().name;
        scenesArrayString = getListAllScenes(scenesArray);

        //Get a list of all the cues in the build
        pendingCues = "";
        foreach (GameObject cue in pendingCuesArray)
        {
            pendingCues += cue.name + ";";
        }
        completedCues = "";

        //Start listening 
        Thread thread = new Thread(new ThreadStart(Listen));
        thread.Start();
    }

    private String getListAllScenes(SceneDetails[] sceneArray)
    {
        int sceneCount = UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings;
        sceneArray = new SceneDetails[sceneCount];
        for (int i = 0; i < sceneCount; i++)
        {
            String path = UnityEngine.SceneManagement.SceneUtility.GetScenePathByBuildIndex(i);
            String[] pathElements = path.Split('/');
            String name = pathElements[pathElements.Length - 1];
            SceneDetails sceneDetails = new SceneDetails(name,path);
            sceneArray[i] = sceneDetails;
        }

        String scenesArrayString = "";
        foreach (SceneDetails sceneDetails in sceneArray)
        {
            scenesArrayString += sceneDetails+";";
        }
        return scenesArrayString;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Convert any text to a byte array
    /// </summary>
    /// <param name="input">The string input</param>
    /// <returns>The byte array for the given input</returns>
    private byte[] convertTextToBytes(String input)
    {
        return System.Text.Encoding.ASCII.GetBytes(input);
    }

    /// <summary>
    /// Get a list of all objects that are movable
    /// </summary>
    /// <returns>The list of movable objects in string format</returns>
    private String getMovableList()
    {
        movableObjects = GameObject.FindGameObjectsWithTag("pickup");
        StringBuilder gameObjects = new StringBuilder("");
        foreach (GameObject gameObject in movableObjects)
        {
            gameObjects.Append(gameObject.name.ToString());
            gameObjects.Append(";");
        }
        return gameObjects.ToString();

    }

    public void sendCueToCompleted(String cueName)
    {
        pendingCues = pendingCues.Replace(cueName + ";", "");
        completedCues += cueName + ";";
    }
}
