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
        byte[] data = Convert.FromBase64String(Data.Data_base64_office);
        //byte[] data = Convert.FromBase64String(Data.Data_base64_fourways);
        World w = World.Parser.ParseFrom(data);
        Debug.Log(w.Agents.Count());
        foreach (var agent in w.Agents.ToList() )
        {
            AgentMsg.CreateAgent(agent);
        }

        foreach (var cube in w.Cubes.ToList())
        {
            CubeMsg.CreateCube(cube);
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
