using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Asteroid : MonoBehaviour
{
    private Rigidbody _rb;
    private const int MaxSpeed = 3;

    public int ChildrenCount { get; private set; }
    public bool IsChild { get; private set; }

    public event Action<Asteroid> OnBreak;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Player"))
        {
            gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        OnBreak?.Invoke(this);
        _rb.velocity = Vector3.zero;
    }

    private void CheckBorder() => transform.CheckBorder();
    public void UpdateBehavior() => CheckBorder();

    public void Init(int children, bool isChild)
    {
        ChildrenCount = children;
        IsChild = isChild;
        transform.localScale = isChild ? new Vector3(0.5f, 0.5f, 0.5f) : Vector3.one;
        var vector = new Vector2(Random.Range(-MaxSpeed, MaxSpeed), Random.Range(-MaxSpeed, MaxSpeed));
        _rb.AddForce(vector, ForceMode.Impulse);
    }
}