using UnityEngine;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI turnText;
    public GameObject painelVitoria;
    public TextMeshProUGUI textoVitoria;
    public bool turnoBranco = true;

    private Peca pecaSelecionada;
        
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AtualizarTextoTurno();
        painelVitoria.SetActive(false);
    }

    public void SelecionarPeca(Peca peca)
    {
        pecaSelecionada = peca;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (pecaSelecionada != null && Input.GetMouseButtonDown(0))
        {
            // Raycast no plano 2D
            Vector2 posMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(posMouse, Vector2.zero);

            // Se clicou em uma posição vazia
            if (hit.collider == null)
            {
                // Move a peça para onde clicou (snapped pra grid)
                Vector2 novaPos = new Vector2(Mathf.Round(posMouse.x), Mathf.Round(posMouse.y));
                pecaSelecionada.transform.position = novaPos;

                TrocarTurno();
                pecaSelecionada = null;
            }
        }
    }
    
    void TrocarTurno()
    {
        turnoBranco = !turnoBranco;
        AtualizarTextoTurno();
    }

    void AtualizarTextoTurno()
    {
        if (turnoBranco)
            turnText.text = "Turno: Branco";
        else
            turnText.text = "Turno: Preto";
    }

    public void MostrarVitoria(string vencedor)
    {
        painelVitoria.SetActive(true);
        textoVitoria.text = "Vitória do jogador " + vencedor + "!";
    }

    public void ReiniciarJogo()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
