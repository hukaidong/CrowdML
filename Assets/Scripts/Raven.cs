using CrowdWorld;
using CrowdWorld.Proto;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class Raven : MonoBehaviour
{
    private const string sockname = "tcp://*:6566";

    private bool shouldReply;
    private SpinServer server;
    public PreWorldUpdate Preupdate;
    public PostWorldUpdate Postupdate;
    [SerializeField] private NavMeshData m_NavMesh;
    [SerializeField] private GameObject _agent_group;
    [SerializeField] private GameObject _obs_group;
    private NavMeshDataInstance m_NavMeshInstance;
    Vector3 BoundsCenter = Vector3.zero;
    Vector3 BoundsSize = new Vector3(999999f, 4000f, 999999f);

    public World ReqWorld, RepWorld;
    public Transform AgentGroupT
    {
        get { return _agent_group.transform; }
    }
    public Transform ObstacleGroupT
    {
        get { return _obs_group.transform; }
    }

    // Use this for initialization
    private void Start()
    {
        server = new SpinServer(sockname);
        Preupdate = GetComponent<PreWorldUpdate>();
        Postupdate = GetComponent<PostWorldUpdate>();
        Debug.Assert(Preupdate != null);
        Debug.Assert(Postupdate != null);
        // Construct and add navmesh
        m_NavMeshInstance = NavMesh.AddNavMeshData(m_NavMesh);
        server.Start();
    }
    private void HandleReq(byte[] reqproto)
    {
        ReqWorld = World.Parser.ParseFrom(reqproto);
        Config_Type gconfig = ReqWorld.Config;
        if (gconfig == Config_Type.Reset)
        { 
            Application.Quit();
        }
        else
        {
            foreach (Cube cube in ReqWorld.Cubes)
            {
                switch (cube.Config)
                {
                    case Config_Type.None:
                        cube.Config = gconfig;
                        goto default;
                    case Config_Type.Create:
                        CubeMsg.CreateCube(this, cube);
                        break;
                    default:
                        Preupdate.OnCubeReqUpdate(cube);
                        break;
                }
            }
            {
                List<NavMeshBuildMarkup> sources = new List<NavMeshBuildMarkup>();
                List<NavMeshBuildSource> m_Sources = new List<NavMeshBuildSource>();
                NavMeshBuilder.CollectSources(
                    new Bounds(BoundsCenter,BoundsSize),
                    LayerMask.NameToLayer("Everything"), 
                    NavMeshCollectGeometry.RenderMeshes,
                    0, sources , m_Sources);
                NavMeshBuilder.UpdateNavMeshData(
                    m_NavMesh,
                    NavMesh.GetSettingsByID(0),
                    m_Sources,
                    new Bounds(BoundsCenter, BoundsSize)
                    );
            }
            foreach (Agent agt in ReqWorld.Agents)
            {
                switch (agt.Config)
                {
                    case Config_Type.None:
                        agt.Config = gconfig;
                        goto default;
                    case Config_Type.Create:
                        AgentMsg.CreateAgent(this, agt);
                        break;
                    default:
                        Preupdate.OnAgtReqUpdate(agt);
                        break;
                }
                Preupdate.OnAgtReqUpdate(agt);
            }
        }

    }
    private void HandleRep(out byte[] repproto)
    {
        RepWorld = new World()
        {
            Id = ReqWorld.Id
        };
        Postupdate.OnRepUpdate(ref RepWorld);
        repproto = RepWorld.ToByteArray();
        Debug.Log(RepWorld.ToString());
    }
    private void FixedUpdate()
    {
        if (shouldReply)
        {
            shouldReply = false;
            HandleRep(out server.RepProto);
            server.RepReady = true;
        }

        if (server.ReqReady)
        {
            server.ReqReady = false;
            HandleReq(server.ReqProto);
            shouldReply = true;
        }
    }
    private void OnApplicationQuit()
    {
        server.Stop();
        NavMesh.RemoveNavMeshData(m_NavMeshInstance);
    }
}



