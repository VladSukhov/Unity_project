using UnityEngine;

public class SupportGraphBuilder : MonoBehaviour
{
    [Header("Настройки")]
    public float detectionRadius = 0.8f;
    public LayerMask structureLayer;

    private void Start()
    {
        var myPart = GetComponent<DestructiblePart>();
        if (myPart == null) return;

        Vector3 checkPos = transform.position + Vector3.down * 0.5f;
        var hits = Physics.OverlapSphere(checkPos, detectionRadius, structureLayer);

        foreach (var hit in hits)
        {
            var other = hit.GetComponent<DestructiblePart>();
            if (other != null && other != myPart)
            {
                if (!myPart.supportsMe.Contains(other)) myPart.supportsMe.Add(other);
                if (!other.supportsOthers.Contains(myPart)) other.supportsOthers.Add(myPart);
            }
        }
        
        if (myPart.supportsMe.Count == 0 && transform.position.y < 1.0f)
        {

        }
    }
}