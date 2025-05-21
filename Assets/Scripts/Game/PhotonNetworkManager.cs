using System;
using System.Collections.Generic;
using Arixen.ScriptSmith;
using Fusion;
using Fusion.Sockets;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PhotonNetworkManager : MonoGenericLazySingleton<PhotonNetworkManager>,INetworkRunnerCallbacks
{
    public NetworkRunner Runner { get;private set;}
    
    [SerializeField] private PlayerController playerPrefab;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartGame(GameMode.AutoHostOrClient);
    }
    

    async void StartGame(GameMode mode)
    {
        // Create the Fusion runner and let it know that we will be providing user input
        Runner = gameObject.AddComponent<NetworkRunner>();
        Runner.ProvideInput = true;

        // Create the NetworkSceneInfo from the current scene
        var scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex);
        var sceneInfo = new NetworkSceneInfo();
        if (scene.IsValid) {
            sceneInfo.AddSceneRef(scene, LoadSceneMode.Additive);
        }

        // Start or join (depends on gamemode) a session with a specific name
        await Runner.StartGame(new StartGameArgs()
        {
            GameMode = mode,
            SessionName = "TestRoom",
            Scene = scene,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
        });
        EventBusService.InvokeEvent(new GameStartEvent());
    }
    #region Network callbacks

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        this.Debug("OnPlayerJoined");

        var networkPlayer = runner.Spawn(playerPrefab, Vector3.zero, Quaternion.identity, player);
        ViewsManager.Instance.OnPlayerJoined(networkPlayer);
        networkPlayer.UpdatePlayerPositionX(player.PlayerId);
        networkPlayer.IsLocalPlayer = player == runner.LocalPlayer;
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        this.Debug("OnPlayerLeft");
    }
    

    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
        this.Debug("OnObjectExitAOI");
    }

    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
        this.Debug("OnObjectEnterAOI");
    }


    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
        this.Debug("OnShutdown reason: " + shutdownReason);
    }

    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {
        this.Debug("OnDisconnectedFromServer reason: " + reason);
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
        this.Debug("onConnectRequest");
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
        this.Debug("OnConnectFailed reason: " + reason);
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
        this.Debug("OnUserSimulationMessage message: " + message);
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
    {
        this.Debug("OnReliableDataReceived");
    }

    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
    {
        this.Debug("OnReliableDataProgress");
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        this.Debug("OnInput input: " + input.ToString());
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
        this.Debug("OnInputMissing");
        
    }

    public void OnConnectedToServer(NetworkRunner runner)
    {
        this.Debug("OnConnectedToServer");
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        this.Debug("OnSessionListUpdated");
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
        this.Debug("OnCustomAuthenticationResponse");
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
        this.Debug("OnHostMigration");
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
        this.Debug("OnSceneLoadDone");
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
        this.Debug("OnSceneLoadStart");
    }
    #endregion

}
