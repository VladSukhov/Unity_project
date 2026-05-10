using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    [Header("Настройки")]
    public float damagePerHit = 40f;
    public float forceMultiplier = 12f;
    public LayerMask structureLayer;
    public Camera targetCamera;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = targetCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100f, structureLayer))
            {
                var part = hit.collider.GetComponent<DestructiblePart>();
                if (part != null)
                {
                    Vector3 dir = ray.direction.normalized * forceMultiplier;
                    part.ApplyDamage(damagePerHit, hit.point, dir);
                }
            }
        }
    }
}