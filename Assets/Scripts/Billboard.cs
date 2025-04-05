using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Camera mainCamera;
    
    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        // face camera but only along the y axis
        Vector3 lookDirection = mainCamera.transform.position - transform.position;
        lookDirection.y = 0; // keep the y component unchanged
        Quaternion rotation = Quaternion.LookRotation(lookDirection);
        transform.rotation = rotation;
        // set the rotation to the camera's rotation
        transform.rotation = Quaternion.Euler(0, mainCamera.transform.eulerAngles.y, 0);

    }
}
