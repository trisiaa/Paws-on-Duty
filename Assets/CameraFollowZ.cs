using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target & Jarak")]
    [Tooltip("Tarik GameObject mobil ke kolom ini")]
    public Transform target;

    [Tooltip("Posisi kamera relatif terhadap mobil. Jika mobil menghadap -Z, Z harus POSITIF (contoh: Z=7) agar kamera berada di belakang.")]
    public Vector3 offset = new Vector3(0f, 3f, 7f);

    [Header("Pengaturan Panning & Orientasi")]
    [Tooltip("Sudut kemiringan kamera ke atas/bawah (Pitch). Nilai positif untuk menunduk.")]
    public float pitchOffset = 10f;

    [Tooltip("Putaran kamera ke kiri/kanan (Yaw). Isi 180 untuk memutar balik kamera jika menghadap ke belakang.")]
    public float yawOffset = 180f;

    [Header("Pengaturan Kehalusan (Smoothness)")]
    [Tooltip("Seberapa cepat kamera menyusul posisi mobil")]
    public float positionSmoothness = 10f;
    [Tooltip("Seberapa cepat kamera menyalin rotasi mobil")]
    public float rotationSmoothness = 10f;

    void LateUpdate()
    {
        if (target != null)
        {
            // 1. Pindahkan posisi kamera dengan halus (Copy Posisi + Offset)
            Vector3 targetPosition = target.TransformPoint(offset);
            transform.position = Vector3.Lerp(transform.position, targetPosition, positionSmoothness * Time.deltaTime);

            // 2. Kalkulasi Panning & Orientasi
            // Ambil rotasi dasar dari target
            Quaternion baseRotation = target.rotation;

            // Buat rotasi lokal baru untuk sumbu X (Atas/Bawah) dan sumbu Y (Putar Balik 180 derajat)
            Quaternion localRotation = Quaternion.Euler(pitchOffset, yawOffset, 0f);

            // Gabungkan kedua rotasi dengan cara mengalikannya
            Quaternion targetRotation = baseRotation * localRotation;

            // 3. Terapkan rotasi akhir ke kamera dengan halus
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSmoothness * Time.deltaTime);
        }
    }
}