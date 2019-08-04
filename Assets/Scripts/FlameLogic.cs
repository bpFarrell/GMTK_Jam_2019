using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void FlameJump(TorchLogic from, TorchLogic to);
public enum FlameLogicState
{
    Active,
    Jumping,
    Locked
}
public class FlameLogic : MonoBehaviour
{
    TorchLogic target;
    const float TORCH_OFFSET_Y = 0.3f;
    Vector3 lastFramePos;
    Vector3 mouseVelocity;
    public FlameLogicState state = FlameLogicState.Active;
    TrailRenderer tr;
    public static FlameJump onFlameJump;
    // Start is called before the first frame update
    private void OnEnable()
    {
    }
    private void OnDisable()
    {

        RuntimeManager.Play.Enter -= OnAnimationComplete;
        RuntimeManager.Play.Exit -= OnAnimationStart;
    }
    void Start()
    {
        tr = GetComponent<TrailRenderer>();
        lastFramePos = Input.mousePosition;
        Cursor.lockState = CursorLockMode.Locked;
        RuntimeManager.Play.Enter += OnAnimationComplete;
        RuntimeManager.Play.Exit += OnAnimationStart;
    }

    // Update is called once per frame
    void Update()
    {
        if (state == FlameLogicState.Locked) return;
        if(state == FlameLogicState.Active)
        {

            CheckMouseSnap();
        }
        else
        {
            float dist = Vector3.Distance(transform.position, target.transform.position);
            if (dist < 0.001f+ TORCH_OFFSET_Y)
                state = FlameLogicState.Active;
        }
        if (target == null) return;
        transform.position = Vector3.Lerp(transform.position, target.transform.position + Vector3.up * TORCH_OFFSET_Y, 10 * Time.deltaTime);
    }
    void CheckMouseSnap()
    {
        Vector3 delta = new Vector3(Input.GetAxis("Mouse X"), 0, Input.GetAxis("Mouse Y"));
        Vector3 stick = new Vector3(Input.GetAxis("RightH"), 0, -Input.GetAxis("RightV"));

        if (stick.magnitude > 0.8)
        {
            Debug.Log(stick);
            JumpToTarget(CameraPost.GetXZNormalizedVector(stick));
        }
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
        if (TorchLogic.torches.Count == 0) return;
        float closestDot = 0;
        int closestIndex = 0;
        for (int x = 0; x < TorchLogic.torches.Count; x++)
        {
            Vector3 torchDir = TorchLogic.torches[x].transform.position - transform.position;
            float nowDot = Mathf.Clamp01(Vector3.Dot(direction.normalized, torchDir.normalized));
            if (nowDot == 0) continue;
            if (!CheckLOS(TorchLogic.torches[x].gameObject)) continue;
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
        Instantiate(Resources.Load("PopEffect"), transform.position, Quaternion.identity);
        state = FlameLogicState.Jumping;
    }
    private void OnTriggerEnter(Collider other)
    {
        BaseEnemy enemey = other.GetComponent<BaseEnemy>();
        if (enemey != null)
        {
            enemey.Hit();
        }
    }
    private void OnAnimationStart()
    {
        tr.Clear();
        state = FlameLogicState.Locked;
    }
    private void OnAnimationComplete()
    {
        tr.Clear();
        state = FlameLogicState.Active;
    }

    bool CheckLOS(GameObject torch)
    {
        RaycastHit hit;
        var rayDirection = torch.transform.position - this.transform.position;
        if (Physics.Raycast(transform.position, rayDirection, out hit, Mathf.Infinity, (1 + (1 << 13))))
        {
            foreach (Transform _tansform in torch.transform)
            {
                if (ReferenceEquals(hit.transform, _tansform))
                {
                    return true;
                }
            }
            return false;
        }
        return false;

    }
}
