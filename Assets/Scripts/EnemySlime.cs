using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySlime : MonoBehaviour, IEnemy
{
    public float moveSpeed;
    public float jumpSpeed = 5;

    public GameObject targat;
    public float scaleFactor = 0.1f;
    float startY;
    float startScale;
    public void Hit()
    {
        Destroy(gameObject);
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
        Vector3 pos = transform.position;
        float sin = Mathf.Clamp01(Mathf.Sin(Time.time * jumpSpeed)) * 0.1f;
        pos.y = startY;
        if (sin > 0)
        {
            pos = Vector3.MoveTowards(pos, targat.transform.position, moveSpeed * Time.deltaTime);
        }
        pos.y += sin;
        transform.position = pos;
        Vector3 scale = transform.localScale;
        scale.y = startScale + sin * scaleFactor;
        transform.localScale = scale;
    }
}
