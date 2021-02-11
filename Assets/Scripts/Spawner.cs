using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private ShipData shipData;
    [SerializeField] private PoolConfig poolConfig;

    private float _xBorder;
    private float _yBorder;
    private const float Offset = 0.5f;
    private ObjectPooler _pool;

    public void Init()
    {
        var mainCamera = Camera.main;
        _xBorder = mainCamera.orthographicSize * mainCamera.aspect + Offset;
        _yBorder = mainCamera.orthographicSize + Offset;

        _pool = new ObjectPooler();
        InitPool();
    }

    private void InitPool()
    {
        _pool.Init();
        foreach (var pool in poolConfig.pools)
        {
            var objects = new Queue<GameObject>();
            for (var i = 0; i < pool.size; i++)
            {
                var obj = Instantiate(pool.prefab, transform);
                obj.SetActive(false);
                objects.Enqueue(obj);
            }

            _pool.SetDataForWork(pool.poolObject, objects);
        }
    }

    public List<Asteroid> GetStartAsteroidsPack(int countAsteroids, int childrenAsteroidCount)
    {
        var childrenCount = childrenAsteroidCount;
        var list = new List<Asteroid>();
        for (int i = 0; i < countAsteroids; i++)
        {
            var poolObject = (PoolObject) Random.Range(0, (int) PoolObject.AsteroidC);
            var asteroid = GetAsteroid(poolObject);

            asteroid.transform.SetRandomPosition(_xBorder, _yBorder);
            asteroid.transform.rotation = Quaternion.identity;

            if (childrenCount > 0)
            {
                var childrenInAsteroid = Random.Range(1, 2);

                if (childrenInAsteroid > childrenCount)
                    childrenInAsteroid = childrenCount;

                childrenCount -= childrenInAsteroid;

                asteroid.Init(childrenInAsteroid, false);
            }
            else
            {
                asteroid.Init(0, false);
            }

            list.Add(asteroid);
        }

        return list;
    }

    public Asteroid GetAsteroid(PoolObject asteroid) => _pool.SpawnFromPool(asteroid).GetComponent<Asteroid>();

    public GameObject GetShip(ShipBody shipBody, ShipTower shipTower)
    {
        var ship = Instantiate(shipData.ShipPrefab(), Vector3.zero, Quaternion.identity);
        shipData.SetShip(ref ship, (int) shipBody, (int) shipTower);
        return ship;
    }
}