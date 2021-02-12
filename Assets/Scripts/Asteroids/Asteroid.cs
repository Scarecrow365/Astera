using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Asteroid : MonoBehaviour
{
    private Rigidbody _rb;
    private bool _isChild;
    private int _generationNumber;
    private const int MaxSpeed = 3;
    private Transform _baseTransform;
    private List<Asteroid> _childrenList;

    public bool IsChild => _isChild;
    public int GenerationNumber => _generationNumber;
    public List<Asteroid> ChildrenList => _childrenList;


    public event Action<Asteroid> OnBreak;


    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Player"))
        {
            CheckChildren();
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

    private void CheckChildren()
    {
        var children = GetComponentsInChildren<Asteroid>();

        if (children.Length > 1)
        {
            _childrenList = new List<Asteroid>(4);
        }

        foreach (var child in children)
        {
            if (child.transform.parent != transform)
            {
                continue;
            }

            child.transform.SetParent(_baseTransform, true);
            SetChildren(child);
            _childrenList.Add(child);
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
            GetComponent<Collider>().enabled = false;
        }
        else
        {
            var vector = new Vector2(Random.Range(-MaxSpeed, MaxSpeed), Random.Range(-MaxSpeed, MaxSpeed));
            _rb.isKinematic = false;
            _rb.velocity = vector;
            _baseTransform = baseTransform;
        }
        
        _generationNumber++;

        transform.localScale = _generationNumber switch
        {
            2 => new Vector3(0.75f, 0.75f, 0.75f),
            3 => new Vector3(0.5f, 0.5f, 0.75f),
            _ => Vector3.one
        };

        _isChild = isChild;
    }
}