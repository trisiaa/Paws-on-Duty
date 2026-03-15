using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] 
    private LayerMask pickableLayerMask;

    [SerializeField]
    private Transform playerCameraTransform;

    [SerializeField]
    private GameObject pickUpUI;

    [SerializeField]
    [Min(1)]
    private float hitRange = 3;

    [SerializeField]
    private Transform pickUpParent;

    [SerializeField]
    private GameObject inHandItem;
    
    [SerializeField]
    private InputActionReference interactionInput, dropInput;

    private RaycastHit hit; 

    private void Start()
    {
        interactionInput.action.performed += Interact;
        dropInput.action. performed += Drop;
    }

    private void Drop(InputAction.CallbackContext obj)
    {
        if (inHandItem != null)
        {
            Rigidbody rb = inHandItem.GetComponent<Rigidbody>(); // Use the item reference, not the raycast hit
            if (rb != null)
            {
                rb.isKinematic = false;
            }

            inHandItem.transform.SetParent(null);
            inHandItem = null;
        }
    }

    private void Interact(InputAction.CallbackContext obj)
    {
        if(hit.collider != null && inHandItem == null)
        {
            if (hit.collider.GetComponent<Item>())
            {
                inHandItem = hit.collider.gameObject;

                // 1. Set the Parent FIRST
                // Passing 'false' for worldPositionStays is a cleaner way to reset coordinates
                inHandItem.transform.SetParent(pickUpParent, false); 

                // 2. Explicitly reset (Optional if using SetParent(parent, false))
                inHandItem.transform.localPosition = Vector3.zero;
                inHandItem.transform.localRotation = Quaternion.identity;

                // Handle Rigidbody
                Rigidbody rb = inHandItem.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.isKinematic = true;
                }
            }
        }
    }

    // Update is called once per frame
    private void Update()
    {
        Debug.DrawRay(playerCameraTransform.position, playerCameraTransform.forward * hitRange, Color.red); 
        if (hit.collider != null)
        {
            hit.collider.GetComponent<Highlight>()?.ToggleHighlight(false);
            pickUpUI.SetActive(false);
        }

        if (inHandItem != null)
        {
            return;
        }

        if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out hit, hitRange, pickableLayerMask))
        {
            hit.collider.GetComponent<Highlight>()?.ToggleHighlight(true);
            pickUpUI.SetActive(true);
        }
    }
}
