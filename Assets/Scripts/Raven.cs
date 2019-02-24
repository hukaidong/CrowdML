using UnityEngine;
using Google.Protobuf;
using CrowdWorld;
using CrowdWorld.Proto;
using System;
using System.Collections.Generic;

public class Raven : MonoBehaviour {	
    private const string sockname = "tcp://*:6566";
	private SpinServer server;
	private bool shouldReply;

    public World RepWorld;

	// Use this for initialization
	void Start () {
        server = new SpinServer(sockname);
		server.Start ();
    }

    void FixedUpdate()
    {
		if (shouldReply) {
			HandleRep (out server.RepProto);
			server.RepReady = true;
			shouldReply = false;
		}

		if (server.ReqReady) {
			HandleReq (server.ReqProto);
			shouldReply = true;
		}
    }

	void HandleReq(byte[] reqproto) {
		Debug.Log ($" I got {reqproto.ToString()} ");
	}

	void HandleRep(out byte[] repproto) {
        repproto = RepWorld.ToByteArray();
	}





    void OnApplicationQuit()
    {
		server.Stop ();
    }
}



