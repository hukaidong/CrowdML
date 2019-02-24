using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CrowdWorld.Proto;

public class PreWorldUpdate : MonoBehaviour {

    public delegate void ReqUpdateHandler<T>(object source, T proto);
    public Dictionary<string, ReqUpdateHandler<Agent>> AgentReqUpdate;
    protected virtual void OnAgtReqUpdate(Agent agt)
    {
        AgentReqUpdate[agt.Id.ToBase64()]?.Invoke(this, agt);
    }
    public Dictionary<string, ReqUpdateHandler<Cube>> CubeReqUpdate;
    protected virtual void OnCubeReqUpdate(Cube cube)
    {
        CubeReqUpdate[cube.Id.ToBase64()]?.Invoke(this, cube);
    }

    // Update is called once per frame
    void FixUpdate () {
		
	}
}
