using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

/*
 * Builds array of Assessment objects from sentences.txt located in the Resources folder
 * Keeps track of current assessment. Also previous assessment to avoid 2 in a row of the same assessment.
 * Changes text when needed by changing the value of changeText to true
 */
public class Sentences : MonoBehaviour
{
    public static bool changeText = false;
    private Assessment previousAssessment = null;
    public static Assessment currentAssessment;
    public TextAsset file;
    static TextAsset textFile;
    string path;
    static Assessment[] assessments;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Text>().text = "Loading...";
        file = (TextAsset)Resources.Load("Assets/Resources/sentences.txt");
        textFile = Resources.Load("sentences") as TextAsset;
        assessments = BuildAssessments();
        previousAssessment = assessments[0];
        changeText = true;
    }

    // Update is called once per frame
    // checks to see if text needs to be changed. Changes text if needed.
    void Update()
    {
        if(changeText)
        {
            Assessment a = GetAssessment();
            // avoid having the same phrase twice in a row
            if(!a.Phrase.Equals(previousAssessment.Phrase))
            {
                changeText = false;
                // change main text to str
                gameObject.GetComponent<Text>().text = a.Phrase;
                previousAssessment = a;
                currentAssessment = a;
            }
        }
    }
    
    // builds the assessments array from text file
    public Assessment[] BuildAssessments()
    {
        string[] linesFromfile = textFile.text.Split("\n"[0]);
        Assessment[] assessmentList = new Assessment[linesFromfile.Length];
        int j = 0;
        foreach(string l in linesFromfile)
        {
            string[] line = l.Split(","[0]); // each line has phrase and alternatives seperated by commas
            string phrase = line[0]; // phrase is first
            string[] alts = new string[line.Length - 1]; // alternatives are after
            for(int i = 1; i < line.Length; i++)
            {
                alts[i - 1] = line[i];
            }
            Assessment a = new Assessment(phrase, alts);
            assessmentList[j] = a;
            j++;
        }
        return assessmentList;
    }

    // gets a random assessment from assessments array and returns it 
    public static Assessment GetAssessment()
    {
        Random rnd = new Random();
        int rand = Random.Range(0, assessments.Length-1);
        return assessments[rand];
    }
}
