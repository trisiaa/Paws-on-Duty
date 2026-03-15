using UnityEngine;

public class DropArea : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private string requiredItemID; // The ID of the item this slot accepts
    [SerializeField] private GameObject visualReplacementPrefab; 
    [SerializeField] private Transform spawnPoint; 
    
    [Header("UI Feedback")]
    [SerializeField] private GameObject checkmarkUI;

    private bool isOccupied = false;

    private void Start()
    {
        if (checkmarkUI != null) checkmarkUI.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isOccupied) return; // Prevent multiple items in one slot

        Item item = other.GetComponent<Item>();
        
        // Check if it's the CORRECT item based on ID
        if (item != null && item.itemID == requiredItemID)
        {
            isOccupied = true;
            Debug.Log($"Correct item '{item.itemID}' secured!");

            // 1. Destroy the dropped item
            Destroy(other.gameObject);

            // 2. Spawn the static replacement
            if (visualReplacementPrefab != null)
            {
                Vector3 pos = spawnPoint != null ? spawnPoint.position : transform.position;
                Quaternion rot = spawnPoint != null ? spawnPoint.rotation : Quaternion.identity;
                Instantiate(visualReplacementPrefab, pos, rot);
            }

            // 3. Feedback
            if (checkmarkUI != null) checkmarkUI.SetActive(true);
        }
    }
}