using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Level : MonoBehaviour
{
    [Header("Level Settings")]
    [SerializeField] private int numberOfActiveBot = 15;
    [SerializeField] private int numberOfTotalBot = 30;
    [SerializeField] private Transform minPoint; 
    [SerializeField] private Transform maxPoint;
    [SerializeField] private Transform playerSpawn;

    public Transform GetPlayerSpawn()
    {
        return playerSpawn;
    }

    public int GetBotNumber()
    {
        return numberOfActiveBot;
    }

    public int GetAllBotNumber()
    {
        return numberOfTotalBot;
    }

    public Vector3 RandomPoint()
    {
        Vector3 randPoint = new Vector3(Random.Range(minPoint.position.x, maxPoint.position.x), minPoint.position.y, Random.Range(minPoint.position.z, maxPoint.position.z));

        return randPoint;
    }
}
