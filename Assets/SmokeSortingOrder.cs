using UnityEngine;

public class SmokeSortingOrder : MonoBehaviour
{
    private PlayerMainManger playerMainManager;
    private SpriteRenderer smokeRenderer; // Assign this in the inspector

    void Start()
    {
        playerMainManager = FindObjectOfType<PlayerMainManger>();
        smokeRenderer = GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        Vector3 playerPosition = playerMainManager.YoungOmarTransform.position;

        if (playerPosition.y > transform.position.y)
        {
           
            smokeRenderer.sortingOrder = smokeRenderer.sortingOrder - 1;
        }
        else
        {
          
            smokeRenderer.sortingOrder = smokeRenderer.sortingOrder + 1;
        }
    }
}