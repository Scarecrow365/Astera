using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PoolConfig", menuName = "Scriptable objects/PoolConfig", order = 0)]
public class PoolConfig : ScriptableObject
{
    [Serializable]
    public struct Pool
    {
        public PoolObject poolObject;
        public GameObject prefab;
        public int size;
    }
    
    public List<Pool> pools;
}
