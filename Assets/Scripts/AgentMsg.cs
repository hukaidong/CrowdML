using UnityEngine;
using UnityEngine.AI;
using CrowdWorld.Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Configuration;

public class AgentMsg : MonoBehaviour
{
    private Agent _proto;
    private bool _data_changed;
    private Agent_Data _detailchanged;
    private GameObject targetObj;
    protected Rigidbody rb;
    protected NavMeshAgent agt;

    [SerializeField] bool _show_target;

    public Vec3 Force
    {
        set
        {
            if (value == null) { return; }
            Vector3 temp = new Vector3((float)value.X, (float)value.Y, (float)value.Z);
            rb.AddForce(temp);
            _data_changed = true;
        }
    }
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
            targetObj.transform.position = temp;
            _data_changed = true;
        }
    }
    public Agent_Data Agent_Data
    {
        get
        {
            Agent_Data temp = new Agent_Data()
            {
                Velocity = Velocity,
                Location = Location,
                Forwards = Forwards,
                Target = Target,
            };
            return temp;
        }
        set
        {
            Velocity = value.Velocity != null && value.Velocity.HasValue ? value.Velocity : Velocity;
            Location = value.Location != null && value.Location.HasValue ? value.Location : Location;
            Forwards = value.Forwards != null && value.Forwards.HasValue ? value.Forwards : Forwards;
            Target = value.Target != null && value.Target.HasValue ? value.Target : Target;
            Force = value.Force != null && value.Force.HasValue ? value.Force : null;
        }

    }
    public Agent Proto_Data
    {
        get { return _proto; }
        set {
            _proto = value;
            Agent_Data = value.Data;
        }
    }

    public void OnReqUpdate(Agent agt)
    {
        switch (agt.Config)
        {
            case Config_Type.None:
                break;
            case Config_Type.Create:
                _data_changed = true;
                break;
            case Config_Type.Current:
                break;
            case Config_Type.History:
                break;
            case Config_Type.Hybrid:
                throw new NotImplementedException("Function Not Supported Yet");
            case Config_Type.Query:
                HandleQuery(agt);
                break;
            case Config_Type.Update:
                Proto_Data = agt;
                break;
            default:
                throw new NotImplementedException($"{agt.Config.ToString()}, Unexcepted State");
        }

    }
    public void OnRepUpdate(ref World world)
    {
        switch (_proto.Config)
        {
            case Config_Type.None:
                break;
            case Config_Type.Create:
                goto case Config_Type.Current;
            case Config_Type.Current:
                Agent temp = new Agent()
                {
                    Id = _proto.Id,
                    Timestamp = Time.frameCount
                };
                temp.Data = Agent_Data;
                world.Agents.Add(temp);
                break;
            case Config_Type.History:
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

    private void HandleQuery(Agent proto)
    {
        throw new NotImplementedException("HandleQuery Not available yet");
    }

    static public AgentMsg CreateAgent(Raven raven, Agent a_proto)
    {
        GameObject agentPrefab = Resources.Load("Agent") as GameObject;
        GameObject targetPrefab = Resources.Load("Cube") as GameObject;
        GameObject agent = Instantiate(agentPrefab);
        AgentMsg msg = agent.AddComponent<AgentMsg>() as AgentMsg;
        msg.targetObj = Instantiate(targetPrefab);
        msg.targetObj.transform.parent = msg.transform;
        msg.rb = agent.GetComponent<Rigidbody>() as Rigidbody;
        msg.agt = agent.GetComponent<NavMeshAgent>() as NavMeshAgent;
        msg.Proto_Data = a_proto;
        agent.name = (a_proto.Nickname == null) ? a_proto.Nickname : "Agent_"+a_proto.Id.Substring(0, 6);
        agent.transform.parent = raven.AgentGroupT;

        Dictionary<string, Action < QueryArgs >> ReqFunc = new Dictionary<string, Action<QueryArgs>>();
        ReqFunc.Add("agentRegion.regionBound", msg.AgentRegion_RegionBound);

        foreach (string Q in ReqFunc.Keys)
            if (a_proto.Queries.ContainsKey(Q))
            {
                ReqFunc[Q](a_proto.Queries[Q]);
                a_proto.Queries.Remove(Q);
            }

#if true
        msg.rb.isKinematic = true;
        msg.agt.updatePosition = true;
#endif

        raven.Preupdate.AgentReqUpdate.Add(a_proto.Id, msg.OnReqUpdate);
        raven.Postupdate.RepUpdate += msg.OnRepUpdate; 
        return msg;
        
    }

    void AgentRegion_RegionBound(QueryArgs Q)
    {
        var args = Q.Args;
        var randX = UnityEngine.Random.Range((float)args[0], (float)args[1]);
        var randZ = UnityEngine.Random.Range((float)args[4], (float)args[5]);
        Location = new Vec3
        {
            X = randX,
            Y = 2,
            Z = randZ
        };

    }
    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        agt = GetComponent<NavMeshAgent>();
        agt.updatePosition = false;
    }
    private void FixedUpdate()
    {
        if (!rb.isKinematic)
        {
            _data_changed = true;
            _proto.Data = Agent_Data;
        }
        if (_show_target)
        {
            Debug.DrawLine(transform.position, agt.destination);
        }
    }
}
