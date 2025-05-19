using System;
using System.Collections.Generic;
using System.Linq;
using Arixen.ScriptSmith;
using UnityEngine;
using UnityEngine.Serialization;

public class PlatformManager : MonoBehaviour
{
    [SerializeField] private PlatformController[] _platforms;

    private void Awake()
    {
        _platforms = GetComponentsInChildren<PlatformController>();
        EventBusService.Subscribe<PlatformCreateEvent>(PlatformCreation);
    }

    private void PlatformCreation(PlatformCreateEvent e)
    {
        PlatformController platform = default;
        foreach (var platformController in _platforms)
        {
            if (!platformController.gameObject.activeSelf)
            {
                platform = platformController;
                break;
            }
        }
        if (platform == default) return;

        platform.gameObject.SetActive(true);
        //As we are disabling the platform when it's back of player by 1 multiple of platform size. We need it 2 multiples forward
        platform.transform.position += Vector3.forward * (_platforms.Length-1) * (platform.platformSize);
        
    }


    private void Start()
    {
        for (int i = 0; i < _platforms.Length; i++)
        {
            var platform = _platforms[i];
            platform.transform.position = Vector3.forward * i * platform.platformSize;
        }
    }
}
