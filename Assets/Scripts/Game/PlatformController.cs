using System;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    public float platformSize;
    public float moveSpeed;

    private void Awake()
    {
        transform.localScale = Vector3.one + Vector3.forward * (platformSize-1);
    }

    private void Update()
    {
        transform.position -= Vector3.forward * (moveSpeed * Time.deltaTime);
        if (transform.position.z < -1f * platformSize)
        {
            DisablePlatform();
        }
    }

    private void DisablePlatform()
    {
        gameObject.SetActive(false);
    }
}
