using UnityEngine;
using System;
using System.IO;
using System.Net;

using IBM.Watson.DeveloperCloud.Logging;
using IBM.Watson.DeveloperCloud.Services.SpeechToText.v1;
using IBM.Watson.DeveloperCloud.Utilities;
using IBM.Watson.DeveloperCloud.DataTypes;
using IBM.Watson.DeveloperCloud.Connection;
using System.Collections.Generic;
using UnityEngine.UI;

using IBM.Watson.DeveloperCloud.Services.TextToSpeech.v1;
using System.Collections;
using SimpleJSON;

[DisallowMultipleComponent]
/*
 * Implementation of IBM Watson and the record button
 * When button is held down, records voice.
 * When button is back up, sends recording to IBM Watson, retrieves text results.
 * If results is not empty, calls AssessmentResults to analyze the results.
 * 
 */
public class Record : MonoBehaviour
{
    #region PLEASE SET THESE VARIABLES IN THE INSPECTOR
    [Space(10)]
    [Tooltip("The service URL (optional). This defaults to \"https://stream.watsonplatform.net/speech-to-text/api\"")]
    [SerializeField]
    private string _serviceUrl;
    [Header("IAM Authentication")]
    [Tooltip("The IAM apikey.")]
    [SerializeField]
    private string _iamApikey;
    #endregion


    public string speechToTextResult; // stores result from IBM Watson STT --> use it to assess text
    private AssessmentResults analyzer;
    AudioClip recording;
    AudioSource audioSource;
    private float startRecordingTime;

    private SpeechToText _speechToText;

    
    private void Start()
    {
        Debug.Log("Start");
        LogSystem.InstallDefaultReactors();
        // create Watson service
        Runnable.Run(CreateService());
        // note if there are mic devices found
        foreach (string device in Microphone.devices)
        {
            Debug.Log(device);
        }

        audioSource = gameObject.AddComponent<AudioSource>();

        analyzer = gameObject.AddComponent<AssessmentResults>();
    }

    // creates Watson credentials and initializes STT service
    private IEnumerator CreateService()
    {

        if (string.IsNullOrEmpty(_iamApikey))
        {
            throw new WatsonException("Plesae provide IAM ApiKey for the service.");
        }

        //  Create credential and instantiate service
        Credentials credentials = null;

        //  Authenticate using iamApikey
        TokenOptions tokenOptions = new TokenOptions()
        {
            IamApiKey = _iamApikey
        };

        credentials = new Credentials(tokenOptions, _serviceUrl);

        if(credentials != null) Debug.Log("Credentials is not null!");
        Log.Critical("Entering while loop!","");
        Console.WriteLine("Entering while loop!");
        //  Wait for tokendata
        while (!credentials.HasIamTokenData())
        {
            Debug.Log("In the while loop"); // INFINITE LOOP ON ANDROID
            yield return null;
        }

        Debug.Log("OUT of the while loop");

        _speechToText = new SpeechToText(credentials); // NEVER REACHES ON ANDROID
    }

    // mouse down ==> start recording
    public void OnMouseDown()
    {
        Debug.Log("Start Recording");
        //Get the max frequency of a microphone, if it's less than 44100 record at the max frequency, else record at 44100
        int minFreq;
        int maxFreq;
        int freq = 44100;
        Microphone.GetDeviceCaps("", out minFreq, out maxFreq);
        if (maxFreq < 44100)
            freq = maxFreq;

        //Start the recording, the length of 300 gives it a cap of 5 minutes
        recording = Microphone.Start("", false, 15, 22055);
        startRecordingTime = Time.time;
    }

    // mouse up ==> stop recording, save clip and send to goWatson
    public void OnMouseUp()
    {
        Debug.Log("Stop Recording");
        //End the recording when the mouse comes back up, then play it
        Microphone.End("");

        //Trim the audioclip by the length of the recording
        AudioClip recordingNew = AudioClip.Create(recording.name, (int)((Time.time - startRecordingTime) * recording.frequency), recording.channels, recording.frequency, false);
        float[] data = new float[(int)((Time.time - startRecordingTime) * recording.frequency)];
        recording.GetData(data, 0);
        recordingNew.SetData(data, 0);
        this.recording = recordingNew;

        //Play recording
        audioSource.clip = recording;
        audioSource.Play();
        goWatson(audioSource.clip);

        
    }

    // call Watson with recorded clip
    public void goWatson(AudioClip clip)
    {
        Log.Debug("STT: ", "Attempting to recognize from Watson");
        List<string> keywords = new List<string>();
        keywords.Add("speech");
        _speechToText.KeywordsThreshold = 0.5f;
        _speechToText.InactivityTimeout = 120;
        _speechToText.StreamMultipart = false;
        _speechToText.Keywords = keywords.ToArray();
        _speechToText.Recognize(HandleOnRecognize, OnFail, clip);
    }

    // On successful call to Watson, Watson's result is sent to here
    // parses result and shows it on screen with confidence level
    private void HandleOnRecognize(SpeechRecognitionEvent result, Dictionary<string, object> customData)
    {
        Debug.Log("Congrats!");
        Log.Debug("ExampleSpeechToText.HandleOnRecognize()", "Speech to Text - Get model response: {0}", customData["json"].ToString());
        var json = JSON.Parse(customData["json"].ToString());
        Debug.Log("JSON: " + json);
        string text = "";
        // parse result and show STT on screen
        if (result != null && result.results.Length > 0)
        {
            foreach (var res in result.results)
            {
                foreach (var alt in res.alternatives)
                {
                    text = string.Format("{0} ({1}, {2:0.00})\n", alt.transcript, res.final ? "Final" : "Interim", alt.confidence);
                }
            }
        }
        if (text.Length == 0)
        {
            Robot rob = GameObject.Find("Robot").GetComponent<Robot>();
            GameObject.Find("RecordedText").GetComponent<Text>().text = "I didn't quite hear you. Could you try again?";
            rob.UpdateToDidNotHear();
        }
        else
        {
            speechToTextResult = "";
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i + 1] == '(') break;
                else speechToTextResult += text[i].ToString();
            }
            Debug.Log("speechToTextResult is: " + speechToTextResult);
            analyzer.CheckIfCorrect(text);
        }
        speechToTextResult = text;
        Debug.Log(text);

    }



    // On a fail call to Watson, this is called. Show error in log and show it on screen
    private void OnFail(RESTConnector.Error error, Dictionary<string, object> customData)
    {
        Log.Error("ExampleSpeechToText.OnFail()", "Error received: {0}", error.ToString());
    }

    public void EnableButton()
    {
        //GameObject.Find("RecordButton").SetActive(true);
    }

    public void DisableButton()
    {
        //GameObject.Find("RecordButton").SetActive(false);
    }

}
