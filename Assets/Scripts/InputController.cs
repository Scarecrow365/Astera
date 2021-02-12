using System;
using UnityEngine;

public class InputController
{
    public event Action OnJumpPressed;
    public event Action OnFirePressed;

    public void UpdateBehaviour()
    {
        if (Input.GetButtonDown("Jump"))
        {
            OnJumpPressed?.Invoke();
        }

        if (Input.GetButtonDown("Fire1"))
        {
            OnFirePressed?.Invoke();   
        }
    }
}