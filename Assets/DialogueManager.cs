using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    // TEXT BOXES
    public Text bottomText;
    public Text topLeftText;
    public Text midLeftText;
    public Text topRightText;
    public Text midRightText;

    // DIALOGUE
    // Dialogue for safe areas that is essential for the player to see 
    string[] narrativeDialogue;
    // Dialogue for darkness, which will likely be randomized
    string[] fillerDialogueBottom; // "Grounded" thoughts that appear in the bottom of the screen
    string[] fillerDialogueOther; // "Intrusive" thoughts that appear all over the screen
    
    // Variables below allow us to randomize position of "intrusive" thoughts within a range
    int minPosX = 0;
    int maxPosX = 0;
    int minPosY = 0;
    int maxPosY = 0;


    // PLAYER STATUS
    // These will be tracked using trigger zones
    bool isSafe = true; // Tracks whether player is in a safe zone or in darkness
    int location = 0; // Tracks which location the player is in (or last visited)



    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            int textToChange = Random.Range(0, 5);
            Text currentTextBox = bottomText;
            switch(textToChange)
            {
                case 0: // Bottom
                    currentTextBox = bottomText;
                    break;
                case 1: // TopLeft
                    currentTextBox = topLeftText;
                    break;
                case 2: // MidLeft
                    currentTextBox = midLeftText;
                    break;
                case 3: // TopRight
                    currentTextBox = topRightText;
                    break;
                case 4: // MidRight
                    currentTextBox = midRightText;
                    break;

            }
            RandomizePosition(currentTextBox, textToChange);
            DisplayText(currentTextBox, "Nyeheheh");
        }
    }

    void RandomizePosition(Text textBox, int textBoxID)
    {
        switch(textBoxID)
        {
            case 0: // Bottom
                minPosX = 0;
                maxPosX = 0;
                minPosY = 0;
                maxPosY = 0;
                break;
            case 1: // TopLeft
                minPosX = 100;
                maxPosX = 400;
                minPosY = -600;
                maxPosY = -100;
                break;
            case 2: // MidLeft
                minPosX = 100;
                maxPosX = 400;
                minPosY = -500;
                maxPosY = 0;
                break;
            case 3: // TopRight
                minPosX = -1200;
                maxPosX = -900;
                minPosY = -600;
                maxPosY = -100;
                break;
            case 4: // MidRight
                minPosX = -1500;
                maxPosX = -1200;
                minPosY = -500;
                maxPosY = 0;
                break;
        }
        float posX = Random.Range(minPosX, maxPosX);
        float posY = Random.Range(minPosY, maxPosY);
        Debug.Log("X: " + posX + " Y: " + posY);
        textBox.GetComponent<RectTransform>().anchoredPosition = new Vector3(posX, posY, 0);
    }

    void DisplayText(Text textBox, string dialogue)
    {
        textBox.text = dialogue;
    }
}
