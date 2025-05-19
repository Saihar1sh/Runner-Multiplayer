using System;
using Arixen.ScriptSmith;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private InputAction move;
    [SerializeField] private InputAction jump;
    [SerializeField] private Rigidbody playerRb;
    
    [SerializeField] private float jumpForce = 5f;

    private const float playerMaxMoveX = 2f;
    
    private void Awake()
    {
        move.performed += ctx => Move(ctx.ReadValue<Vector2>());
        jump.performed += ctx => Jump();
    }
    
    private void OnEnable()
    {
        move.Enable();
        jump.Enable();
    }

    private void OnDisable()
    {
        move.Disable();
        jump.Disable();
    }
    
    private void Move(Vector2 readValue)
    {
        float playerX = Mathf.Clamp(readValue.x, -playerMaxMoveX, playerMaxMoveX);
        playerRb.MovePosition(transform.position + Vector3.right * playerX);
        Debug.Log("value: " + readValue);
    }

    private void Jump()
    {
        playerRb.AddForce(Vector3.up* jumpForce, ForceMode.Impulse);
        Debug.Log("jump");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            EventBusService.InvokeEvent(new PlatformCreateEvent());
            
        }
    }
}
