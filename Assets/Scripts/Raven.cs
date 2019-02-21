using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NetMQ;
using ByteComm;

public class Raven : MonoBehaviour {

    private const string sockname = "tcp://*:6566";
    private const string client_sockname = "tcp://localhost:6566";
    private ByteServer server;
    private ByteClient client;

	// Use this for initialization
	void Start () {
        Debug.Log("starting");
        server = new ByteServer(sockname);
        //client = new ByteClient(client_sockname);
        //client.Send_String("Hello");
        //string recv = server.Recv_String();
        //Debug.Log("I recieved " + recv);
        //server.Send_String($"From Unity, I got {recv}");
        //recv = client.Recv_String();
        //Debug.Log(recv);
        //client.Close();
        server.Close();
        NetMQConfig.Cleanup();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Sending msg");

        //Debug.Log(client.Send_And_Recv("hello"));

    }

    void OnApplicationQuit()
    {
    }
}
