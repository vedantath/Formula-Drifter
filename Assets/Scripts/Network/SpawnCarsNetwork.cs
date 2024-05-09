using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Fusion.Sockets;
using System;


public class SpawnCarsNetwork : MonoBehaviour, INetworkRunnerCallbacks
{
    public NetworkPlayer playerPrefab;

    CarInputHandler localCarInputHandler;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    Vector3 GetRandomSpawnPoint()
    {
        GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");

        if(spawnPoints.Length == 0)
            return Vector3.zero;
        else return spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)].transform.position;
    }

    public void OnConnectedToServer(NetworkRunner runner)
    {
        if(runner.Topology == Topologies.Shared)
        {
            Debug.Log("OnConnectedToServer, starting player prefab as local player");

            runner.Spawn(playerPrefab, GetRandomSpawnPoint(), Quaternion.identity, runner.LocalPlayer);
        }
        else Debug.Log("OnConnectedToServer");
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        if(runner.IsServer)
        {
            Debug.Log("OnPlayerJoined we are server. Spawning player");
            runner.Spawn(playerPrefab, GetRandomSpawnPoint(), Quaternion.identity, player);
        }
        else Debug.Log("OnPlayerJoined");
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        //Access the local car input when its ready
        if(localCarInputHandler == null && NetworkPlayer.Local != null)
        {
            localCarInputHandler = NetworkPlayer.Local.GetComponent<CarInputHandler>();
        }

        //Ensure that access to the local car input handler before accessing
        if(localCarInputHandler != null)
        {
            input.Set(localCarInputHandler.GetNetworkInput());
        }
    }



    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) { }
    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) {}
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { Debug.Log("OnShutdown"); }
    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
        throw new NotImplementedException();
    }

    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
        throw new NotImplementedException();
    }

    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {
        Debug.Log("OnDisconnectedFromServer");
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
        //throw new NotImplementedException();
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
        throw new NotImplementedException();
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
        throw new NotImplementedException();
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        throw new NotImplementedException();
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
        throw new NotImplementedException();
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
        throw new NotImplementedException();
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
    {
        throw new NotImplementedException();
    }

    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
    {
        throw new NotImplementedException();
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
        throw new NotImplementedException();
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
        throw new NotImplementedException();
    }


}