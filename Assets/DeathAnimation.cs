using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathAnimation : MonoBehaviour
{
    public Material screenMat;
    public Material playerMat;
    public bool isDeath;
    public float speed;
    public float screenSpeed;
    float t;
    private void OnEnable()
    {
        RuntimeManager.Death.Enter += StartDeath;
        playerMat.SetFloat("_Death", 0);
        screenMat.SetVector("_Exposure", new Vector4(0.45f,0.15f,0,0));
    }
    private void OnDisable()
    {
        RuntimeManager.Death.Enter -= StartDeath;
    }
    void StartDeath()
    {
        isDeath = true;
        t = 0;
        Time.timeScale = 0;
    }
    private void Update()
    {
        if (!isDeath) return;
        t += Time.unscaledDeltaTime * speed;
        playerMat.SetFloat("_Death", t);
        Vector4 exp = screenMat.GetVector("_Exposure");
        exp.x += Time.unscaledDeltaTime * screenSpeed;
        screenMat.SetVector("_Exposure", exp);
        if (exp.x > 1.5f)
        {
            RuntimeManager.SetState(RuntimeManager.GameState.END);
            Time.timeScale = 1;

        }
    }

}
