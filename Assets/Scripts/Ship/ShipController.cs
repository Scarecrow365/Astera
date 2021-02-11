using System;
using UnityEngine;

public class ShipController
{
    private Ship _ship;

    public event Action<int> OnJumpCountChange; 
    public event Action OnShipDie; 

    public void Init(GameObject newShip)
    {
        _ship = newShip.GetComponent<Ship>();
        _ship.OnJumpCountChange += i => OnJumpCountChange?.Invoke(i);
        _ship.OnShipDie += () => OnShipDie?.Invoke();
    }

    public void UpdateBehavior()
    {
        _ship.CheckBorder();
        _ship.UpdateMousePos();
    }

    public void ResetPosition()
    {
        _ship.transform.position = Vector3.zero;
    }

    public void Jump()
    {
        _ship.Jump();
    }
}