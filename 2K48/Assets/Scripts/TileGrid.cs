using UnityEngine;

public class TileGrid : MonoBehaviour
{
    public TileRow[] rows { get; private set; }
    public TileCell[] cells { get; private set; }

    public int Size => cells.Length;
    public int Height => rows.Length;
    public int Width => Size / Height;

    private void Awake()
    {
        rows = GetComponentsInChildren<TileRow>();
        cells = GetComponentsInChildren<TileCell>();

        for (int i = 0; i < cells.Length; i++) {
            cells[i].coordinates = new Vector2Int(i % Width, i / Width);
        }
    }

    public void Start()
    {
        for (int y = 0; y < rows.Length; y++)
        {
            for (int x = 0; x < rows [y].cells.Length; x++)
            {
                rows[y].cells[x].coordinates = new Vector2Int(x, y);
            }
        }
    }

    public TileCell GetCell(int x, int y)
    {
        if (x >= 0 && x < Width && y >= 0 && y < Height)
        {
            return rows[y].cells[x];
        }
        else
        {
            return null;
        }
    }

    public TileCell GetCell(Vector2Int coordinates)
    {
        return GetCell(coordinates.x, coordinates.y);
    }

    public TileCell GetAdjecentCell(TileCell cell, Vector2Int direction)
    {
        Vector2Int coordinates = cell.coordinates;
        coordinates.x += direction.x;
        coordinates.y -= direction.y;

        return GetCell(coordinates);
    }

    public TileCell GetRandomEmptyCell()
    {
        int index = Random.Range(0, cells.Length);
        int startingIndex = index;

    while (cells[index].Occupied) //error occopied //fixed to Occupied
    {
        index++;

        if (index >= cells.Length){
            index = 0;
        }
        if (index == startingIndex){
            return null;
        }
    }
        return cells[index];
    }

}
