using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PullingMinigame : MonoBehaviour
{
    // Field for max score
    public int goalSuccessions, actsTillReset;

    // GameObject References
    public HarpoonFire harpoon;
    public Text actionText;
    public Image actionBox;

    // Global Trackers
    private int playerAction, numSucceeded;
    private bool isActive;

    // Start is called before the first frame update
    void Start()
    {
        isActive = false;
        enableActionsUI(false);
    }

    public void startNewMinigame()
    {
        enableActionsUI(true);
        isActive = true;
        numSucceeded = 0;
        randomizeAction();
    }
    
    // Update is called once per frame
    void Update()
    {
        if (isActive) checkAction();
    }

    private void checkAction()
    {
        if (Input.GetKeyDown(KeyCode.Q) && playerAction == 0) iterateScore();
        else if (Input.GetKeyDown(KeyCode.W) && playerAction == 1) iterateScore();
        else if (Input.GetKeyDown(KeyCode.E) && playerAction == 2) iterateScore();
        else if (Input.GetKeyDown(KeyCode.A) && playerAction == 3) iterateScore();
        else if (Input.GetKeyDown(KeyCode.S) && playerAction == 4) iterateScore();
        else if (Input.GetKeyDown(KeyCode.D) && playerAction == 5) iterateScore();
        else if (Input.GetKeyDown(KeyCode.Z) && playerAction == 6) iterateScore();
        else if (Input.GetKeyDown(KeyCode.X) && playerAction == 7) iterateScore();
        else if (Input.GetKeyDown(KeyCode.C) && playerAction == 8) iterateScore();
    }

    private void iterateScore()
    {
        numSucceeded++;
        if (numSucceeded >= goalSuccessions) processWin();
        else if (numSucceeded % actsTillReset == 0) randomizeAction();
    }

    private void processWin()
    {
        playerAction = -1;
        updateActionText();
        isActive = false;
        enableActionsUI(false);
        harpoon.endMinigame(true);
    }

    private void randomizeAction()
    {
        playerAction = Random.Range(0, 9);
        updateActionText();
    }

    private void updateActionText()
    {
        if (playerAction == 0) actionText.text = "Q";
        else if (playerAction == 1) actionText.text = "W";
        else if (playerAction == 2) actionText.text = "E";
        else if (playerAction == 3) actionText.text = "A";
        else if (playerAction == 4) actionText.text = "S";
        else if (playerAction == 5) actionText.text = "D";
        else if (playerAction == 6) actionText.text = "Z";
        else if (playerAction == 7) actionText.text = "X";
        else if (playerAction == 8) actionText.text = "C";
        else actionText.text = "+->";
    }

    private void enableActionsUI(bool state)
    {
        actionBox.enabled = state;
        actionText.enabled = state;
    }
}
