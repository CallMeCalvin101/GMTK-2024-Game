using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();
    }

    public void OnPlayButton()
    {
        SceneManager.LoadSceneAsync(1);
    }
    public void OnCreditsButton()
    {
        SceneManager.LoadSceneAsync(2);
    }

    public void OnQuitButton()
    {
        Application.Quit();
    }
}
