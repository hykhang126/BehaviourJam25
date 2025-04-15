using Levels;
using UnityEngine;
using UnityEngine.Tilemaps;
using Combat;

public class MapChanger : MonoBehaviour
{
    public Material[] mapMats; // Reference to the Tilemap component

    public Level level = Level.Instance;

    [SerializeField] private LevelColor _currentColor;

    [SerializeField] Renderer mapRenderer;

    // Update the level's color based on the current level color
    // Subscribe to OnLevelColorChanged event
    public void UpdateCurrentColor(LevelColor newColor)
    {
        _currentColor = newColor;
        // Switch the map color based on the new color
        switch (newColor)
        {
            case LevelColor.Red:
                mapRenderer.material = mapMats[0];
                break;
            case LevelColor.Blue:
                mapRenderer.material = mapMats[1];
                break;
            case LevelColor.Green:
                mapRenderer.material = mapMats[2];
                break;
            case LevelColor.Yellow:
                mapRenderer.material = mapMats[3];
                break;
            default:
                Debug.LogError("Invalid color selected.");
                break;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (mapRenderer == null)
        {
            Debug.LogError("Map renderer not assigned in the inspector.");
        }
        
        if (mapMats == null || mapMats.Length == 0)
        {
            Debug.LogError("Map material not assigned in the inspector.");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
