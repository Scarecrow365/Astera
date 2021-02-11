using TMPro;
using UnityEngine;

public class MainDataWindow : MonoBehaviour, IWindow
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI jumpCountText;

    public void ChangeState(State state)
    {
        gameObject.SetActive(state == State.Game);
    }

    public void SetScoreText(int score)
    {
        scoreText.text = score.ToString();
    }

    public void SetJumpText(int jumpCount)
    {
        jumpCountText.text = $"Jumps: {jumpCount}";
    }
}