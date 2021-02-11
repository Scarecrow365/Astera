using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Ship : MonoBehaviour
{
    [SerializeField] private Transform tower;
    [Range(1, 10)] [SerializeField] private float speed;
    [SerializeField] private int jumpCount = 3;

    public event Action<int> OnJumpCountChange;
    public event Action OnShipDie;

    private Rigidbody _rb;
    private Camera _camera;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _camera = Camera.main;
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        var posX = CrossPlatformInputManager.GetAxis("Horizontal");
        var posY = CrossPlatformInputManager.GetAxis("Vertical");

        var pos = new Vector3(posX, posY);

        if (pos.magnitude > 1)
        {
            pos.Normalize();
        }

        var movement = pos * speed;
        _rb.velocity = movement;
    }

    private void Die()
    {
        OnShipDie?.Invoke();
        transform.gameObject.SetActive(false);
    }

    // private void OnCollisionEnter(Collision other)
    // {
    //     if (other.collider.CompareTag("Asteroid"))
    //     {
    //         Die();
    //     }
    // }

    public void Jump()
    {
        if (jumpCount > 0)
        {
            jumpCount--;
            OnJumpCountChange?.Invoke(jumpCount);
            _rb.AddForce(Vector2.one * 100, ForceMode.Impulse);
        }
    }

    public void CheckBorder() => transform.CheckBorder();
    
    public void UpdateMousePos()
    {
        var mousePos = _camera.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        tower.LookAt(mousePos, Vector3.back);
    }
}