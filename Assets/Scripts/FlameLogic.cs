using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void FlameJump(TorchLogic from, TorchLogic to);
public class FlameLogic : MonoBehaviour
{
    TorchLogic target;
    const float TORCH_OFFSET_Y = 0.3f;
    Vector3 lastFramePos;
    Vector3 mouseVelocity;
    public static FlameJump onFlameJump;
    // Start is called before the first frame update
    void Start()
    {
        lastFramePos = Input.mousePosition;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = Vector3.zero;

        if (Input.GetKey(KeyCode.W)){
            direction.z += 1;
        }

        if (Input.GetKey(KeyCode.A)){

            direction.x -= 1;
        }

        if (Input.GetKey(KeyCode.S)){

            direction.z -= 1;
        }
        if (Input.GetKey(KeyCode.D)){

            direction.x += 1;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            JumpToTarget(CameraPost.GetXZNormalizedVector(direction));
        }
        CheckMouseSnap();
        if (target == null) return;
        transform.position = Vector3.Lerp(transform.position, target.transform.position + Vector3.up * TORCH_OFFSET_Y, 10 * Time.deltaTime);
    }
    void CheckMouseSnap()
    {
        Vector3 delta = new Vector3(Input.GetAxis("Mouse X"), 0, Input.GetAxis("Mouse Y"));
        mouseVelocity *= 0.5f;
        mouseVelocity += delta;
        lastFramePos = Input.mousePosition;
        if (mouseVelocity.magnitude > 10)
        {
            JumpToTarget(CameraPost.GetXZNormalizedVector(mouseVelocity));
            mouseVelocity = Vector3.zero;
        }
    }
    void JumpToTarget(Vector3 direction)
    {
        float closestDot = 0;
        int closestIndex = 0;
        for (int x = 0; x < TorchLogic.torches.Count; x++)
        {
            Vector3 torchDir = TorchLogic.torches[x].transform.position - transform.position;
            float nowDot = Mathf.Clamp01(Vector3.Dot(direction.normalized, torchDir.normalized));
            if (nowDot == 0) continue;
            nowDot /= torchDir.magnitude;
            if (nowDot > closestDot)
            {
                closestDot = nowDot;
                closestIndex = x;
            }
        }
        if(target!=null)
            target.SetOff();
        if (onFlameJump != null)
            onFlameJump(target, TorchLogic.torches[closestIndex]);
        target = TorchLogic.torches[closestIndex];
        target.SetOn();
    }
    private void OnTriggerEnter(Collider other)
    {
        IEnemy enemey = other.GetComponent<IEnemy>();
        if (enemey != null)
        {
            enemey.Hit();
        }
    }

}
