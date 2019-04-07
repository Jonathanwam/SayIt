using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * Implementation of the next word button.
 * Switches word and resets the game state back to default state.
 */
public class Reset : MonoBehaviour
{
    GameObject btn;
    public Button nextWordButton;
    void Start()
    {
        btn = GameObject.Find("NextWord");
        nextWordButton.onClick.AddListener(TaskOnClick);
    }
    private void OnMouseDown()
    {
        Sentences.changeText = true;
        GameObject.Find("RecordedText").GetComponent<UnityEngine.UI.Text>().text = "Click and hold the red button then say the word!";
        Robot rob = GameObject.Find("Robot").GetComponent<Robot>();
        rob.UpdateToNeutral();
        DisableButton();
    }
    private void TaskOnClick()
    {
        Sentences.changeText = true;
        GameObject.Find("RecordedText").GetComponent<UnityEngine.UI.Text>().text = "Click and hold the red button then say the word!";
        Robot rob = GameObject.Find("Robot").GetComponent<Robot>();
        rob.UpdateToNeutral();
        DisableButton();
    }

    public void EnableButton()
    {
        btn.SetActive(true);
    }

    public void DisableButton()
    {
        btn.SetActive(false);
        //GameObject.Find("RecordButton").SetActive(true);
    }
}
