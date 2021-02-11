using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelKeeper : MonoBehaviour
{
    private int _allAsteroidsOnLevel;
    private const int MaxCountOnLevel = 6;
    private const int SpawnChildrenAfterLevel = 0;
    public int GetAsteroidsOnLevel { get; private set; }
    public int GetChildrenOnLevel { get; private set; }
    public int GetCurrentScore { get; private set; }
    public int GetCurrentLevel { get; private set; }

    public event Action<int> OnChangeScore;
    public event Action OnLevelEnd;

    public void Init()
    {
        GetCurrentLevel = 1;
        GetCurrentScore = 0;
        GetAsteroidsOnLevel = 2;
        GetChildrenOnLevel = 0;
        _allAsteroidsOnLevel = GetAsteroidsOnLevel + GetChildrenOnLevel;
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

    public void AddScore(int score)
    {
        GetCurrentScore += score;
        ReduceAsteroidsCount();
        OnChangeScore?.Invoke(GetCurrentScore);
    }

    public void RestartLevel()
    {
        StartCoroutine(DelayRestartGame());
    }

    private IEnumerator DelayRestartGame()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(0);
    }
}