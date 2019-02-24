using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CrowdWorld.Proto;

public class CubeMsg : MonoBehaviour {

    public Cube proto;
    
    public Vec3 Center
    {
        get
        {
            Vec3 temp = new Vec3
            {
                X = transform.position.x,
                Y = transform.position.y,
                Z = transform.position.z
            };
            return temp;
        }
        set
        {
            Vector3 temp = new Vector3((float)value.X, (float)value.Y, (float)value.Z);
            transform.position = temp;
        }
    }
    public Vec3 Scale
    {
        get
        {
            Vec3 temp = new Vec3
            {
                X = transform.localScale.x,
                Y = transform.localScale.y,
                Z = transform.localScale.z
            };
            return temp;
        }
        set
        {
            Vector3 temp = new Vector3((float)value.X, (float)value.Y, (float)value.Z);
            transform.localScale = temp;
        }
    }
    public Vec3 Forwards
    {
        get
        {
            Vec3 temp = new Vec3
            {
                X = transform.forward.x,
                Y = transform.forward.y,
                Z = transform.forward.z
            };
            return temp;
        }
        set
        {
            Vector3 temp = new Vector3((float)value.X, (float)value.Y, (float)value.Z);
            transform.forward = temp;
        }
    }
    public Cube_Data Cube_Data
    {
        get
        {
            Cube_Data temp = new Cube_Data {
                Center = Center,
                Scale = Scale,
                Forward = Forwards
            };
            return temp;
        }
        set
        {
            Center = value.Center != null && value.Center.HasValue ? value.Center : Center;
            Scale = value.Scale != null && value.Scale.HasValue ? value.Scale : Scale;
            Forwards = value.Forward != null && value.Forward.HasValue ? value.Forward : Forwards;
        }
    }
    public Cube Proto_Data
    {
        get { return proto; }
        set
        {
            proto = value;
            Cube_Data = value.Data;
        }
    }

    public void OnReqUpdate(Cube cube) { }
    public void OnRepUpdate(ref World world) { }

    static public
        GameObject CreateCube(Cube c_proto)
    {
        GameObject cubePrefab = Resources.Load("Cube") as GameObject;
        GameObject cube = Instantiate(cubePrefab);
        CubeMsg msg = cube.AddComponent<CubeMsg>() as CubeMsg;
        msg.Proto_Data = c_proto;
        cube.name = "cube_" + c_proto.Id.ToBase64();
        
        return cube;
    }

    // Use this for initialization
    void Start () {
    }
}
