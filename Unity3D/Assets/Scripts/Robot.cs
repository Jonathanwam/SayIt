using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * Changes which sprite that the robot is
 * 
 */
public class Robot : MonoBehaviour
{
    public static Image robotImage;

    // set these in inspector
    public Sprite goodJob;
    public Sprite didNotHear;
    public Sprite wrongAnswer;
    public Sprite neutral;

    // Start is called before the first frame update
    void Start()
    {
        robotImage = GetComponent<Image>();
    }
    // Call when we have a correct result
    public void UpdateToGoodJob()
    {
        robotImage.sprite = goodJob;
        //yield return new WaitForSeconds(5);
        //this.UpdateToNeutral();
    }
    // Call when robot did not hear anything
    public void UpdateToDidNotHear()
    {
        robotImage.sprite = didNotHear;
        //yield return new WaitForSeconds(5);
        //this.UpdateToNeutral();

    }
    // Call when robot got a wrong result
    public void UpdateToWrong()
    {
        robotImage.sprite = wrongAnswer;
        //yield return new WaitForSeconds(5);
        //this.UpdateToNeutral();
    }
    // Call when robot goes back to neutral state
    public void UpdateToNeutral()
    {
        robotImage.sprite = neutral;

    }
}
