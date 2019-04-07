using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * Evaluates if the user said the phrase correctly or not
 * If right, allow user to switch to next word.
 * If wrong 3 times in a row, allow user to switch to next word anyways.
 * Calls Robot script to change robot sprite when necessary.
 */
public class AssessmentResults : MonoBehaviour
{
    [SerializeField]
    public SpriteRenderer spriteRenderer;
    public Color correctColor = Color.green;
    public Color wrongColor = Color.red;
    public bool changeColor = true;
    Robot rob;
    Reset resBtn;
    int counter;

    void Start()
    {
        resBtn = GameObject.Find("NextWord").GetComponent<Reset>();
    }
    // Update is called once per frame
    void Update()
    {
    }

    // When user got it right
    public void CorrectResult()
    {
        AudioSource audio = GameObject.Find("CorrectAudio").GetComponent<AudioSource>();
        GameObject.Find("RecordedText").GetComponent<UnityEngine.UI.Text>().text = "Good job, you said it right!";
        // TextToSpeechHandler ttsHandler = GameObject.Find("Robot").GetComponent<TextToSpeechHandler>();
        // string goodJobPhrase = "<speak version=\"1.0\"><express-as type=\"GoodNews\">Good job, you said it right!</express-as></speak>";
        // Debug.Log("Attempting to say phrase...",ttsHandler);
        // ttsHandler.SayPhrase(goodJobPhrase);
        audio.Play();
        rob = GameObject.Find("Robot").GetComponent<Robot>();
        rob.UpdateToGoodJob();
        counter = 0;
        // show reset button and disable record button
        //GameObject.Find("NextWord").SetActive(true);
        resBtn.EnableButton();
        //GameObject.Find("RecordButton").SetActive(false);
    }

    // When user got it wrong
    public void WrongResult()
    {
        AudioSource audio = GameObject.Find("WrongAudio").GetComponent<AudioSource>();
        GameObject.Find("RecordedText").GetComponent<UnityEngine.UI.Text>().text = "I'm not sure that was the word above me. Could you try again?";
        rob = GameObject.Find("Robot").GetComponent<Robot>();
        rob.UpdateToWrong();
        audio.Play();
        counter++;
        if(counter > 2)
        {
            //GameObject.Find("NextWord").SetActive(true);
            resBtn.EnableButton();
        }
    }

    // evaluates 
    public void CheckIfCorrect(string result)
    {
        string check1 = result.ToLower();
        
        string check2 = Sentences.currentAssessment.Phrase.ToLower();
        Debug.Log("check1: " + check1 + "; check2: " + check2 + ";");
        if (check1.Contains(check2))
        {
            Debug.Log("Correct 1");
            CorrectResult();
            return;
        }
        else
        {
            check1 = check1.Substring(0, check1.IndexOf("("));
            foreach (string s in Sentences.currentAssessment.AlternativeMatches)
            {
                string str = s.ToLower();

                Debug.Log("Checking if " + check1 + " contains: " + str);
                bool contains = check1.IndexOf(str, StringComparison.OrdinalIgnoreCase) >= 0;
                Debug.Log("Does it? ... " + check1.Contains(str) + " , " + contains);
                if (check1.Contains(str) || contains)
                {
                    CorrectResult();
                    return;
                }
            }
        }
        // else it's a wrong answer
        Debug.Log("Incorrect");
        WrongResult();

    }
}
