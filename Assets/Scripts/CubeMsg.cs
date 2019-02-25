using System;
using System.Collections.Generic;
using UnityEngine;
using CrowdWorld.Proto;

public class CubeMsg : MonoBehaviour {

    private Cube _proto;
    private bool _data_changed;

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
            _data_changed = true;
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
            _data_changed = true;
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
            _data_changed = true;
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
        get { return _proto; }
        set
        {
            _proto = value;
            Cube_Data = value.Data;
        }
    }

    public void OnReqUpdate(Cube cube)
    {
        switch (cube.Config)
        {
            case Config_Type.None:
            case Config_Type.Current: break;
            case Config_Type.Create:
                _data_changed = true; break;
            case Config_Type.Query:
                HandleQuery(cube);
                break;
            case Config_Type.Update:
                Proto_Data = cube;
                break;
            default:
                throw new NotImplementedException($"{cube.Config.ToString()}, Unexcepted State");
        }

    }
    public void OnRepUpdate(ref World world)
    {
        Debug.Log("Cube fire!");
        if (world.Config.Equals(Config_Type.Current) || _data_changed)
        {
            world.Cubes.Add(_proto);
        }
        else 
        {
            switch (_proto.Config)
            {
                case Config_Type.Current:
                    world.Cubes.Add(_proto);
                    break;
                default: break;
            }
        }
        _proto.Config = Config_Type.None;
        _data_changed = false;
    }

    private void HandleQuery(Cube cube)
    {
        throw new NotImplementedException("Function Not Supported Yet");
    }

    static public
    CubeMsg CreateCube(Raven raven, Cube c_proto)
    {
        GameObject cubePrefab = Resources.Load("Cube") as GameObject;
        GameObject cube = Instantiate(cubePrefab);
        CubeMsg msg = cube.AddComponent<CubeMsg>() as CubeMsg;
        msg.Proto_Data = c_proto;
        cube.name = "cube_" + c_proto.Id.ToBase64();
        raven.Preupdate.CubeReqUpdate.Add(c_proto.Id.ToBase64(), msg.OnReqUpdate);
        raven.Postupdate.RepUpdate += msg.OnRepUpdate;
        cube.transform.parent = raven.transform;
        return msg;
    }
}
