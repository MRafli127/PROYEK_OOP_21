using UnityEngine;

//Tile Number
[CreateAssetMenu(menuName = "Tile State")]
public class TileState : ScriptableObject
{
    public int number;
    public Color backgroundColor;
    public Color textColor;
}
