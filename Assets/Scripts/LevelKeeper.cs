using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelKeeper : MonoBehaviour
{
    private int _allAsteroidsOnLevel;
    private const int MaxCountOnLevel = 8;
    private const float RestartDelay = 3f;
    private const int SpawnChildrenAfterLevel = 3;
    private int _parentScore;
    private int _childrenScore;
    private int _grandChildrenScore;
    
    public int GetAsteroidsOnLevel { get; private set; }
    public int GetChildrenOnLevel { get; private set; }
    public int GetCurrentScore { get; private set; }
    public int GetCurrentLevel { get; private set; }

    public event Action OnLevelEnd;
    public event Action<int> OnChangeScore;

    public void Init()
    {
        GetCurrentLevel = 1;
        GetCurrentScore = 0;
        GetAsteroidsOnLevel = 2;
        GetChildrenOnLevel = 0;
        _allAsteroidsOnLevel = GetAsteroidsOnLevel + GetChildrenOnLevel;
    }

    public void UpdateDataFromServer()
    {
        _parentScore = 400;
        _childrenScore = 200;
        _grandChildrenScore = 100;
    }

    private void ReduceAsteroidsCount()
    {
        _allAsteroidsOnLevel--;
        if (_allAsteroidsOnLevel < 1)
        {
            NextLevel();
            OnLevelEnd?.Invoke();
        }
    }

    private void NextLevel()
    {
        GetCurrentLevel++;
        GetAsteroidsOnLevel++;
        GetChildrenOnLevel = GetCurrentLevel - SpawnChildrenAfterLevel;

        if (GetChildrenOnLevel < 1)
            GetChildrenOnLevel = 0;

        _allAsteroidsOnLevel = GetAsteroidsOnLevel + GetChildrenOnLevel;

        if (GetAsteroidsOnLevel > MaxCountOnLevel)
            GetAsteroidsOnLevel = MaxCountOnLevel;

        if (GetChildrenOnLevel > MaxCountOnLevel)
            GetChildrenOnLevel = MaxCountOnLevel;
    }

    public void AddScore(int generationDisabledAsteroid)
    {
        GetCurrentScore += generationDisabledAsteroid switch
        {
            2 => _childrenScore,
            3 => _grandChildrenScore,
            _ => _parentScore
        };

        ReduceAsteroidsCount();
        OnChangeScore?.Invoke(GetCurrentScore);
    }

    public void RestartLevel()
    {
        StartCoroutine(DelayRestartGame());
    }

    private IEnumerator DelayRestartGame()
    {
        yield return new WaitForSeconds(RestartDelay);
        SceneManager.LoadScene(0);
    }
}