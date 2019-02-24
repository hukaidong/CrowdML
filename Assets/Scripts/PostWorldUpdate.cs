using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CrowdWorld.Proto;

public class PostWorldUpdate : MonoBehaviour {
    public World RepWorld;
    public delegate void RepUpdateHandler(ref World world);
    public event RepUpdateHandler RepUpdate;
    protected virtual void OnRepUpdate()
    {
        RepUpdate?.Invoke(ref RepWorld);
    }
	
	// Update is called once per frame
	void FixUpdate () {
		
	}
}
