using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moddedMoveSpeed = 1;
    public float moddedTurnSpeed = 1;
    private float PLAYERHEIGHT = 0;
 
    // Start is called before the first frame update
    void Start()
    {
        PLAYERHEIGHT = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 dir = Vector3.zero;
        if (Input.GetKey("w"))
        {
            dir += GetXZNormalizedVector(Camera.main.transform.forward);
        }
        if (Input.GetKey("s"))
        {
            dir += GetXZNormalizedVector(-Camera.main.transform.forward);
        }
        if (Input.GetKey("a"))
        {
            dir += GetXZNormalizedVector(-Camera.main.transform.right);
        }
        if (Input.GetKey("d"))
        {
            dir += GetXZNormalizedVector(Camera.main.transform.right);
        }

        if (dir.magnitude > 0)
        {
            Quaternion targetRot = Quaternion.LookRotation(dir, Vector3.up);
            Quaternion rotation = Quaternion.Lerp(transform.rotation, targetRot, Time.deltaTime * moddedTurnSpeed);
            transform.rotation = GetYOnlyQuaternion(rotation);
            float faceScale = (1 + Vector3.Dot(GetXZNormalizedVector(transform.forward), dir.normalized)) /2;
          //  float faceScaleDtoF = 180 - Vector3.Dot(dir.normalized, GetXZNormalizedVector(transform.forward));
            //float faceScale = faceScaleFtoD <= faceScaleDtoF ? faceScaleFtoD : faceScaleDtoF;
            Vector3 targetDir = GetXZNormalizedVector(transform.forward) * moddedMoveSpeed * Time.deltaTime * faceScale;
            transform.position = transform.position + targetDir;
        }
        else
        {
            if (transform.rotation.eulerAngles.x != 0 || transform.rotation.eulerAngles.z != 0)
                transform.rotation = GetYOnlyQuaternion(transform.rotation);
            if (transform.position.y != PLAYERHEIGHT)
            {
                transform.position = new Vector3(transform.position.x, PLAYERHEIGHT, transform.position.z);
            }
        }

        
    }

    private Quaternion GetYOnlyQuaternion(Quaternion quat)
    {
        Vector3 euler = quat.eulerAngles;
      //  float mag = euler.magnitude;
        Vector3 ret = new Vector3(0, euler.y, 0);
      //  ret = ret.normalized * mag;
        return Quaternion.Euler(ret.x, ret.y, ret.z);
    }

    private Vector3 GetXZNormalizedVector(Vector3 vec) {
        Vector3 ret = new Vector3(vec.x, 0, vec.z);
        ret = ret.normalized;
        return ret;
    }
}
