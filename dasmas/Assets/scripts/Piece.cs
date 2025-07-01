using UnityEngine;

public class Piece : MonoBehaviour
{
    public bool IsWhite;
    public bool IsKing;
    public int X { get; private set; }
    public int Y { get; private set; }

    public void SetPosition(int x, int y)
    {
        X = x;
        Y = y;
    }
}
