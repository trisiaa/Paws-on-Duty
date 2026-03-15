using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("Layer Settings")]
    [SerializeField] private LayerMask pickableLayerMask;
    [SerializeField] private LayerMask dropAreaLayerMask;
    private int pickableLayer; // To store the original layer index
    private int ignoreRaycastLayer;

    [Header("References")]
    [SerializeField] private Transform playerCameraTransform;
    [SerializeField] private Transform pickUpParent;
    [SerializeField] private GameObject pickUpUI;
    [SerializeField] private GameObject dropUI; 

    [Header("Parameters")]
    [SerializeField] [Min(1)] private float hitRange = 3;

    [Header("Input Actions")]
    [SerializeField] private InputActionReference interactionInput;
    [SerializeField] private InputActionReference dropInput;

    private GameObject inHandItem;
    private RaycastHit hit;

    private void Awake()
    {
        // Cache the layer indices
        pickableLayer = LayerMask.NameToLayer("Pickable"); // Ensure your items use this name
        ignoreRaycastLayer = LayerMask.NameToLayer("Ignore Raycast");
    }

    private void OnEnable()
    {
        interactionInput.action.Enable();
        dropInput.action.Enable();
    }

    private void OnDisable()
    {
        interactionInput.action.Disable();
        dropInput.action.Disable();
    }

    private void Start()
    {
        interactionInput.action.performed += Interact;
        dropInput.action.performed += Drop;
    }

    private void Interact(InputAction.CallbackContext obj)
    {
        if (hit.collider != null && inHandItem == null)
        {
            if (((1 << hit.collider.gameObject.layer) & pickableLayerMask) != 0 && hit.collider.GetComponent<Item>())
            {
                inHandItem = hit.collider.gameObject;
                
                // CHANGE LAYER: So the raycast ignores the item in your hand
                inHandItem.layer = ignoreRaycastLayer;

                inHandItem.transform.SetParent(pickUpParent, false);
                inHandItem.transform.localPosition = Vector3.zero;
                inHandItem.transform.localRotation = Quaternion.identity;

                Rigidbody rb = inHandItem.GetComponent<Rigidbody>();
                if (rb != null) rb.isKinematic = true;
            }
        }
    }

    private void Drop(InputAction.CallbackContext obj)
    {
        if (inHandItem != null)
        {
            // RESET LAYER: So it can be picked up again later
            inHandItem.layer = pickableLayer;

            Rigidbody rb = inHandItem.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
                rb.useGravity = true;
            }

            inHandItem.transform.SetParent(null);
            inHandItem = null;
        }
    }

    private void Update()
    {
        // Reset UI/Highlights
        if (hit.collider != null)
        {
            hit.collider.GetComponent<Highlight>()?.ToggleHighlight(false);
        }
        pickUpUI.SetActive(false);
        dropUI.SetActive(false);

        // Raycast logic
        bool hasHit = Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out hit, hitRange, ~0, QueryTriggerInteraction.Collide);

        if (hasHit)
        {
            int hitLayer = hit.collider.gameObject.layer;

            // Looking at Item
            if (inHandItem == null && ((1 << hitLayer) & pickableLayerMask) != 0)
            {
                hit.collider.GetComponent<Highlight>()?.ToggleHighlight(true);
                pickUpUI.SetActive(true);
            }

            // Looking at Drop Area
            if (inHandItem != null && hit.collider.TryGetComponent<DropArea>(out DropArea area))
            {
                // Optional: Only show Drop UI if the item ID matches the area ID
                // Note: You'd need to expose 'requiredItemID' in DropArea as a public property
                dropUI.SetActive(true); 
            }
        }

        // Debug Ray (Green = Hit, Red = No Hit)
        Color rayColor = hasHit ? Color.green : Color.red;
        Debug.DrawRay(playerCameraTransform.position, playerCameraTransform.forward * hitRange, rayColor);
    }
}