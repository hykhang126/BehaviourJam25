using Levels;
using UnityEngine;
using UnityEngine.Tilemaps;
using Combat;

public class MapChanger : MonoBehaviour
{
    public Tilemap tilemap; // Reference to the Tilemap component

    public Level level = Level.Instance;

    [SerializeField] private LevelColor _currentColor;

    // Update the level's color based on the current level color
    // Subscribe to OnLevelColorChanged event
    public void UpdateCurrentColor(LevelColor newColor)
    {
        _currentColor = newColor;
        // Switch the map color based on the new color
        switch (newColor)
        {
            case LevelColor.Red:
                tilemap.color = Color.red;
                break;
            case LevelColor.Green:
                tilemap.color = Color.green;
                break;
            case LevelColor.Blue:
                tilemap.color = Color.blue;
                break;
            case LevelColor.Yellow:
                tilemap.color = Color.yellow;
                break;
            case LevelColor.Black:
                tilemap.color = Color.black;
                break;
            default:
                tilemap.color = Color.white; // Default color
                break;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (tilemap == null)
        {
            tilemap = GetComponent<Tilemap>(); // Get the Tilemap component if not assigned in the inspector
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
