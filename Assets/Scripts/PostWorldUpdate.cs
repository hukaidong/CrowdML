using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CrowdWorld.Proto;

public class PostWorldUpdate : MonoBehaviour {
    public delegate void RepUpdateHandler(ref World world);
    public event RepUpdateHandler RepUpdate;
    public virtual void OnRepUpdate(ref World world)
    {
        RepUpdate?.Invoke(ref world);
    }
	
	// Update is called once per frame
	void FixUpdate () {
		
	}
}
