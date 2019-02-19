using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour {

    GameObject wld_init;

	// Use this for initialization
	void Start () {
        wld_init = Resources.Load("World") as GameObject;
        Debug.Assert(wld_init);

        GameObject ground = Instantiate(wld_init, Vector3.zero, Quaternion.identity);
        ground.name = "World";
        ground.transform.localScale = new Vector3(5, 1, 5);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
