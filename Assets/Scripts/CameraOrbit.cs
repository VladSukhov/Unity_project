using UnityEngine;

public class CameraOrbit : MonoBehaviour
{
    [Header("Настройки вращения")]
    public float rotationSpeed = 100f;
    public bool autoRotate = true;

    private bool isUserControlling = false;

    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            isUserControlling = true;
            
            float mouseX = Input.GetAxis("Mouse X");
            
            transform.Rotate(0, mouseX * rotationSpeed * Time.deltaTime, 0);
        }
        else
        {
            
        }
        
        if (autoRotate && !isUserControlling)
        {
            transform.Rotate(0, 20f * Time.deltaTime, 0);
        }
    }
}