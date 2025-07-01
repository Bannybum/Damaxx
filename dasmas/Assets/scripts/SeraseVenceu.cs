using UnityEngine;

public class SeraseVenceu : MonoBehaviour
{
    public BoardManager boardManager;
    private bool gameOver = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (gameOver) return;

        int whiteCount = 0;
        int blackCount = 0;

        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                var piece = boardManager.GetPieceAt(x, y);
                if (piece != null)
                {
                    if (piece.IsWhite)
                        whiteCount++;
                    else
                        blackCount++;
                }
            }
        }

        if (whiteCount == 0)
        {
            UIManager.Instance.ShowWinScreen(1); // jogador preto venceu
            gameOver = true;
        }
        else if (blackCount == 0)
        {
            UIManager.Instance.ShowWinScreen(0); // jogador branco venceu
            gameOver = true;
        }
    }
}
