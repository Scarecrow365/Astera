using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Asteroid : MonoBehaviour
{
    private Rigidbody _rb;
    private const int MaxSpeed = 3;
    private Transform _baseTransform;
    private Collider _collider;

    public bool IsChild { get; private set; }

    public int GenerationNumber { get; private set; }

    public List<Asteroid> ChildrenList { get; private set; }


    public event Action<Asteroid> OnBreak;


    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Bullet"))
        {
            CheckChildren();
            gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        OnBreak?.Invoke(this);
        _rb.velocity = Vector3.zero;
        GenerationNumber = 0;
    }

    private void CheckBorder() => transform.CheckBorder();
    public void UpdateBehavior() => CheckBorder();

    private void CheckChildren()
    {
        var children = GetComponentsInChildren<Asteroid>();

        if (children.Length > 1)
        {
            ChildrenList = new List<Asteroid>(4);
        }

        foreach (var child in children)
        {
            if (child.transform.parent != transform)
            {
                continue;
            }

            child.transform.SetParent(_baseTransform, true);
            SetChildren(child);
            ChildrenList.Add(child);
        }
    }

    private void SetChildren(Asteroid child)
    {
        child.GetComponent<Collider>().enabled = true;
        child.Init(false, _baseTransform);
    }

    public void Init(bool isChild, Transform baseTransform)
    {
        if (isChild)
        {
            _rb.isKinematic = true;
            _collider.enabled = false;
        }
        else
        {
            var vector = new Vector2(Random.Range(-MaxSpeed, MaxSpeed), Random.Range(-MaxSpeed, MaxSpeed));
            _rb.isKinematic = false;
            _rb.velocity = vector;
            _baseTransform = baseTransform;
        }

        GenerationNumber++;

        transform.localScale = GenerationNumber switch
        {
            2 => new Vector3(0.75f, 0.75f, 0.75f),
            3 => new Vector3(0.5f, 0.5f, 0.5f),
            _ => Vector3.one
        };

        IsChild = isChild;
    }
}