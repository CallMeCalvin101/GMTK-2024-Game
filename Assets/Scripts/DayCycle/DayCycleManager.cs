using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;
//using UnityEngine.Experimental.GlobalIllumination;

public class DayCycleManager : MonoBehaviour
{
    public static DayCycleManager instance;
    public GameObject directionalLightObj;

    public float minutesForFullCycle = 20f;
    private float timer = 0f;
    private float fullRotation = 360;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        } else
        {
            print("Day Cycle Already exists");
            Destroy(this);
        }
    }

    private void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;

        float percentDay = timer / (minutesForFullCycle*60);
        float rotation  = fullRotation * percentDay;

        directionalLightObj.transform.rotation = Quaternion.Euler(rotation, 0f, 0f);
    }
}
