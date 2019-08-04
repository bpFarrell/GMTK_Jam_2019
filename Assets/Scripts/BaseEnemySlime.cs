using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BaseEnemySlime : BaseEnemy, IRoomObject
{
    public float moveSpeed;
    public float jumpSpeed = 5;
    Material material;
    public GameObject targat;
    Vector3 dest;
    public float scaleFactor = 0.1f;
    float startY;
    float startScale;
    bool isDying;
    bool isPaused;
    float timeOfDeath;
    public override void Hit()
    {
        JuiceManager.Hang(3);
        isDying = true;
        timeOfDeath = Time.time;
        MeshRenderer mr = GetComponent<MeshRenderer>();
        mr.material = new Material(mr.material);
        material = mr.material;
        Destroy(GetComponent<Collider>());
        //Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        startY = transform.position.y;
        startScale = transform.localScale.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (isPaused) return;
        if (isDying)
        {
            Dying();
            return;
        }
        Vector3 pos = transform.position;
        float sin = Mathf.Clamp01(Mathf.Sin(Time.time * jumpSpeed)) * 0.1f;
        pos.y = startY;
        if (sin > 0)
        {
            pos = Vector3.MoveTowards(pos, dest, moveSpeed * Time.deltaTime);
        }
        else
        {
            dest = PlayerMovement.Instance.transform.position;
        }
        pos.y += sin;
        transform.position = pos;
        Vector3 scale = transform.localScale;
        scale.y = startScale + sin * scaleFactor;
        transform.localScale = scale;
    }
    void Dying()
    {
        Vector3 scale = transform.localScale;
        scale.x += Time.deltaTime*1;
        scale.y += Time.deltaTime*1;
        scale.z += Time.deltaTime*1;
        transform.localScale = scale;
        float t = (Time.time - timeOfDeath)*3;
        material.SetFloat("_Death", t);
        if (t > 1)
            Kill();
    }

    public void OnRoomTransitionOut(Room room)
    {
        isPaused = true;
        if (isDead) return;
        activeEnemyCount--;
    }

    public void OnRoomTransitionIn(Room room)
    {
        isPaused = false;
        if (isDead) return; 
        activeEnemyCount++;
    }
}
