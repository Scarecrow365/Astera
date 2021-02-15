using System;
using System.Collections;
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
    private bool _immune;

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
    
    private Vector3 GetMousePos()
    {
        var mousePos = _camera.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        return mousePos;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (_immune)
        {
            return;
        }
        if (other.collider.CompareTag("Asteroid"))
        {
            Die();
        }
    }

    private void DeactivateImmune() => _immune = false;

    public void ActivateImmune()
    {
        _immune = true;
        Invoke("DeactivateImmune" , 0.2f);
    }

    public void Jump()
    {
        if (jumpCount > 0)
        {
            jumpCount--;
            OnJumpCountChange?.Invoke(jumpCount);
            StartCoroutine(SpeedUp());
        }
    }

    public void CheckBorder() => transform.CheckBorder();

    public void UpdateMousePos()
    {
        var mousePos = GetMousePos();
        tower.LookAt(mousePos, Vector3.back);
    }

    public void SetBulletPosition(GameObject bullet)
    {
        bullet.transform.position = tower.position; 
        bullet.transform.LookAt(GetMousePos());
    }

    private IEnumerator SpeedUp()
    {
        var curSpeed = speed;
        speed = 20;
        yield return new WaitForSeconds(0.15f);
        speed = curSpeed;
    }
}