using System.Collections.Generic;
using Levels;
using UnityEngine;
using UnityEngine.Tilemaps;
using Combat;

public class MapChanger : MonoBehaviour
{
    public Material[] mapMats; // Reference to the Tilemap component
    [SerializeField] private List<Renderer> mapRenderers;
    
    public Level level = Level.Instance;

    [SerializeField] private LevelColor _currentColor;

    // Update the level's color based on the current level color
    // Subscribe to OnLevelColorChanged event
    public void UpdateCurrentColor(LevelColor newColor)
    {
        _currentColor = newColor;
        foreach (var mapRenderer in mapRenderers)
        {
            if (mapRenderer == null)
            {
                continue;
            }
            
            UpdateCurrentColorGivenMapRenderer(mapRenderer);
        }
    }
    
    void Start()
    {
        foreach (var mapRenderer in mapRenderers)
        {
            if (mapRenderer == null)
            {
                Debug.LogError("Map renderer not assigned in the inspector.");
            }
        }
        
        if (mapMats == null || mapMats.Length == 0)
        {
            Debug.LogError("Map material not assigned in the inspector.");
        }
    }
    
    private void UpdateCurrentColorGivenMapRenderer(Renderer mapRenderer)
    {
        // Switch the map color based on the new color
        switch (_currentColor)
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
            case LevelColor.None:
            default:
                Debug.LogError("Invalid color selected.");
                break;
        }
    }
}
