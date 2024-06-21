using System.Collections.Generic;
using UnityEngine;

public class StarGenerator : MonoBehaviour
{
    public GameObject starPrefab;
    public int areaSize = 50;
    private Vector2Int currentAreaPosition;
    private Dictionary<Vector2Int, List<GameObject>> generatedStars = new Dictionary<Vector2Int, List<GameObject>>();
    private PlayerMainManger playerMainManager;

    void Start()
    {
        playerMainManager = FindObjectOfType<PlayerMainManger>();
        currentAreaPosition = PlayerAreaPosition();
        // Generate initial star areas around the player
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                Vector2Int areaPosition = new Vector2Int(currentAreaPosition.x + x, currentAreaPosition.y + y);
                GenerateStarsInArea(areaPosition);
            }
        }
    }

    void Update()
    {
        Vector2Int playerAreaPosition = PlayerAreaPosition();
        if (currentAreaPosition != playerAreaPosition)
        {
            currentAreaPosition = playerAreaPosition;
            // Generate new star areas around the new player area
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    Vector2Int areaPosition = new Vector2Int(playerAreaPosition.x + x, playerAreaPosition.y + y);
                    if (!generatedStars.ContainsKey(areaPosition))
                    {
                        GenerateStarsInArea(areaPosition);
                    }
                }
            }
            CleanupDistantAreas(playerAreaPosition);
        }
    }

    Vector2Int PlayerAreaPosition()
    {
        return new Vector2Int(
            Mathf.FloorToInt(playerMainManager.ShipTransform.position.x / areaSize),
            Mathf.FloorToInt(playerMainManager.ShipTransform.position.y / areaSize)
        );
    }

    void GenerateStarsInArea(Vector2Int areaPosition)
    {
        if (generatedStars.ContainsKey(areaPosition)) return; // Prevent duplicate generation

        List<GameObject> starsInArea = new List<GameObject>();
        float areaStartX = areaPosition.x * areaSize - areaSize / 2;
        float areaStartY = areaPosition.y * areaSize - areaSize / 2;

        for (int i = 0; i < areaSize / 2; i++)
        {
            float x = Random.Range(areaStartX, areaStartX + areaSize);
            float y = Random.Range(areaStartY, areaStartY + areaSize);

            Vector3 starPosition = new Vector3(x, y, 0);
            GameObject star = Instantiate(starPrefab, starPosition, Quaternion.Euler(0f, 0f, Random.Range(0f, 360f)));
            float scale = Random.Range(0.02f, 0.1f);
            star.transform.localScale = new Vector3(scale, scale, 1);

            starsInArea.Add(star);
        }
        generatedStars.Add(areaPosition, starsInArea);
    }

    void CleanupDistantAreas(Vector2Int currentArea)
    {
        List<Vector2Int> areasToRemove = new List<Vector2Int>();

        foreach (var area in generatedStars.Keys)
        {
            if (Mathf.Abs(area.x - currentArea.x) > 2 || Mathf.Abs(area.y - currentArea.y) > 2)
            {
                areasToRemove.Add(area);
            }
        }

        foreach (var area in areasToRemove)
        {
            foreach (GameObject star in generatedStars[area])
            {
                Destroy(star);
            }
            generatedStars.Remove(area);
        }
    }
}