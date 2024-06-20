using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class TileMapGenerator : MonoBehaviour
{
    public GameObject player;
    public TileBase tile;
    public Tilemap tilemap;
    public float tileWidth = 10f;
    public float generationThreshold = 5f;

    private Vector3Int lastTilePosition;
    private List<Vector3Int> generatedTiles = new List<Vector3Int>();

void Start()
{
    Vector3Int playerPosition = tilemap.WorldToCell(player.transform.position); // Use player's position as origin
    GenerateTilesAroundPlayer(playerPosition); // Generate tiles around the player at the start
}

void Update()
{
    Vector3Int playerPosition = tilemap.WorldToCell(player.transform.position);
    float distanceToEdgeX = Mathf.Abs(playerPosition.x - lastTilePosition.x);
    float distanceToEdgeY = Mathf.Abs(playerPosition.y - lastTilePosition.y);

    float adjustedGenerationThresholdX = generationThreshold * 1.5f;
    float adjustedGenerationThresholdY = generationThreshold * 2f;

    // Generate tiles in the direction of movement
    if (distanceToEdgeX * tileWidth >= adjustedGenerationThresholdX || distanceToEdgeY * tileWidth >= adjustedGenerationThresholdY)
    {
        GenerateTilesAroundPlayer(playerPosition);
    }

    RemoveOutOfRangeTiles(playerPosition);
}

void GenerateTilesAroundPlayer(Vector3Int playerPosition)
{
    for (int x = -1; x <= 1; x++)
    {
        for (int y = -1; y <= 1; y++)
        {
            Vector3Int newPosition = new Vector3Int(playerPosition.x + x, playerPosition.y + y, 0);
            GenerateTileAtPosition(newPosition);
        }
    }
}


    void GenerateTileAtPosition(Vector3Int position)
    {
        if (!tilemap.HasTile(position))
        {
            tilemap.SetTile(position, tile);
            if (!generatedTiles.Contains(position))
            {
                generatedTiles.Add(position);
            }
        }
        lastTilePosition = position;
    }

    void RemoveOutOfRangeTiles(Vector3Int playerPosition)
    {
        float removalThreshold = generationThreshold + 10f;
        for (int i = generatedTiles.Count - 1; i >= 0; i--)
        {
            Vector3Int tilePosition = generatedTiles[i];
            float distanceX = Mathf.Abs(tilePosition.x - playerPosition.x) * tileWidth;
            float distanceY = Mathf.Abs(tilePosition.y - playerPosition.y) * tileWidth;
            if (distanceX > removalThreshold || distanceY > removalThreshold)
            {
                tilemap.SetTile(tilePosition, null);
                generatedTiles.RemoveAt(i);
            }
        }
    }
}