using System;
using System.Collections.Generic;
using Arixen.ScriptSmith;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class ObstaclesHandler : MonoBehaviour
{
    //public List<CylinderPoolable> _cylinderPoolables;

    [SerializeField] private CylinderObstacle[] _cylinderObstacles;

    private int randomSeed = 9879;

    private void Awake()
    {
        _cylinderObstacles = GetComponentsInChildren<CylinderObstacle>();
    }

    private void Start()
    {
        Random.InitState(randomSeed);
    }

    private void OnEnable()
    {
        ObstaclesSetup();
        
    }

    private void OnDisable()
    {
        foreach (var obstacle in _cylinderObstacles)
        {
            obstacle.gameObject.SetActive(false);
        }
    }
    public void ObstaclesSetup()
    {
        foreach (var obstacle in _cylinderObstacles)
        {
            obstacle.gameObject.SetActive(true);
            Vector3 pos = new Vector3((int)Random.Range(-5f, 5f), obstacle.transform.position.y,  (int)Random.Range(-5f, 5f));
            obstacle.transform.position = pos;
        }

    }
}