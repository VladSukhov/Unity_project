using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DestructiblePart : MonoBehaviour
{
    [Header("Здоровье")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float collapseDelay = 0.4f;

    public List<DestructiblePart> supportsMe = new();
    public List<DestructiblePart> supportsOthers = new();

    public delegate void PartDestroyed(DestructiblePart part);
    public static event PartDestroyed OnPartDestroyed;

    [Header("Эффекты")]
    public GameObject dustPrefab;
    
    private float currentHealth;
    private Rigidbody rb;
    private Collider col;
    private Joint joint;
    private bool isDestroyed;

    private void Awake()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        joint = GetComponent<Joint>();
        isDestroyed = false;

        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
            
            // настройки против туннелирования
            rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            rb.interpolation = RigidbodyInterpolation.Interpolate;
            rb.sleepThreshold = 0.005f;
            rb.mass = transform.localScale.y > 1.5f ? 5f : 2f;
        }
    }

    public void ApplyDamage(float damage, Vector3 hitPoint, Vector3 forceDir)
    {
        if (isDestroyed) return;

        currentHealth -= damage;
        if (currentHealth <= 0f) DestroyPart(hitPoint, forceDir);
    }

    private void DestroyPart(Vector3 point, Vector3 dir)
    {
        if (dustPrefab != null) 
        {
            Instantiate(dustPrefab, transform.position, transform.rotation);
        }
        isDestroyed = true;
        rb.isKinematic = false;
        rb.useGravity = true;
        if (joint != null) Destroy(joint);

        rb.AddForceAtPosition(dir * 3f, point, ForceMode.Impulse);
        OnPartDestroyed?.Invoke(this);
        NotifyDependents();
    }

    public void TriggerCollapse()
    {
        if (isDestroyed) return;
        StartCoroutine(CollapseRoutine());
    }

    private IEnumerator CollapseRoutine()
    {
        yield return new WaitForSeconds(collapseDelay + Random.value * 0.2f);
        if (isDestroyed) yield break;

        if (joint != null) Destroy(joint);
        rb.isKinematic = false;
        rb.useGravity = true;
        
        transform.position += Vector3.up * 0.06f;

        rb.AddForce(Vector3.down * 1.5f + Vector3.right * Random.Range(-0.5f, 0.5f), ForceMode.Impulse);
        
        isDestroyed = true;
        OnPartDestroyed?.Invoke(this);
        NotifyDependents();
    }

    private void NotifyDependents()
    {
        foreach (var dep in supportsOthers)
        {
            dep.supportsMe.Remove(this);
            if (dep.supportsMe.Count == 0 && !dep.isDestroyed)
                dep.TriggerCollapse();
        }
    }
}