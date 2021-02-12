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

    public List<Asteroid> InitAndGetListAsteroids(int countAsteroids, int childrenAsteroidCount)
    {
        var childrenCount = childrenAsteroidCount;
        var list = new List<Asteroid>();

        for (int i = 0; i < countAsteroids; i++)
        {
            var asteroid = GetAsteroid();
            asteroid.transform.SetRandomPosition(_xBorder, _yBorder);
            asteroid.transform.rotation = Quaternion.identity;
            asteroid.Init(false, transform);
            list.Add(asteroid);
        }

        CheckOnSetChildren(list, childrenCount);

        return list;
    }

    private Asteroid GetAsteroid() => GetAsteroidFromPool(GetRandomAsteroidType());
    private PoolObject GetRandomAsteroidType() => (PoolObject) Random.Range(0, (int) PoolObject.AsteroidC);
    private Asteroid GetAsteroidFromPool(PoolObject asteroid) => _pool.SpawnFromPool(asteroid).GetComponent<Asteroid>();
    
    private void CheckOnSetChildren(List<Asteroid> list, int childrenCount)
    {
        foreach (var parentAsteroid in list)
        {
            if (childrenCount < 1)
            {
                break;
            }

            if (childrenCount > 0 && childrenCount < 3)
            {
                SetChildrenInAsteroid(parentAsteroid);
                childrenCount--;
            }
            else if (childrenCount > 2 && childrenCount < 5)
            {
                var parent = SetChildrenInAsteroid(parentAsteroid);
                SetChildrenInAsteroid(parent);
                childrenCount -= 2;
            }
            else
            {
                var parent = SetChildrenInAsteroid(parentAsteroid);
                SetChildrenInAsteroid(parent);
                SetChildrenInAsteroid(parent);
                childrenCount -= 3;
            }
        }
    }

    private Asteroid SetChildrenInAsteroid(Asteroid asteroid)
    {
        var asteroidChildren = GetAsteroid();
        asteroidChildren.transform.SetParent(asteroid.transform);
        asteroidChildren.transform.position = asteroid.transform.position;
        asteroidChildren.transform.rotation = Random.rotation;
        asteroidChildren.Init(true, transform);
        return asteroidChildren;
    }

    public GameObject GetShip(ShipBody shipBody, ShipTower shipTower)
    {
        var ship = Instantiate(shipData.ShipPrefab(), Vector3.zero, Quaternion.identity);
        shipData.SetShip(ref ship, (int) shipBody, (int) shipTower);
        return ship;
    }
}