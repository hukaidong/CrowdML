using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CrowdWorld.Proto;

public class PreWorldUpdate : MonoBehaviour {
    
    public delegate void ReqUpdateHandler<T>(T proto);
    public Dictionary<string, ReqUpdateHandler<Agent>> AgentReqUpdate;
    public virtual void OnAgtReqUpdate(Agent agt)
    {
        AgentReqUpdate[agt.Id]?.Invoke(agt);
    }
    public Dictionary<string, ReqUpdateHandler<Cube>> CubeReqUpdate;
    public virtual void OnCubeReqUpdate(Cube cube)
    {
        CubeReqUpdate[cube.Id.ToBase64()]?.Invoke(cube);
    }
    private void Start()
    {
        AgentReqUpdate = new Dictionary<string, ReqUpdateHandler<Agent>>();
        CubeReqUpdate = new Dictionary<string, ReqUpdateHandler<Cube>>();
    }
    // Update is called once per frame
    void FixUpdate () {
		
	}
}
