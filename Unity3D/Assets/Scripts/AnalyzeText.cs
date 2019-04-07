using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Evaluates STT results against the current phrase and makes changes in-game based on results
 */
public class AnalyzeText : MonoBehaviour
{
    bool good = false;
    bool bad = false;
    //public AssessmentResults resObject;
    private AssessmentResults res;

    void Start()
    {
        res = GameObject.Find("ResultColorCircle").GetComponent<AssessmentResults>();
    }
    void Update()
    {
        //Debug.Log(resObject.spriteRenderer.color.ToString());
        if(good)
        {
            res.CorrectResult();
            good = false;
        }
        if(bad)
        {
            Debug.Log("In bad");
            res.WrongResult();
            bad = false;
        }
    }
    // evaluates 
    public void CheckIfCorrect(string result)
    {
        good = false;
        string check1 = result.ToLower();
        string check2 = Sentences.currentAssessment.Phrase.ToLower();
        Debug.Log("check1: " + check1 + "; check2: " + check2 + ";");
        if(check1.Contains(check2))
        {
            Debug.Log("Correct 1");
            good = true;
        }
        else
        {
            foreach(string s in Sentences.currentAssessment.AlternativeMatches)
            {
                if(result.Contains(s.ToLower()))
                {
                    Debug.Log("Correct 2");
                    good = true;
                    break;
                }
            }
        }
        if(!good)
        {
            Debug.Log("Incorrect");
            //Debug.Log(resObject.ToString());
            //resObject.WrongResult();
            bad = true;
        }
        
    }
}
