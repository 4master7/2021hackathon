using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public int maxCount;
    public int enemyCount;
    public float spwanTime = 3f;
    public float curTime;
    public Transform[] spanwPoints;
    public GameObject enemy;
    public static SpawnManager _instance;
    private void Start()
    {
        _instance = this;
    }
    void Update()
    {
        if (curTime >= spwanTime && enemyCount < maxCount)
        {
            int x = Random.Range(0, spanwPoints.Length);
            SpawnEnemy(x);
        }
        curTime += Time.deltaTime;
    }
    public void SpawnEnemy(int ranNum)
    {
        curTime = 0;
        enemyCount++;
        Instantiate(enemy, spanwPoints[ranNum]);
    }
}
