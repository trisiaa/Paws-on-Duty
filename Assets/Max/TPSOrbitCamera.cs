using UnityEngine;

public class TPSOrbitCamera : MonoBehaviour
{
    [Header("Target")]
    [Tooltip("Tarik GameObject karakter/objek Anda ke sini")]
    public Transform target;

    [Header("Pengaturan Jarak (Offset)")]
    [Tooltip("Jarak kamera dari objek (depan-belakang)")]
    public float distance = 5f;
    [Tooltip("Tinggi kamera dari objek (naik-turun)")]
    public float height = 2f;

    [Header("Pengaturan Pandangan (Look At)")]
    [Tooltip("Tinggi titik fokus pandangan pada sumbu Y lokal objek (Misal: melihat ke arah kepala/pundak, bukan kaki)")]
    public float lookAtHeight = 1.5f;

    [Header("Pengaturan Kontrol Mouse")]
    [Tooltip("Kecepatan putaran kamera saat mouse digerakkan ke kiri/kanan")]
    public float mouseSensitivity = 5f;

    // Menyimpan rotasi sudut horizontal saat ini
    private float currentYRotation = 0f;

    void Start()
    {
        // (Opsional tapi disarankan) Mengunci kursor mouse di tengah layar dan menyembunyikannya 
        // agar mouse tidak keluar dari jendela game saat sedang memutar kamera
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Menggunakan LateUpdate agar kamera bergerak setelah objek target selesai bergerak di FixedUpdate/Update
    void LateUpdate()
    {
        if (target == null) return;

        // 1. Ambil input gerakan mouse pada sumbu X (Kiri/Kanan)
        float mouseX = Input.GetAxis("Mouse X");

        // Tambahkan ke rotasi saat ini
        currentYRotation += mouseX * mouseSensitivity;

        // 2. Kalkulasi putaran orbit (hanya memutar pada sumbu Y dunia)
        Quaternion orbitRotation = Quaternion.Euler(0f, currentYRotation, 0f);

        // 3. Kalkulasi Offset Posisi
        // Kita gabungkan tinggi (Y) dan jarak mundur (-Z)
        Vector3 positionOffset = new Vector3(0f, height, -distance);

        // 4. Terapkan posisi baru ke kamera
        // Posisi target + (rotasi orbit * posisi offset) akan membuat kamera selalu menjaga jarak tapi bisa berputar
        transform.position = target.position + (orbitRotation * positionOffset);

        // 5. Atur arah pandangan (Look At) ke Y-axis lokal dari target
        // Kita menggunakan target.up (Y lokal) agar jika objek menanjak, pandangan tetap menyesuaikan
        Vector3 lookAtPoint = target.position + (target.up * lookAtHeight);
        transform.LookAt(lookAtPoint);
    }
}