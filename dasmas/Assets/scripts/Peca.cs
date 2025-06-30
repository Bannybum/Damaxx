using UnityEngine;

public class Peca : MonoBehaviour
{
    
    public bool ehBranca;
    public GameManager gameManager;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //gameManager = FindObjectOfType<GameManager>();
    }

    void OnMouseDown()
    {
        // Só seleciona a peça se for a vez dela
        if ((ehBranca && gameManager.turnoBranco) || (!ehBranca && !gameManager.turnoBranco))
        {
            gameManager.SelecionarPeca(this);
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
