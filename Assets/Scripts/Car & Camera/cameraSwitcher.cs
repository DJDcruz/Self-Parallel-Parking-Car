using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    public Camera[] cameras; // Array to hold the camera views
    private int currentCameraIndex = 0; // Start with the first camera view

    void Start()
    {
        // Disable all cameras initially
        foreach (Camera cam in cameras)
        {
            cam.gameObject.SetActive(false);
        }
        // Enable the first camera
        cameras[currentCameraIndex].gameObject.SetActive(true);
    }

    void Update()
    {
        // Switch cameras with arrow keys (or any keys you prefer)
        if (Input.GetKeyDown(KeyCode.Alpha1)) // Key 1 for first camera
        {
            SwitchCamera(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2)) // Key 2 for second camera
        {
            SwitchCamera(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3)) // Key 3 for third camera
        {
            SwitchCamera(2);
        }
    }

    void SwitchCamera(int index)
    {
        // Disable the current camera and enable the new one
        cameras[currentCameraIndex].gameObject.SetActive(false);
        currentCameraIndex = index;
        cameras[currentCameraIndex].gameObject.SetActive(true);
    }
}
