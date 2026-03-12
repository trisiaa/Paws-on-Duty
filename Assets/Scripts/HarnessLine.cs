using UnityEngine;

public class HarnessLine : MonoBehaviour
{
    public Transform maxPoint;
    public Transform handPoint;

    public float maxOffset = 0.15f;
    public float handOffset = 0.05f;

    LineRenderer line;

    void Start()
    {
        line = GetComponent<LineRenderer>();
    }

    void Update()
    {
        // posisi keluar dari badan Max
        Vector3 maxPos = maxPoint.position + maxPoint.forward * maxOffset;

        // posisi sedikit keluar dari tangan Maya
        Vector3 handPos = handPoint.position + handPoint.forward * handOffset;

        line.SetPosition(0, maxPos);
        line.SetPosition(1, handPos);
    }
}