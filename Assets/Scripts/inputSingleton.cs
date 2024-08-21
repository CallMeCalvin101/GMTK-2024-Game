using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inputSingleton : MonoBehaviour
{
    public static inputSingleton instance;
    public static PlayerControls playerControls;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        } else
        {
            Destroy(this);
        }
    }
    
    public PlayerControls GetControls()
    {
        if (playerControls == null)
        {
            playerControls = new PlayerControls();
        } 

        return playerControls;
    }

}
