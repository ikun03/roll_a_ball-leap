using System;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using Unity.Jobs;
using UnityEngine;
using System.Collections;
using System.Text;

public class GameManagerScriptSecond : MonoBehaviour
{

    private GameObject currentObject;
    String movableObjects;

    String currentObjectName;
    private bool xNeg;
    private bool xPos;
    private bool zPos;
    private bool zNeg;

    private bool yPos;

    private bool yNeg;

    private bool clockRot;
    private bool antiClockRot;

    String currentAxis;


    // Start is called before the first frame update
    private void Start()
    {
        Debug.Log("Started Game Manager");
        movableObjects = getMovableList();
        currentObjectName = null;
        Thread thread = new Thread(new ThreadStart(listen));
        thread.Start();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (currentObjectName == null)
        {
            return;
        }
        else
        {
            currentObject = GameObject.Find(currentObjectName);
        }
        if (xPos)
        {
            currentObject.transform.position = new Vector3(currentObject.transform.position.x + 0.1f,
                                                        currentObject.transform.position.y,
                                                        currentObject.transform.position.z);
        }
        if (xNeg)
        {
            currentObject.transform.position = new Vector3(currentObject.transform.position.x - 0.1f,
                                                        currentObject.transform.position.y,
                                                        currentObject.transform.position.z);
        }
        if (yPos)
        {
            currentObject.transform.position = new Vector3(currentObject.transform.position.x,
                                                        currentObject.transform.position.y + 0.1f,
                                                        currentObject.transform.position.z);
        }
        if (yNeg)
        {
            currentObject.transform.position = new Vector3(currentObject.transform.position.x,
                                                                    currentObject.transform.position.y - 0.1f,
                                                                    currentObject.transform.position.z);
        }
        if (zPos)
        {
            currentObject.transform.position = new Vector3(currentObject.transform.position.x,
                                                                    currentObject.transform.position.y,
                                                                    currentObject.transform.position.z + 0.1f);
        }
        if (zNeg)
        {
            currentObject.transform.position = new Vector3(currentObject.transform.position.x,
                                                                    currentObject.transform.position.y,
                                                                    currentObject.transform.position.z - 0.1f);
        }
        if (currentAxis == null)
        {
            return;
        }
        else
        {
            if (clockRot)
            {
                switch (currentAxis)
                {
                    case "x":
                        currentObject.transform.Rotate(3, 0, 0);
                        break;
                    case "y":
                        currentObject.transform.Rotate(0, 3, 0);
                        break;
                    case "z":
                        currentObject.transform.Rotate(0, 0, 3);
                        break;
                }
            }
            if (antiClockRot)
            {
                switch (currentAxis)
                {
                    case "x":
                        currentObject.transform.Rotate(-3, 0, 0);
                        break;
                    case "y":
                        currentObject.transform.Rotate(0, -3, 0);
                        break;
                    case "z":
                        currentObject.transform.Rotate(0, 0, -3);
                        break;
                }
            }
        }
    }
    /*
        static void Connect(String server, String message)
        {
            try
            {
                int portNo = 13000;
                TcpClient client = new TcpClient(server, portNo);
                Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);

                NetworkStream stream = client.GetStream();

                stream.Write(data, 0, data.Length);

                Console.WriteLine("Sent: {0}", message);

                data = new Byte[256];

                String responseData = String.Empty;

                int bytes = stream.Read(data, 0, data.Length);
                responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                Console.WriteLine("Received: {0}", responseData);

                stream.Close();
                client.Close();
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }

            Console.WriteLine("\n Press Enter to continue...");
            Console.Read();
        }
    */
    public void listen()
    {
        Debug.Log("Executed");
        TcpListener server = null;
        try
        {
            IPAddress address = IPAddress.Parse("192.168.137.1");
            server = new TcpListener(address, 3000);

            server.Start();

            Byte[] bytes = new Byte[256];
            String data = null;

            Debug.Log("Waiting for a connection...");

            TcpClient client = server.AcceptTcpClient();
            Debug.Log("Connected!");

            data = null;

            while (true)
            {
                NetworkStream stream = client.GetStream();
                int i;
                byte[] movableObjectsByteArray = System.Text.Encoding.ASCII.GetBytes(movableObjects);
                stream.Write(movableObjectsByteArray, 0, movableObjectsByteArray.Length);
                while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                {
                    data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                    Debug.Log(String.Format("Received: {0}", data));

                    if (data.Contains("select_movable"))
                    {
                        currentObjectName = data.Replace("select_movable:", "");
                    }
                    else
                    {
                        switch (data)
                        {
                            case "x_pos_up":
                                xPos = false;
                                break;
                            case "x_pos_down":
                                xPos = true;
                                break;
                            case "x_neg_up":
                                xNeg = false;
                                break;
                            case "x_neg_down":
                                xNeg = true;
                                break;
                            case "y_pos_up":
                                yPos = false;
                                break;
                            case "y_pos_down":
                                yPos = true;
                                break;
                            case "y_neg_up":
                                yNeg = false;
                                break;
                            case "y_neg_down":
                                yNeg = true;
                                break;
                            case "z_pos_up":
                                zPos = false;
                                break;
                            case "z_pos_down":
                                zPos = true;
                                break;
                            case "z_neg_up":
                                zNeg = false;
                                break;
                            case "z_neg_down":
                                zNeg = true;
                                break;
                            case "select_x_axis":
                                currentAxis = "x";
                                break;
                            case "select_y_axis":
                                currentAxis = "y";
                                break;
                            case "select_z_axis":
                                currentAxis = "z";
                                break;
                            case "clock_down":
                                clockRot = true;
                                break;
                            case "clock_up":
                                clockRot = false;
                                break;
                            case "anti_clock_down":
                                antiClockRot = true;
                                break;
                            case "anti_clock_up":
                                antiClockRot = false;
                                break;
                            default:
                                break;
                        }
                    }

                }

            }
            client.Close();
        }
        catch (SocketException e)
        {
            Debug.Log(String.Format("SocketException: {0}", e));
        }
        finally
        {
            server.Stop();
        }

    }

    private String getMovableList()
    {
        GameObject[] objectList = GameObject.FindGameObjectsWithTag("pickup");
        StringBuilder gameObjects = new StringBuilder("");
        foreach (GameObject gameObject in objectList)
        {
            gameObjects.Append(gameObject.name.ToString());
            gameObjects.Append(";");
        }
        return gameObjects.ToString();

    }
}