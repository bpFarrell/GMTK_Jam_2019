using UnityEngine;

public class CameraRoomScaler : SingletonMonoBehaviour<CameraRoomScaler>
{
    public Camera mainCamera;
    private const float magicFloat = .59f; 
    
    private void OnEnable()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main; 
        }

        if (mainCamera == null)
        {
            Debug.LogError("MAIN_CAMERA_NULL", mainCamera);
            return; 
        }
    }

    public void SetCameraOrthographicSize(Room room)
    {
        if (room == null) {
            Debug.Log("set room size failure");
            return;
        }
        mainCamera.orthographicSize = magicFloat * room.size; 
    }

    public void ResetCameraOrthographicSize()
    {
        mainCamera.orthographicSize = magicFloat; 
    }
}
