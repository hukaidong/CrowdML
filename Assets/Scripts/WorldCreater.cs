using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using CrowdWorld.Proto;
using CrowdWorldSample;

public class WorldCreater : MonoBehaviour {
  private byte[] id;

  void SetUpWorld(World w_proto) {

  }

	// Use this for initialization
	void Start () {
        byte[] data = Convert.FromBase64String(Data.Data_base64);
        World w = World.Parser.ParseFrom(data);
        Debug.Log(w.Agents.Count());
        foreach (var agent in w.Agents.ToList().
            Take(10))
        {
            AgentMsg.CreateAgent(agent);
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
