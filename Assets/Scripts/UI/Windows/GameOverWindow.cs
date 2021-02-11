using TMPro;
using UnityEngine;

public class GameOverWindow : MonoBehaviour, IWindow
{
    [SerializeField] private TextMeshProUGUI statisticText;

    public void ChangeState(State state)
    {
        gameObject.SetActive(state == State.GameOver);
    }

    public void SetStatistic(int maxLevel, int score)
    {
        statisticText.text = $"Final level: {maxLevel} \nScore: {score}";
    }
}