using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CrowdWorld.Proto;

public class CubeMsg : MonoBehaviour {

    public Cube proto;
    public Cube_Data data;

    public byte[] Id
    {
        get { return proto.Id.ToByteArray(); }
        set { proto.Id.CopyTo(value, 0); }
    }

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
            data.Center = value;
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
            data.Scale = value;
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
            data.Forward = value;
            transform.forward = temp;
        }
    }

    // Use this for initialization
    void Start () {
    }
}
