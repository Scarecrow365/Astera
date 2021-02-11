using TMPro;
using UnityEngine;

public class LevelInfoWindow : MonoBehaviour, IWindow
{
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI levelInfoText;

    public void ChangeState(State state)
    {
        gameObject.SetActive(state == State.NextLevel);
    }

    public void SetTextLevel(int level)
    {
        levelText.text = $"Level: {level}";
    }

    public void SetDataLevelText(int asteroidsCount, int childrenCount)
    {
        levelInfoText.text = $"Asteroids: {asteroidsCount}  Children: {childrenCount}";
    }
}