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
        if(room.cameraOverride == 0)
            mainCamera.orthographicSize = magicFloat * room.size;
        else 
            mainCamera.orthographicSize = magicFloat * room.cameraOverride;
    }

    public void ResetCameraOrthographicSize()
    {
        mainCamera.orthographicSize = magicFloat; 
    }
}
