using UnityEngine;

public class ObjectCycler : MonoBehaviour
{
    [Header("Urutan Objek")]
    [Tooltip("Masukkan GameObject ke sini sesuai urutan munculnya (1, 2, 3...)")]
    public GameObject[] sequenceObjects;

    [Header("Hubungan Kamera")]
    [Tooltip("Tarik Main Camera yang memiliki script CameraFollow ke sini")]
    public CameraFollow cameraScript;

    private int currentIndex = 0;

    void Start()
    {
        for (int i = 0; i < sequenceObjects.Length; i++)
        {
            if (sequenceObjects[i] != null)
            {
                bool isFirstObject = (i == 0);
                sequenceObjects[i].SetActive(isFirstObject);

                if (isFirstObject)
                {
                    SnapToGround(sequenceObjects[i]);

                    if (cameraScript != null)
                    {
                        cameraScript.target = sequenceObjects[i].transform;
                    }
                }
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CycleNextObject();
        }
    }

    void CycleNextObject()
    {
        if (sequenceObjects.Length == 0) return;

        Vector3 lastPosition = Vector3.zero;
        Quaternion lastRotation = Quaternion.identity;
        Vector3 lastVelocity = Vector3.zero;
        Vector3 lastAngularVelocity = Vector3.zero;
        bool hasLastData = false;

        GameObject currentObj = sequenceObjects[currentIndex];
        if (currentObj != null)
        {
            lastPosition = currentObj.transform.position;
            lastRotation = currentObj.transform.rotation;

            Rigidbody currentRb = currentObj.GetComponent<Rigidbody>();
            if (currentRb != null)
            {
                lastVelocity = currentRb.linearVelocity;
                lastAngularVelocity = currentRb.angularVelocity;
            }

            hasLastData = true;
            currentObj.SetActive(false);
        }

        currentIndex++;
        if (currentIndex >= sequenceObjects.Length)
        {
            currentIndex = 0;
        }

        GameObject nextObj = sequenceObjects[currentIndex];
        if (nextObj != null)
        {
            if (hasLastData)
            {
                Vector3 newPos = nextObj.transform.position;
                newPos.x = lastPosition.x;
                newPos.z = lastPosition.z;
                nextObj.transform.position = newPos;

                nextObj.transform.rotation = lastRotation;
            }

            nextObj.SetActive(true);

            // Fungsi ini sekarang lebih aman dan presisi
            SnapToGround(nextObj);

            if (hasLastData)
            {
                Rigidbody nextRb = nextObj.GetComponent<Rigidbody>();
                if (nextRb != null)
                {
                    nextRb.linearVelocity = lastVelocity;
                    nextRb.angularVelocity = lastAngularVelocity;
                }
            }

            if (cameraScript != null)
            {
                cameraScript.target = nextObj.transform;
            }
        }
    }

    // --- PERBAIKAN LOGIKA ADA DI SINI ---
    void SnapToGround(GameObject obj)
    {
        // 1. Ambil semua collider yang ada di mobil ini (body dan ban)
        Collider[] colliders = obj.GetComponentsInChildren<Collider>();
        if (colliders.Length == 0) return;

        // 2. Simpan rotasi asli dan setel tegak lurus sementara agar pengukuran presisi
        Quaternion originalRotation = obj.transform.rotation;
        obj.transform.rotation = Quaternion.identity;

        // 3. Matikan collider sementara agar Raycast tidak menabrak mobil ini sendiri
        foreach (Collider col in colliders)
        {
            col.enabled = false;
        }

        // 4. Cari nilai paling bawah (Y terendah) dari semua bagian mobil
        float lowestY = float.MaxValue;
        foreach (Collider col in colliders)
        {
            if (!col.isTrigger && col.bounds.min.y < lowestY)
            {
                lowestY = col.bounds.min.y;
            }
        }

        // Hitung jarak dari pivot objek ke ban paling bawah
        float pivotToBottomOffset = 0f;
        if (lowestY != float.MaxValue)
        {
            pivotToBottomOffset = obj.transform.position.y - lowestY;
        }

        // 5. Tembakkan Raycast untuk mencari tanah sungguhan
        Vector3 rayStart = obj.transform.position + (Vector3.up * 5f);
        if (Physics.Raycast(rayStart, Vector3.down, out RaycastHit hit, 20f))
        {
            Vector3 newPos = obj.transform.position;
            newPos.y = hit.point.y + pivotToBottomOffset;
            obj.transform.position = newPos;
        }

        // 6. Kembalikan rotasi ke semula (misal saat sedang menanjak)
        obj.transform.rotation = originalRotation;

        // 7. Nyalakan kembali semua collider mobil
        foreach (Collider col in colliders)
        {
            col.enabled = true;
        }
    }
}