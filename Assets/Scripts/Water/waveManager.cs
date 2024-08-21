using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public class waveManager : MonoBehaviour
{
    public static waveManager instance;

    public float amplitude = 1.0f;
    public float length = 1.0f;
    public float speed = 1.0f;
    public float offset = 0.0f;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }  else
        {
            print("Alreadty exists");
            Destroy(this);
        }
    }
    private void FixedUpdate()
    {
        offset += Time.fixedDeltaTime * speed;
    }

    public float GetWaveWaveHeight(float x)
    {
        return  transform.position.y + (amplitude * Mathf.Sin(x / length + offset));
    }
}
