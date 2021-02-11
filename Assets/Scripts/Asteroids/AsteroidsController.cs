using System;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidsController : MonoBehaviour, IDisposable
{
    private const int ScoreForParent = 100;
    private const int ScoreForChild = 50;
    [SerializeField]private Transform _lastDisableAsteroidPos;
    [SerializeField]private List<Asteroid> _asteroids;

    public event Action<int> OnSetPoints;
    public event Action<int> OnRequestChildAsteroid;

    public void Init(List<Asteroid> newAsteroids)
    {
        _asteroids = newAsteroids;
        foreach (var asteroid in _asteroids)
        {
            asteroid.OnBreak += RemoveDisableAsteroid;
            asteroid.OnBreak += ReportDisableAsteroid;
        }
    }

    public void UpdateBehaviour()
    {
        if (_asteroids.Count > 0)
        {
            foreach (var asteroid in _asteroids)
            {
                asteroid.UpdateBehavior();
            }
        }
    }

    public void SetNewAsteroid(Asteroid newAsteroid)
    {
        _asteroids.Add(newAsteroid);
        newAsteroid.transform.position = _lastDisableAsteroidPos.position;
        _lastDisableAsteroidPos = null;
        newAsteroid.Init(0,true);
        newAsteroid.OnBreak += RemoveDisableAsteroid;
        newAsteroid.OnBreak += ReportDisableAsteroid;
    }

    private void ReportDisableAsteroid(Asteroid asteroid)
    {
        OnSetPoints?.Invoke(asteroid.IsChild ? ScoreForChild : ScoreForParent);

        if (asteroid.ChildrenCount > 0)
        {
            OnRequestChildAsteroid?.Invoke(asteroid.ChildrenCount);
        }

        asteroid.OnBreak -= ReportDisableAsteroid;
    }

    private void RemoveDisableAsteroid(Asteroid asteroid)
    {
        for (var index = 0; index < _asteroids.Count; index++)
        {
            var obj = _asteroids[index];
            if (obj == asteroid)
            {
                obj.OnBreak -= RemoveDisableAsteroid;
                obj.OnBreak -= ReportDisableAsteroid;
                _lastDisableAsteroidPos = asteroid.transform;
                _asteroids.Remove(obj);
            }
        }
    }

    public void Dispose()
    {
        foreach (var asteroid in _asteroids)
        {
            asteroid.OnBreak -= ReportDisableAsteroid;
            asteroid.OnBreak -= RemoveDisableAsteroid;
        }
    }
}