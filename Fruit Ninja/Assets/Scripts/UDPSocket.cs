
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class UDPSocket : MonoBehaviour
{
    public bool cursorDetected;
    private float x;
    private float y;
    public Vector2 pos = new Vector2();

    bool connected;
    private GameObject check;

    [HideInInspector] public bool isTxStarted = false;

    [SerializeField] string IP = "127.0.0.1"; // local host
    [SerializeField] int rxPort = 8000; // port to receive data from Python on

    // Create necessary UdpClient objects
    UdpClient client;
    IPEndPoint remoteEndPoint;
    Thread receiveThread; // Receiving Thread

    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        check = GameObject.Find("Connection Check");
        check.SetActive(false);
        connected = false;

        // Create local client
        client = new UdpClient(rxPort);

        // local endpoint define (where messages are received)
        // Create a new thread for reception of incoming messages
        receiveThread = new Thread(new ThreadStart(ReceiveData));
        receiveThread.IsBackground = true;
        receiveThread.Start();

        // Initialize (seen in comments window)
        print("UDP Comms Initialised");
    }

    private void Update()
    {
        if (connected && SceneManager.GetActiveScene().buildIndex == 0)
        {
            check.SetActive(true);
        }
    }

    // Receive data, update packets received
    private void ReceiveData()
    {
        while (true)
        {
            try
            {
                IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, 0);
                byte[] data = client.Receive(ref anyIP);
                string text = Encoding.UTF8.GetString(data);

                CalcCords(text);

                connected = true;
                print(">> " + text);
            }
            catch (Exception err)
            {
                print(err.ToString());
            }
        }
    }


    void CalcCords(string input) {
        if (input.Substring(0, 1) == "T") {
            cursorDetected = true;
        }
        else if (input.Substring(0, 1) == "F") {
            cursorDetected = false;
        }
        
        x = int.Parse(input.Substring(1, 3));
        y = int.Parse(input.Substring(4, 3));

        x = (float)(x * 36 / 255) - 18;
        y = (float)(y * 20 /255) - 10;
        pos = new Vector2(x, y);
    }

    //Prevent crashes - close clients and threads properly!
    void OnDisable()
    {
        if (receiveThread != null)
            receiveThread.Abort();

        client.Close();
    }
}