using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.U2D.Aseprite;
using UnityEngine;

public class TileBoard : MonoBehaviour
{
    public Tile tilePrefabs;
    public TileState[] tileStates;
    private TileGrid grid;
    private List<Tile> tiles;
    private bool wait;

    private void Awake()
    {
        grid = GetComponentInChildren<TileGrid>();
        tiles = new List<Tile>(16);
    }

  private void Start()
  {
        CreateTile();
        CreateTile();
  }

  private void CreateTile()
  {
    Tile tile = Instantiate(tilePrefabs, grid.transform);
    tile.SetState(tileStates[0], 2);//fixed
    tile.Spawn(grid.GetRandomEmptyCell());
    tiles.Add(tile);
  }

  private void Update()
  {
    if (!wait)
    {
      if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) 
      {
        MoveTiles(Vector2Int.up, 0, 1, 1, 1);
      }
      else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) 
      {
        MoveTiles(Vector2Int.down, 0, 1, grid.Height -2, -1);
      }
      else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) 
      {
        MoveTiles(Vector2Int.left, 1, 1, 0, 1);
      }
      else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) 
      {
        MoveTiles(Vector2Int.right, grid.Width -2, -1, 0, 1);
      }
    }
  }
  
  private void MoveTiles(Vector2Int direction, int startX, int incrementX, int startY, int incrementY)
  {
    bool changed = false;
      for ( int x =  startX; x >= 0 && x < grid.Width; x += incrementX)
      {
        for (int y = startY; y >= 0 && y < grid.Height; y += incrementY)
        {
          TileCell cell = grid.GetCell(x, y);

          if (cell.Occupied)
          {
            changed |= MoveTile(cell.tile, direction);
          }
        }
      }

      if (changed)
      {
        StartCoroutine(WaitChanges());
      }
  }

  private bool MoveTile(Tile tile, Vector2Int direction)
  {
    TileCell newCell = null;
    TileCell adjecent = grid.GetAdjecentCell(tile.cell, direction);

    while (adjecent != null)
    {
      if (adjecent.Occupied)
      {
        // todo merging
        break;
      }

      newCell = adjecent;
      adjecent = grid.GetAdjecentCell(adjecent, direction);
    }

    if (newCell != null)
    {
      tile.MoveTo(newCell);
      return true;
    }

    return false;

  }

  private IEnumerator WaitChanges()
  {
    wait = true;

    yield return new WaitForSeconds(0.1f);

    wait = false;

  }
}