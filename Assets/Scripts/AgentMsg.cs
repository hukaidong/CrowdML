using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using CrowdWorld.Proto;

public class AgentMsg : MonoBehaviour
{
    public Agent proto;
    public Agent_Data data;
    Rigidbody rb;
    NavMeshAgent agt;

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
            data.Velocity = value;
            rb.velocity = temp;
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
            data.Location = value;
            transform.position = temp;
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
            data.Forwards = value;
            transform.forward = temp;
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
            data.Target = value;
            agt.destination = temp;
        }
    }


    // Update is called once per frame
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
            Velocity = value.Velocity;
            Location = value.Location;
            Forwards = value.Forwards;
            Target = value.Target;
        }

    }

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        agt = GetComponent<NavMeshAgent>();
    }
}
