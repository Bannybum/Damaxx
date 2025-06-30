using UnityEngine;

public class TabuleiroManager : MonoBehaviour
{
    
    public GameObject pecaPretaPrefab;
    public GameObject pecaBrancaPrefab;
    public Transform fundoDoTabuleiro;
    
    private float tamanhoCasa = 1.0f; // tamanho de cada casa do tabuleiro
    private int linhas = 8;
    private int colunas = 8;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CriarTabuleiro();
    }
    
    void CriarTabuleiro()
    {
        for (int y = 0; y < linhas; y++)
        {
            for (int x = 0; x < colunas; x++)
            {
                // Casas pretas ficam em posições onde (x + y) é ímpar
                if ((x + y) % 2 != 0)
                {
                    Vector2 posicao = new Vector2(x * tamanhoCasa, y * tamanhoCasa);
                    posicao -= new Vector2(3.5f, 3.5f); // centralizar

                    if (y < 3)
                    {
                        // Colocar peças pretas
                        Instantiate(pecaPretaPrefab, posicao, Quaternion.identity, fundoDoTabuleiro);
                    }
                    else if (y > 4)
                    {
                        // Colocar peças brancas
                        Instantiate(pecaBrancaPrefab, posicao, Quaternion.identity, fundoDoTabuleiro);
                    }
                }
            }
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
