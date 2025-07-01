using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Turno")]
    public TextMeshProUGUI turnText;

    [Header("Tela de Vit�ria")]
    public GameObject winPanel;
    public TextMeshProUGUI winText;

    private void Awake()
    {
        // Singleton para acesso f�cil de qualquer lugar
        if (Instance == null)
            Instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateTurnText(0); // Come�a com o jogador branco
        winPanel.SetActive(false);
    }

    public void UpdateTurnText(int player)
    {
        if (player == 0)
            turnText.text = "Vez do Jogador Branco";
        else
            turnText.text = "Vez do Jogador Preto";
    }

    public void ShowWinScreen(int winner)
    {
        winPanel.SetActive(true);
        if (winner == 0)
            winText.text = "Jogador Branco venceu!";
        else
            winText.text = "Jogador Preto venceu!";
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
