using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPost : MonoBehaviour
{
    Camera cam;
    public Material mat;
    private static CameraPost instance;
    public void Start()
    {
        instance = this;
        cam = GetComponent<Camera>();
    }
    public static Quaternion GetYOnlyQuaternion(Quaternion quat)
    {
        Vector3 euler = quat.eulerAngles;
        //  float mag = euler.magnitude;
        Vector3 ret = new Vector3(0, euler.y, 0);
        //  ret = ret.normalized * mag;
        return Quaternion.Euler(ret.x, ret.y, ret.z);
    }

    public static Vector3 GetXZNormalizedVector(Vector3 vec)
    {
        Vector3 camSpace =
            instance.cam.transform.right * vec.x +
            instance.cam.transform.forward * vec.z;
        Vector3 ret = new Vector3(camSpace.x, vec.y, camSpace.z);
        ret = ret.normalized;
        return ret;
    }
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, mat);
    }
}
