using UnityEngine;
using UnityEngine.InputSystem;

public class CustomPlayerInput : MonoBehaviour
{
    [SerializeField]
    private InputActionReference m_movementRef;

    // Update is called once per frame
    void Update()
    {
        Vector2 movementInput = 
            m_movementRef.action
            .ReadValue<Vector2>();

        transform.position +=
            new Vector3(
                movementInput.y,
                0f,
                movementInput.x)
            * Time.deltaTime;
    }
}
