using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject whitePiecePrefab;
    public GameObject blackPiecePrefab;
    public GameObject highlightPrefab;
    public Transform highlightsContainer;

    private Piece[,] pieces = new Piece[8, 8];
    private List<GameObject> highlights = new List<GameObject>();

    private const float TILE_SIZE = 1f;
    private Vector2 boardOrigin = new Vector2(-4f, -4f);

    private Piece selectedPiece;
    private List<Vector2Int> validMoves = new();
    private int currentPlayer = 0; // 0 = branco, 1 = preto

    void Start()
    {
        GeneratePieces();
    }

    void GeneratePieces()
    {
        for (int y = 0; y < 3; y++) // peças pretas
        {
            for (int x = 0; x < 8; x++)
            {
                if ((x + y) % 2 != 0)
                    SpawnPiece(blackPiecePrefab, x, y);
            }
        }

        for (int y = 5; y < 8; y++) // peças brancas
        {
            for (int x = 0; x < 8; x++)
            {
                if ((x + y) % 2 != 0)
                    SpawnPiece(whitePiecePrefab, x, y);
            }
        }
    }

    void SpawnPiece(GameObject prefab, int x, int y)
    {
        Vector3 worldPos = CoordToWorld(x, y);
        GameObject obj = Instantiate(prefab, worldPos, Quaternion.identity);
        Piece p = obj.GetComponent<Piece>();
        p.SetPosition(x, y);
        pieces[x, y] = p;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2Int coord = WorldToCoord(mouseWorld);

            if (!IsInsideBoard(coord))
                return;

            Piece clickedPiece = pieces[coord.x, coord.y];

            if (clickedPiece != null && clickedPiece.IsWhite == (currentPlayer == 0))
            {
                SelectPiece(clickedPiece);
            }
            else if (selectedPiece != null && validMoves.Contains(coord))
            {
                MoveSelectedPiece(coord.x, coord.y);
            }
        }
    }

    void SelectPiece(Piece piece)
    {
        selectedPiece = piece;
        validMoves = GetValidMoves(piece);
        ShowHighlights(validMoves);
    }

    List<Vector2Int> GetValidMoves(Piece piece)
    {
        List<Vector2Int> moves = new();
        int dir = piece.IsWhite ? -1 : 1;

        // Movimentos simples
        TryAddMove(piece.X + 1, piece.Y + dir, moves);
        TryAddMove(piece.X - 1, piece.Y + dir, moves);

        // Capturas
        TryAddCapture(piece.X, piece.Y, 1, dir, moves);
        TryAddCapture(piece.X, piece.Y, -1, dir, moves);

        return moves;
    }

    void TryAddMove(int x, int y, List<Vector2Int> moves)
    {
        if (IsInsideBoard(x, y) && pieces[x, y] == null)
            moves.Add(new Vector2Int(x, y));
    }

    void TryAddCapture(int x, int y, int dx, int dy, List<Vector2Int> moves)
    {
        int enemyX = x + dx;
        int enemyY = y + dy;
        int destX = x + 2 * dx;
        int destY = y + 2 * dy;

        if (!IsInsideBoard(destX, destY)) return;

        Piece target = pieces[enemyX, enemyY];
        if (target != null && target.IsWhite != pieces[x, y].IsWhite && pieces[destX, destY] == null)
        {
            moves.Add(new Vector2Int(destX, destY));
        }
    }

    void MoveSelectedPiece(int x, int y)
    {
        Vector2Int oldPos = new(selectedPiece.X, selectedPiece.Y);
        pieces[oldPos.x, oldPos.y] = null;

        int dx = x - oldPos.x;
        int dy = y - oldPos.y;

        // Captura
        if (Mathf.Abs(dx) == 2 && Mathf.Abs(dy) == 2)
        {
            int capturedX = oldPos.x + dx / 2;
            int capturedY = oldPos.y + dy / 2;
            Destroy(pieces[capturedX, capturedY].gameObject);
            pieces[capturedX, capturedY] = null;
        }

        selectedPiece.SetPosition(x, y);
        selectedPiece.transform.position = CoordToWorld(x, y);
        pieces[x, y] = selectedPiece;

        selectedPiece = null;
        ClearHighlights();
        currentPlayer = 1 - currentPlayer;
    }

    void ShowHighlights(List<Vector2Int> coords)
    {
        ClearHighlights();

        foreach (var coord in coords)
        {
            Vector3 pos = CoordToWorld(coord.x, coord.y);
            GameObject go = Instantiate(highlightPrefab, pos, Quaternion.identity, highlightsContainer);
            highlights.Add(go);
        }
    }

    void ClearHighlights()
    {
        foreach (var obj in highlights)
            Destroy(obj);
        highlights.Clear();
    }

    Vector2Int WorldToCoord(Vector2 worldPos)
    {
        int x = Mathf.FloorToInt((worldPos.x - boardOrigin.x) / TILE_SIZE);
        int y = Mathf.FloorToInt((worldPos.y - boardOrigin.y) / TILE_SIZE);
        return new Vector2Int(x, y);
    }

    Vector3 CoordToWorld(int x, int y)
    {
        return new Vector3(boardOrigin.x + x * TILE_SIZE + TILE_SIZE / 2f,
                           boardOrigin.y + y * TILE_SIZE + TILE_SIZE / 2f,
                           -1f);
    }

    bool IsInsideBoard(int x, int y) => x >= 0 && x < 8 && y >= 0 && y < 8;
    bool IsInsideBoard(Vector2Int v) => IsInsideBoard(v.x, v.y);

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                Vector3 center = CoordToWorld(x, y);
                Gizmos.DrawSphere(center, 0.05f);
            }
        }
    }

    public Piece GetPieceAt(int x, int y)
    {
        return pieces[x, y];
    }
#endif
}
