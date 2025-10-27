using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager:MonoBehaviour
{
    [Header("Timer Setttings")]
    public float matchTime = 90f;
    private float currentTime;

    [Header("Player Carts")]
    public CartZone player1Cart;
    public CartZone player2Cart;

    [Header("UI Elements")]
    public TextMeshProUGUI timerText;
    public GameObject endPanel;
    public TextMeshProUGUI winnerText;

    private bool gameEnded = false;

    void Start()
    {
        currentTime = matchTime;
        endPanel.SetActive(false);
    }

    void Update()
    {
        if (gameEnded) return;

        currentTime-=Time.deltaTime;
        UpdateTimerUI();

        if (currentTime <= 0)
        {
            EndGame();
        }
    }

    void UpdateTimerUI()
    {
        int seconds = Mathf.CeilToInt(currentTime);
        timerText.text = seconds.ToString();
    }

    void EndGame()
    {
        gameEnded = true;
        endPanel.SetActive(true);

        if (player1Cart.itemsCollected > player2Cart.itemsCollected)
            winnerText.text = "Player 1 Wins!!!";
        else if (player2Cart.itemsCollected > player1Cart.itemsCollected)
            winnerText.text = "Player 2 Wins!";
        else
            winnerText.text = "It's a Draw!";
    }
}
