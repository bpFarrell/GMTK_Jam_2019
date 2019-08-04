using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : SingletonMonoBehaviour<PlayerMovement>
{
    public float moddedMoveSpeed = 1;
    public float moddedTurnSpeed = 1;
    public float faceScaleClamp = 0;
    public state playerState = state.PlayerControlled;
    [Range(0.02f, 0.2f)]
    double goToFloor = 0.05f;

    private float PLAYERHEIGHT = 0;
    private Vector3 destination;

    public enum state {
        PlayerControlled,
        GenericSystemControlled,
        GoTo,
        Animation
    }

    // Start is called before the first frame update
    private void OnEnable()
    {
        PLAYERHEIGHT = transform.position.y;
        RuntimeManager.Play.Enter += GiveControll;
        RuntimeManager.Play.Exit += TakeControll;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 dir = Vector3.zero; 

        if (playerState == state.PlayerControlled)
        {
            dir.x = Input.GetAxis("Horizontal");
            dir.z = Input.GetAxis("Vertical");
            if (dir.magnitude < 0.1f)
                dir = Vector3.zero;
            if (Input.GetKey("w"))
            {
                dir += CameraPost.GetXZNormalizedVector(Vector3.forward);
            }
            if (Input.GetKey("s"))
            {
                dir += CameraPost.GetXZNormalizedVector(-Vector3.forward);
            }
            if (Input.GetKey("a"))
            {
                dir += CameraPost.GetXZNormalizedVector(Vector3.left);
            }
            if (Input.GetKey("d"))
            {
                dir += CameraPost.GetXZNormalizedVector(Vector3.right);
            }
        }
        else if (playerState == state.GoTo)
        {
            dir = GetXZNormalizedVector(destination - transform.position);
            if ((transform.position - new Vector3(destination.x, PLAYERHEIGHT, destination.z)).magnitude <= goToFloor) {
                playerState = state.GenericSystemControlled;
                this.gameObject.layer = 8;
                PlayerCallbacks.PlayerGoToDone?.Invoke();
                PlayerCallbacks.PlayerGoToDone = null;
            }
        }

        MoveTwards(dir);


    }

    private void MoveTwards(Vector3 dir) {
        if (dir.magnitude > 0)
        {
            dir = dir.normalized;
            Quaternion targetRot = Quaternion.LookRotation(dir, Vector3.up);
            Quaternion rotation = Quaternion.Lerp(transform.rotation, targetRot, Time.deltaTime * moddedTurnSpeed);
            transform.rotation = GetYOnlyQuaternion(rotation);
            float faceScale = Vector3.Dot(GetXZNormalizedVector(transform.forward), dir.normalized);
            if (faceScale < faceScaleClamp)
                faceScale = 0;
            //  float faceScaleDtoF = 180 - Vector3.Dot(dir.normalized, GetXZNormalizedVector(transform.forward));
            //float faceScale = faceScaleFtoD <= faceScaleDtoF ? faceScaleFtoD : faceScaleDtoF;
            Vector3 targetDir = GetXZNormalizedVector(transform.forward) * moddedMoveSpeed * Time.deltaTime * faceScale;
            transform.position = transform.position + targetDir;
            if (transform.position.y != PLAYERHEIGHT)
                transform.position = new Vector3(transform.position.x, PLAYERHEIGHT, transform.position.z);
            Rigidbody rigidbody = this.GetComponent<Rigidbody>();
            rigidbody.velocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;
        }
        else
        {
            if (transform.rotation.eulerAngles.x != 0 || transform.rotation.eulerAngles.z != 0)
                transform.rotation = GetYOnlyQuaternion(transform.rotation);
            if (transform.position.y != PLAYERHEIGHT)
            {
                transform.position = new Vector3(transform.position.x, PLAYERHEIGHT, transform.position.z);
            }
            Rigidbody rigidbody = this.GetComponent<Rigidbody>();

            rigidbody.velocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;
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

    public void Teleport(Vector3 destiation, Vector3 lookAtPosition) {
        transform.position = new Vector3(destiation.x, PLAYERHEIGHT, destiation.z);
        transform.LookAt(lookAtPosition);
        transform.rotation = GetYOnlyQuaternion(transform.rotation);
    }

    public void GoTo(Vector3 destination, System.Action goToDone)
    {
        if (PlayerCallbacks.PlayerGoToDone != null) return;
        this.gameObject.layer = 14;
        PlayerCallbacks.PlayerGoToDone = goToDone;
        playerState = state.GoTo;
        this.destination = destination;

    }

    public void TakeControll() {
        playerState = state.GenericSystemControlled;
    }
    public void GiveControll()
    {
        playerState = state.PlayerControlled;
    }

}
