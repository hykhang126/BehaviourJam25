using UnityEngine;
using UnityEngine.Tilemaps;

public class MapChanger : MonoBehaviour
{
    public Tilemap tilemap; // Reference to the Tilemap component

    public Level level = Level.Instance;

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
