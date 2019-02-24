using CrowdWorld;
using CrowdWorld.Proto;
using Google.Protobuf;
using UnityEngine;

public class Raven : MonoBehaviour
{
    private const string sockname = "tcp://*:6566";
    private SpinServer server;
    private bool shouldReply;
    private PreWorldUpdate preupdate;
    private PostWorldUpdate postupdate;

    public World ReqWorld, RepWorld;

    // Use this for initialization
    private void Start()
    {
        server = new SpinServer(sockname);
        server.Start();
        preupdate = GetComponent<PreWorldUpdate>();
        postupdate = GetComponent<PostWorldUpdate>();
    }

    private void FixedUpdate()
    {
        if (shouldReply)
        {
            HandleRep(out server.RepProto);
            server.RepReady = true;
            shouldReply = false;
        }

        if (server.ReqReady)
        {
            HandleReq(server.ReqProto);
            shouldReply = true;
        }
    }

    private void WorldCreater()
    {

    }

    private void HandleReq(byte[] reqproto)
    {
        Config_Type gconfig = ReqWorld.Config;
        ReqWorld = World.Parser.ParseFrom(reqproto);
        if (gconfig == Config_Type.Create)
        {
            WorldCreater();
        }
            else
        {
            foreach (Agent agt in ReqWorld.Agents)
            {
                switch (agt.Config)
                {
                    case Config_Type.None:
                        agt.Config = gconfig;
                        goto default;
                    case Config_Type.Create:
                        AgentMsg.CreateAgent(agt);
                        break;
                    default:
                        preupdate.OnAgtReqUpdate(agt);
                        break;
                }
                preupdate.OnAgtReqUpdate(agt);
            }
            foreach (Cube cube in ReqWorld.Cubes)
            {
                switch (cube.Config)
                {
                    case Config_Type.None:
                        cube.Config = gconfig;
                        goto default;
                    case Config_Type.Create:
                        CubeMsg.CreateCube(cube);
                        break;
                    default:
                        preupdate.OnCubeReqUpdate(cube);
                        break;
                }
            }
        }
    }

    private void HandleRep(out byte[] repproto)
    {
        postupdate.OnRepUpdate();
        repproto = RepWorld.ToByteArray();
    }

    private void OnApplicationQuit()
    {
        server.Stop();
    }
}



