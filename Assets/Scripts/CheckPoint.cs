using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CheckPoint : SingletonMonoBehaviour<CheckPoint>
{
    public List<GameObject> spawnPoints;
    public int ActiveSpawnPointIndex { get; set; }

    private GameObject activeSpawnPoint;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        SetFirstActiveSpawnPoint();
    }

    public GameObject GetActiveSpawnPoint()
    {
        return activeSpawnPoint;
    }

    private void SetFirstActiveSpawnPoint()
    {
        activeSpawnPoint = spawnPoints[ActiveSpawnPointIndex];
    }

    public void SetActiveSpawnPoint(int activeSpawnPointIndex)
    {
        if (activeSpawnPointIndex < spawnPoints.Count)
        {
            activeSpawnPoint = spawnPoints[activeSpawnPointIndex];
        }
    }
}
