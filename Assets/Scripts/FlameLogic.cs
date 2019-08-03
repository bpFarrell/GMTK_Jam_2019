using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameLogic : MonoBehaviour
{
    GameObject target;
    const float TORCH_OFFSET_Y = 0.3f;
    // Start is called before the first frame update
    void Start()
    {
        
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
            JumpToTarget(direction);
        }
        if (target == null) return;
        transform.position = Vector3.Lerp(transform.position, target.transform.position + Vector3.up * TORCH_OFFSET_Y, 10 * Time.deltaTime);
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
        target = TorchLogic.torches[closestIndex].gameObject;
    }
}
