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
        Instantiate(Resources.Load("PopEffect"), transform.position, Quaternion.identity);
        state = FlameLogicState.Jumping;
    }
    private void OnTriggerEnter(Collider other)
    {
        IEnemy enemey = other.GetComponent<IEnemy>();
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

}
