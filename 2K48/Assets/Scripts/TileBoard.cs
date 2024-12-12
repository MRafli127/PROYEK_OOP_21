using System.Collections.Generic;
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
    Tile tile = Instantiate(tilePrefabs, grid.transform);
    tile.SetState(tileStates[0]); // harusnya ada , 2 "masih bug"
    tile.Spawn(grid.GetRandomEmptyCell());
  }

}