using UnityEngine;

public class CollapseManager : MonoBehaviour
{
    private void Awake()
    {
        DestructiblePart.OnPartDestroyed += HandlePartDestroyed;
    }

    private void HandlePartDestroyed(DestructiblePart part)
    {

    }

    private void OnDestroy() => DestructiblePart.OnPartDestroyed -= HandlePartDestroyed;
}