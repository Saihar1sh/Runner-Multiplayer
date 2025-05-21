using System;
using System.Collections.Generic;
using Arixen.ScriptSmith;
using Fusion;
using UnityEngine;
using UnityEngine.InputSystem;
using Fusion;
using Fusion.Sockets;
using UnityEngine.UI;
using TouchPhase = UnityEngine.TouchPhase;

public class PlayerController : NetworkBehaviour, INetworkRunnerCallbacks
{
    //[SerializeField] private InputAction move;
    //[SerializeField] private InputAction jump;
    [SerializeField] private Rigidbody playerRb;
    
    [SerializeField] private float jumpForce = 5f;
    
    private float moveMultiplier = 1f;
    
    public CameraHandler CameraHandler { get; private set; }
    public RawImage CameraTextureView { get;  set; }
    public bool IsLocalPlayer { get; set; }
    
    [Networked]
    private Vector3 networkPosition { get; set; }
    [Networked]
    private Quaternion networkRotation { get; set; }
    [Networked] public bool IsJumping { get; set; }
    [Networked] public bool IsAlive { get; set; }
    [Networked] public bool IsMultiplierActive { get; set; }
    
    private Vector2 touchStartPos;
    private bool isSwiping;
    private Vector3 velocity;
    private bool isGrounded;
    private const float playerMaxMoveX = 2f;
    
    private void Awake()
    {
        CameraHandler = GetComponentInChildren<CameraHandler>();
    }

    private void Start()
    {
        Runner.AddCallbacks(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (HasStateAuthority && IsLocalPlayer)
        {
            if (other.gameObject.layer == 6)        //section end trigger
            {
                //EventBusService.InvokeEvent(new PlatformCreateEvent());
                if (IsMultiplierActive) //SectionEnd 
                {
                    moveMultiplier += 1f;
                    this.Debug("Multiplier: " + moveMultiplier);
                }
                
                IsMultiplierActive = true;
            }
        }
    }
    

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        if (!Object.HasInputAuthority || !IsLocalPlayer)
            return;

        NetworkInputData data = new NetworkInputData();

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                touchStartPos = touch.position;
                isSwiping = true;
            }
            else if (touch.phase == TouchPhase.Ended && isSwiping)
            {
                Vector2 delta = touch.position - touchStartPos;
                if (Mathf.Abs(delta.x) > 50f) // Swipe threshold
                {
                    data.Horizontal = (byte)(delta.x > 0 ? 1 : -1);
                }
                else
                {
                    data.JumpPressed = playerRb.linearVelocity.y == 0; // Jump only if grounded
                }
                isSwiping = false;
            }
        }

        input.Set(data);
    }

    public override void FixedUpdateNetwork()
    {
        if (HasStateAuthority && IsAlive)
        {
            // Get input data
            if (Runner.TryGetInputForPlayer(Object.InputAuthority, out NetworkInputData input))
            {
                UpdatePlayerPositionX(input.Horizontal, moveMultiplier * Runner.DeltaTime);
                
                // Handle jump
                if (input.JumpPressed && playerRb.linearVelocity.y == 0)
                {
                    playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                    IsJumping = true;
                }
                else if (playerRb.linearVelocity.y == 0)
                {
                    IsJumping = false;
                }
                
                if ((networkPosition - transform.position).sqrMagnitude > 0.0001f)
                    networkPosition = transform.position;

                if (Quaternion.Angle(networkRotation, transform.rotation) > 0.1f)
                    networkRotation = transform.rotation;
            }
            else
            {
                InterpolateMovement();
            }

        }
    }
    
    private void InterpolateMovement()
    {
        float lerpSpeed = 10f * Runner.DeltaTime;
        transform.position = Vector3.Lerp(transform.position, networkPosition, lerpSpeed);
        transform.rotation = Quaternion.Slerp(transform.rotation, networkRotation, lerpSpeed);
    }

    public void UpdatePlayerPositionX(float positionX, float multipler =1f)
    {
        float playerX = Mathf.Clamp(positionX, -playerMaxMoveX, playerMaxMoveX);
        playerRb.MovePosition(transform.position + Vector3.right * playerX * multipler);
    }
    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
    }

    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
    }

    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
        
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
        
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
        
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
    {
        
    }

    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
    {
        
    }


    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
        
    }

    public void OnConnectedToServer(NetworkRunner runner)
    {
        
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
        
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
        
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
        
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
        
    }

    public void LostMultiplier()
    {
        moveMultiplier = 1f;
        IsMultiplierActive = false;
    }
}

public struct NetworkInputData : INetworkInput
{
    public float Horizontal;
    public bool JumpPressed;
}