using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TPSMovement : MonoBehaviour
{
    [Header("Pengaturan Pergerakan")]
    [Tooltip("Kecepatan gerak objek")]
    public float moveSpeed = 5f;

    [Tooltip("Kecepatan objek memutar badannya")]
    public float rotationSpeed = 15f;

    [Header("Pengaturan Lompatan")]
    [Tooltip("Kekuatan lompatan (dorongan ke atas)")]
    public float jumpForce = 5f;

    [Header("Pengaturan Model 3D")]
    [Tooltip("Centang ini jika model 3D terlihat berjalan mundur (orientasi aslinya menghadap -Z)")]
    public bool invertModelOrientation = true;

    [Header("Referensi Kamera")]
    [Tooltip("Tarik Main Camera ke sini. Jika kosong, script akan mencari Main Camera otomatis.")]
    public Transform cameraTransform;

    private Rigidbody rb;
    private bool isJumpRequested = false; // Penanda bahwa tombol lompat telah ditekan

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (cameraTransform == null && Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }
    }

    // Update dipanggil setiap frame, sangat baik untuk menangkap input cepat seperti klik atau spasi
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isJumpRequested = true;
        }
    }

    // FixedUpdate dipanggil pada interval waktu yang tetap, wajib untuk kalkulasi fisika Rigidbody
    void FixedUpdate()
    {
        // --- LOGIKA PERGERAKAN ---
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;

        camForward.y = 0f;
        camRight.y = 0f;

        camForward.Normalize();
        camRight.Normalize();

        Vector3 moveDirection = (camForward * vertical + camRight * horizontal).normalized;

        if (moveDirection.magnitude >= 0.1f)
        {
            // GERAK
            rb.linearVelocity = new Vector3(moveDirection.x * moveSpeed, rb.linearVelocity.y, moveDirection.z * moveSpeed);

            // ROTASI
            Vector3 lookDirection = invertModelOrientation ? -moveDirection : moveDirection;
            Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
            rb.MoveRotation(Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime));
        }
        else
        {
            rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y, 0f);
        }

        // --- LOGIKA LOMPATAN ---
        if (isJumpRequested)
        {
            // Menambahkan force ke arah sumbu Y lokal (transform.up)
            // ForceMode.Impulse digunakan karena lompatan adalah dorongan instan, bukan tenaga berkelanjutan
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);

            // Kembalikan ke false agar tidak melompat terus-menerus
            isJumpRequested = false;
        }
    }
}