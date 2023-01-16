using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking;
using UnityEngine;

public class NetworkGUI : NetworkBehaviour
{
    static public NetworkVariable<ServerVars> serverVars = new NetworkVariable<ServerVars>(new ServerVars());

    private void OnServerInitialized()
    {
        serverVars.Initialize(this);
        ServerVars var = new ServerVars();
        var.matchEndTimestamp = DateTime.MinValue;
        var.blueScore = 0;
        var.redScore = 0;
        serverVars.Value = var;
    }

    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10, 10, 300, 300));
        if (!NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsServer)
        {
            StartButtons();
        }
        else
        {
            StatusLabels();

            SubmitNewPosition();
        }

        GUILayout.EndArea();
        if (NetworkManager.Singleton.IsClient || NetworkManager.Singleton.IsServer)
        {
            GUILayout.BeginArea(new Rect(350, 10, 300, 300));
            if (NetworkManager.Singleton.IsServer)
            {
                ServerButtons();
                serverVars.Initialize(this);
            }
            ShowServerVars();
            GUILayout.EndArea();
        }
    }

    static void ServerButtons()
    {
        if (!serverVars.Value.runningMatch)
        {
            if (GUILayout.Button("Start Match")) { ServerVars var = serverVars.Value; var.matchEndTimestamp = DateTime.UtcNow.AddMinutes(7); serverVars.Value = var; }
        }
        else
        {
            if (GUILayout.Button("Add Point Red")) { ServerVars var = serverVars.Value; var.redScore++; serverVars.Value = var; }
            if (GUILayout.Button("Add Point Blue")) { ServerVars var = serverVars.Value; var.blueScore++; serverVars.Value = var; }
        }
    }

    static void ShowServerVars()
    {
        if (serverVars.Value.runningMatch)
        {
            GUILayout.Label("Match Ends In: " + Convert.ToDateTime(serverVars.Value.matchEndTimestamp.Subtract(DateTime.UtcNow).ToString()).ToString("m:ss"));
            GUILayout.Label("Red Score: " + serverVars.Value.redScore);
            GUILayout.Label("Blue Score: " + serverVars.Value.blueScore);
        }
        else
        {
            GUILayout.Label("Match Not Running");
        }
    }

    static void StartButtons()
    {
        if (GUILayout.Button("Host")) NetworkManager.Singleton.StartHost();
        if (GUILayout.Button("Client")) NetworkManager.Singleton.StartClient();
        if (GUILayout.Button("Server")) NetworkManager.Singleton.StartServer();
        NetworkManager.Singleton.GetComponent<UnityTransport>().ConnectionData.Address = GUILayout.TextField(NetworkManager.Singleton.GetComponent<UnityTransport>().ConnectionData.Address);
    }

    static void StatusLabels()
    {
        var mode = NetworkManager.Singleton.IsHost ?
            "Host" : NetworkManager.Singleton.IsServer ? "Server" : "Client";

        GUILayout.Label("Transport: " +
            NetworkManager.Singleton.NetworkConfig.NetworkTransport.GetType().Name);
        GUILayout.Label("Mode: " + mode);
        GUILayout.Label("FPS: " + 1 / Time.deltaTime);
    }

    static void SubmitNewPosition()
    {
        /*
        if (GUILayout.Button(NetworkManager.Singleton.IsServer ? "Move" : "Request Position Change"))
        {
            if (NetworkManager.Singleton.IsServer && !NetworkManager.Singleton.IsClient)
            {
                foreach (ulong uid in NetworkManager.Singleton.ConnectedClientsIds)
                    NetworkManager.Singleton.SpawnManager.GetPlayerNetworkObject(uid).GetComponent<HelloWorldPlayer>().Move();
            }
            else
            {
                var playerObject = NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject();
                var player = playerObject.GetComponent<HelloWorldPlayer>();
                player.Move();
            }
        }
        */
    }
}
public struct ServerVars
{
    public readonly bool runningMatch => /*matchEndTimestamp == null ? false :*/ matchEndTimestamp > DateTime.UtcNow;
    public DateTime/*?*/ matchEndTimestamp;
    public int/*?*/ redScore;
    public int/*?*/ blueScore;
}
