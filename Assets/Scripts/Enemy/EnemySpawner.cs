using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject[] enemiesToSpawn;

    [SerializeField] private float spawnSpeed;
    [SerializeField] private int spawnAmount;
    [SerializeField] private int spawnWaves;    //生成敌人波数
    public Transform CentralTransform;
    private Vector3 centralPos;

    private float defaultSizeX = 6f;
    private float defaultSizeY = 3f;

    private float mapSizeX; //设置生成区域
    private float mapSizeY;
    private int waves;

    [SerializeField] private float enemyMapSize;//设置地图比率
    

    private void Start()
    {
        SetMapSize();
        StartCoroutine(SpawnEnemies());
        centralPos = CentralTransform.position;
        waves = 0;
    }

    private IEnumerator SpawnEnemies()
    {
        while (waves < spawnWaves)
        {
            yield return new WaitForSeconds(spawnSpeed);    //设置生成间隔
            for (int i = 0; i < spawnAmount; i++)
            {
                int randEnemy = Random.Range(0, enemiesToSpawn.Length);
                GameObject enemy = Instantiate(enemiesToSpawn[randEnemy],
                    new Vector3(centralPos.x + Random.Range(mapSizeX, -mapSizeX),
                        centralPos.y + Random.Range(mapSizeY, -mapSizeY), 0),
                    Quaternion.identity);
            }
            waves++;
        }
    }
    private void SetMapSize()
    {
        mapSizeX = defaultSizeX * enemyMapSize;
        mapSizeY = defaultSizeY * enemyMapSize;
    }

    public bool IfIsOver()
    {
        return waves == spawnWaves;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(new Vector3(centralPos.x + mapSizeX, centralPos.y + mapSizeY, 0),
            new Vector3(centralPos.x + mapSizeX, centralPos.y - mapSizeY, 0));       
        Gizmos.DrawLine(new Vector3(centralPos.x + mapSizeX, centralPos.y + mapSizeY, 0),
            new Vector3(centralPos.x - mapSizeX, centralPos.y + mapSizeY, 0));       
        Gizmos.DrawLine(new Vector3(centralPos.x - mapSizeX, centralPos.y + mapSizeY, 0),
            new Vector3(centralPos.x - mapSizeX, centralPos.y - mapSizeY, 0));      
        Gizmos.DrawLine(new Vector3(centralPos.x - mapSizeX, centralPos.y - mapSizeY, 0),
            new Vector3(centralPos.x + mapSizeX, centralPos.y - mapSizeY, 0));
    }
}
