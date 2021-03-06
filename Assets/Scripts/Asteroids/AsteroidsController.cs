using System;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidsController : MonoBehaviour
{
    private List<Asteroid> _asteroids;

    public event Action<int> OnGenerationAsteroids;

    public void Awake()
    {
        _asteroids = new List<Asteroid>();
    }

    public void StartGame(List<Asteroid> newAsteroids)
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
        if (_asteroids.Count <= 0) return;

        foreach (var asteroid in _asteroids)
        {
            if (!asteroid.IsChild)
            {
                asteroid.UpdateBehavior();
            }
        }
    }

    private void ReportDisableAsteroid(Asteroid asteroid)
    {
        OnGenerationAsteroids?.Invoke(asteroid.GenerationNumber);

        if (asteroid.ChildrenList != null)
        {
            foreach (var child in asteroid.ChildrenList)
            {
                _asteroids.Add(child);
                child.OnBreak += RemoveDisableAsteroid;
                child.OnBreak += ReportDisableAsteroid;
            }
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
                _asteroids.Remove(obj);
            }
        }
    }

    public void GameOver(State state)
    {
        if (state == State.GameOver)
        {
            foreach (var asteroid in _asteroids)
            {
                asteroid.OnBreak -= ReportDisableAsteroid;
                asteroid.OnBreak -= RemoveDisableAsteroid;
            }

            _asteroids.Clear();
        }
    }

    public void OnDestroy()
    {
        GameOver(State.GameOver);
    }
}