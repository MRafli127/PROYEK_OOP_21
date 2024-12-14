using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TileBoard : MonoBehaviour
{
    public Tile tilePrefabs;
    public TileState[] tileStates;
    private TileGrid grid;
    private List<Tile> tiles;

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
    Tile tile = Instantiate(tilePrefabs, grid.transform);// ada yang salah ini
    tile.SetState(tileStates[0], 2); // harusnya ada , 2 "masih bug"
    tile.Spawn(grid.GetRandomEmptyCell());
    tiles.Add(tile);
  }

  private void Update()
  {
    if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) 
    {

    }else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) 
    {

    }else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) 
    {

    }else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) 
    {

    }
  }
  
  private void MoveTiles()
  {
    
  }
}