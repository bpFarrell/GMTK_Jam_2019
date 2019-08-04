using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VSFXLogic : MonoBehaviour
{
    public enum VSFXAnimation
    {
        Shake,
        Pop,
    }
    public string sound;
    public float duration;
    TextMesh[] texts;
    Vector3[] targets;
    public VSFXAnimation type;
    public float freq = 1;
    public float mag = 1;
    float seed;
    float timeStart;
    bool cleaningUp;
    float cleaningUpAt;
    float timeAlive {
        get { return Time.time - timeStart; }
    }
    float timeTillKill {

        get { return Time.time - cleaningUpAt; }
    }
    private void OnEnable()
    {
        RoomManager.Instance.OnRoomSwap += Kill;
    }
    private void OnDisable()
    {
        RoomManager.Instance.OnRoomSwap-= Kill;
    }
    private void Kill()
    {
        Destroy(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        if (!string.IsNullOrEmpty(sound))
        {
            Init(sound);
        }
    }
    public void Init(string text)
    {
        seed = Random.value;
        timeStart = Time.time;
        texts = new TextMesh[sound.Length];
        targets = new Vector3[texts.Length];
        for (int x = 0; x < texts.Length; x++)
        {
            GameObject go = new GameObject(sound[x] + "");
            go.transform.parent = transform;
            targets[x] = new Vector3(x, 0, 0);
            go.transform.localPosition = targets[x];
            go.transform.localRotation = Quaternion.identity;
            go.transform.localScale = Vector3.one;

            texts[x] = go.AddComponent<TextMesh>();
            texts[x].text = sound[x] + "";
            texts[x].color = Color.clear;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (timeAlive > duration&& !cleaningUp)
        {
            CleanUp();
        }
        if (cleaningUp && timeTillKill > 1)
        {
            Destroy(gameObject);
        }
        switch (type)
        {
            case VSFXAnimation.Shake:
                Shake();
                break;
            case VSFXAnimation.Pop:
                Pop();
                break;
            default:
                break;
        }
    }
    void Shake()
    {
        for (int x = 0; x < texts.Length; x++)
        {
            texts[x].color = TimeColor(0,0);
            texts[x].transform.localPosition = targets[x] + new Vector3(
                Mathf.PerlinNoise(x + Time.time * freq, 0),
                Mathf.PerlinNoise(x + Time.time * freq, 1),
                Mathf.PerlinNoise(x + Time.time * freq, 2)) * mag;
        }
    }
    void Pop()
    {
        for (int x = 0; x < texts.Length; x++)
        {
            texts[x].color = TimeColor(x*0.3f,0);
            texts[x].transform.localPosition = targets[x] + new Vector3(0,0, 
                Mathf.Clamp01(timeAlive*5 - x*0.3f-1) *10-10 ) *
                mag;
            texts[x].transform.localEulerAngles = new Vector3(0, 0, Mathf.PerlinNoise(seed * 10, x * 1.3f) * 80 - 40);
        }
    }
    private Color TimeColor(float spawnOffset,float killOffset)
    {
        return new Color(1, 1, 1, cleaningUp ?
                Mathf.Clamp01((1 - timeTillKill+killOffset) * 5) :
                Mathf.Clamp01(timeAlive * 5-spawnOffset));
    }
    public void CleanUp()
    {
        cleaningUpAt = Time.time;
        cleaningUp = true;
    }

}
