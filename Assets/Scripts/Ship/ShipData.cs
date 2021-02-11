using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShipData", menuName = "Scriptable objects/ShipData", order = 0)]
public class ShipData : ScriptableObject
{
    [SerializeField] private GameObject shipPrefab;
    [SerializeField] private GameObject[] bodyShip;
    [SerializeField] private GameObject[] towerShip;

    public GameObject ShipPrefab() => shipPrefab;

    public void SetShip(ref GameObject ship, int bodyPartIndex, int towerPartIndex)
    {
        Instantiate(bodyShip[bodyPartIndex], ship.transform.GetChild(1));
        Instantiate(towerShip[towerPartIndex], ship.transform.GetChild(0));
    }
}
