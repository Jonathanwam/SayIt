using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Assessment object holds the phrase the user should say, alternative matches the STT API may recognize instead (Avoid false negatives!) 
 * 
 */
public class Assessment
{
    public string Phrase { get; set; }
    public string[] AlternativeMatches { get; set; }
    public Assessment(string phrase, string[] alternativeMatches)
    {
        Phrase = phrase;
        AlternativeMatches = alternativeMatches;
    }
}