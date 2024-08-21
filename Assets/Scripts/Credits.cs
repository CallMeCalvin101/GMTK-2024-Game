using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{
    public void onCMC()
    {
        Application.OpenURL("https://github.com/CallMeCalvin101");
    }
    public void onSM()
    {
        Application.OpenURL("https://github.com/SaltMeister");
    }
    public void onSae()
    {
        Application.OpenURL("https://github.com/saewiwi");
    }
    public void onYRK()
    {
        Application.OpenURL("https://github.com/yuruki0");
    }
    public void OnBack()
    {
        SceneManager.LoadSceneAsync(0);
    }
}
