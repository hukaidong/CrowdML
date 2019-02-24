﻿using UnityEngine;
using UnityEngine.AI;
using CrowdWorld.Proto;
using System;
using System.Linq;

public class AgentMsg : MonoBehaviour
{
    private Agent _proto;
    private bool _data_changed;
    protected Rigidbody rb;
    protected NavMeshAgent agt;

    public Vec3 Velocity
    {
        get
        {
            Vec3 temp = new Vec3
            {
                X = rb.velocity.x,
                Y = rb.velocity.y,
                Z = rb.velocity.z
            };
            return temp;
        }
        set
        {
            Vector3 temp = new Vector3((float)value.X, (float)value.Y, (float)value.Z);
            rb.velocity = temp;
            _data_changed = true;
        }
    }
    public Vec3 Location
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
    public Vec3 Forwards
    {
        get
        {
            Vec3 temp = new Vec3
            {
                X = transform.forward.x,
                Y = transform.forward.y,
                Z = transform.forward.z,
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
    public Vec3 Target
    {
        get
        {
            Vec3 temp = new Vec3
            {
                X = agt.destination.x,
                Y = agt.destination.y,
                Z = agt.destination.z,
            };
            return temp;
        }
        set
        {
            Vector3 temp = new Vector3((float)value.X, (float)value.Y, (float)value.Z);
            agt.destination = temp;
            _data_changed = true;
        }
    }
    public Agent_Data Agent_Data
    {
        get
        {
            Agent_Data temp = new Agent_Data
            {
                Velocity = Velocity,
                Location = Location,
                Forwards = Forwards,
                Target = Target
            };
            return temp;
        }
        set
        {
            Velocity = value.Velocity != null && value.Velocity.HasValue ? value.Velocity : Velocity;
            Location = value.Location != null && value.Location.HasValue ? value.Location : Location;
            Forwards = value.Forwards != null && value.Forwards.HasValue ? value.Forwards : Forwards;
            Target = value.Target != null && value.Target.HasValue ? value.Target : Target;
        }

    }
    public Agent Proto_Data
    {
        get { return _proto; }
        set {
            _proto = value;
            Agent_Data = value.Data.Last();
        }
    }

    public void OnReqUpdate(Agent agt)
    {
        switch (agt.Config)
        {
            case Config_Type.None:
                break;
            case Config_Type.Current:
                break;
            case Config_Type.History:
                goto case Config_Type.Hybrid;
            case Config_Type.Hybrid:
                throw new NotImplementedException("Function Not Supported Yet");
            case Config_Type.Query:
                HandleQuery(agt);
                break;
            case Config_Type.Update:
                Proto_Data = agt;
                break;
            default:
                throw new NotImplementedException("Unexcepted State");
        }

    }
    public void OnRepUpdate(ref World world)
    {
        switch (_proto.Config)
        {
            case Config_Type.None:
                break;
            case Config_Type.Current:
                world.Agents.Add(_proto);
                break;
            default:
                if (_data_changed)
                {
                    world.Agents.Add(_proto);
                }
                break;
        }
        _proto.Config = Config_Type.None;
        _data_changed = false;
    }
    private void HandleQuery(Agent agt)
    {
        throw new NotImplementedException("HandleQuery Not available yet");
    }

    static public 
        GameObject CreateAgent(Agent a_proto)
    {
        GameObject agentPrefab = Resources.Load("Agent") as GameObject;
        GameObject agent = Instantiate(agentPrefab);
        AgentMsg msg = agent.AddComponent<AgentMsg>() as AgentMsg;
        msg.rb = agent.GetComponent<Rigidbody>() as Rigidbody;
        msg.agt = agent.GetComponent<NavMeshAgent>() as NavMeshAgent;
        msg.Proto_Data = a_proto;
        agent.name = a_proto.Nickname == "" ? a_proto.Nickname : a_proto.Id.ToBase64();
        
        if (a_proto.Query.Count > 0)
        {
            var args = (from query in a_proto.Query
                       where query.Name == "regionBounds" select query.Args)
                       .First();
            msg.Location = new Vec3 {
                X = UnityEngine.Random.Range((float)args[0], (float)args[1]),
                Y = 0,
                Z = UnityEngine.Random.Range((float)args[4], (float)args[5])};
        }
        return agent;
    }

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        agt = GetComponent<NavMeshAgent>();
    }

    private void FixedUpdate()
    {
        if (!rb.isKinematic)
        {
            _data_changed = true;
        }
    }
}
