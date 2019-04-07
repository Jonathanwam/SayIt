using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintButton : MonoBehaviour
{
    // Gets current phrase from current assessment then calls Text To Speech on the phrase
    public void PlayHint()
    {
        string s = Sentences.currentAssessment.Phrase.ToLower();
        TextToSpeechHandler ttsHandler = GameObject.Find("Robot").GetComponent<TextToSpeechHandler>();
        string phrase = "<speak version=\"1.0\">" + s + "</speak>";
        Debug.Log("Attempting to say phrase...", ttsHandler);
        ttsHandler.SayPhrase(phrase);
    }
}
