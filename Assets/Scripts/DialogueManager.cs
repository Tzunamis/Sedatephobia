using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    // TEXT BOXES
    TextMeshProUGUI bottomText;
    TextMeshProUGUI topLeftText;
    TextMeshProUGUI midLeftText;
    TextMeshProUGUI topRightText;
    TextMeshProUGUI midRightText;

    TextMeshProUGUI currentTextBox;

    // DIALOGUE
    // Dialogue for safe areas that is essential for the player to see 
    string[] narrativeDialogue;
    // Dialogue for darkness, which will likely be randomized
    string[] groundedThoughtsBank; // "Grounded" thoughts that appear on the bottom of the screen
    string[] intrusiveThoughtsBank; // "Intrusive" thoughts that appear all over the screen
    ArrayList intrusiveThoughts = new ArrayList();

    // Variables below allow us to randomize position of "intrusive" thoughts within a range
    int minPosX = 0;
    int maxPosX = 0;
    int minPosY = 0;
    int maxPosY = 0;

    string currentDialogue; // Dialogue to be printed
    int dialogueLocation; // Location of intrusive thought
    int previousDialogueLocation; // Tracks location of previous intrusive thought

    // PLAYER STATUS
    // These will be tracked using trigger zones
    bool isSafe = true; // Tracks whether player is in a safe zone or in darkness
    int safeZoneID = 0; // Tracks which location the player is in (or last visited)
    float safeZoneTimer = 0; // Tracks time since player entered current safezone
    float darknessTimer = 0; // Tracks time since player entered darkness

    // Start is called before the first frame update
    void Start()
    {
        // Assign variables
        bottomText = GameObject.Find("Text_Bottom").GetComponent<TextMeshProUGUI>();
        topLeftText = GameObject.Find("Text_TopLeft").GetComponent<TextMeshProUGUI>();
        midLeftText = GameObject.Find("Text_MidLeft").GetComponent<TextMeshProUGUI>();
        topRightText = GameObject.Find("Text_TopRight").GetComponent<TextMeshProUGUI>();
        midRightText = GameObject.Find("Text_MidRight").GetComponent<TextMeshProUGUI>();

        // Build intrusive thoughts bank
        BuildIntrusiveThoughtsBank();

        // Fill intrusive thoughts arraylist
        RefreshIntrusiveThoughts();

    }

    // Update is called once per frame
    void Update()
    {
        // For testing purposes
        if(Input.GetKeyDown(KeyCode.T))
        {
            DisplayIntrusiveThought();
        }
        

        if (isSafe)
        {
            switch(safeZoneID)
            {
                case 0:
                    // STARTING ROOM DIALOGUE
                    break;
                case 1:
                    // ROOM 1 DIALOGUE
                    break;
                case 2:
                    // ROOM 2 DIALOGUE
                    break;
                case 3:
                    // ROOM 3 DIALOGUE
                    break;
                case 4:
                    // ROOM 4 DIALOGUE
                    break;
                case 5:
                    // ROOM 5 DIALOGUE
                    break;
                case 6:
                    // ROOM 6 DIALOGUE
                    break;
                case 7:
                    // CABIN DIALOGUE
                    break;
            }

            safeZoneTimer += Time.deltaTime;
        }

        else
        {

            darknessTimer += Time.deltaTime;
        }
    }

    void DisplayIntrusiveThought()
    {
        // Randomize location (never the same location twice in a row)
        do
        {
            dialogueLocation = Random.Range(1, 5);
        } while (dialogueLocation == previousDialogueLocation);
        previousDialogueLocation = dialogueLocation;

        // Set text box according to location
        switch (dialogueLocation)
        {
            case 1:
                currentTextBox = topLeftText;
                break;
            case 2:
                currentTextBox = midLeftText;
                break;
            case 3:
                currentTextBox = topRightText;
                break;
            case 4:
                currentTextBox = midRightText;
                break;
        }

        // Randomize dialogue
        int dialogueIndex = Random.Range(1, intrusiveThoughts.Count + 1);
        currentDialogue = (string)intrusiveThoughts[dialogueIndex - 1];
        // Remove selected dialogue
        intrusiveThoughts.RemoveAt(dialogueIndex - 1);

        // Check if intrusive thoughts list needs refreshing
        if (intrusiveThoughts.Count == 0)
        {
            RefreshIntrusiveThoughts();
        }
        // Display dialogue
        RandomizeTextPosition(currentTextBox, dialogueLocation);
        DisplayText(currentTextBox, currentDialogue);
    }

    void RandomizeTextPosition(TextMeshProUGUI textBox, int textBoxID)
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
                minPosX = -1200;
                maxPosX = -900;
                minPosY = -500;
                maxPosY = 0;
                break;
        }
        float posX = Random.Range(minPosX, maxPosX);
        float posY = Random.Range(minPosY, maxPosY);
        textBox.GetComponent<RectTransform>().anchoredPosition = new Vector3(posX, posY, 0);
    }

    void DisplayText(TextMeshProUGUI textBox, string dialogue)
    {
        textBox.text = dialogue;
    }

    void EnterSafeZone(int safeZone)
    {
        if(safeZone != safeZoneID) // If the player has entered a new safe zone
        {
            safeZoneTimer = 0; // Reset timer to allow for new dialogue
            safeZoneID = safeZone; // Set new safezone
        }
        
        isSafe = true;

        // TRIGGER SAFEZONE AUDIO
    }

    void ExitSafeZone()
    {
        darknessTimer = 0; // Reset timer
        isSafe = false;

        // TRIGGER DARKNESS AUDIO
    }

    void BuildIntrusiveThoughtsBank()
    {
        intrusiveThoughtsBank = new string[5];
        intrusiveThoughtsBank[0] = "You're worthless";
        intrusiveThoughtsBank[1] = "Nobody likes you";
        intrusiveThoughtsBank[2] = "Jump off a bridge";
        intrusiveThoughtsBank[3] = "Stupid piece of shit";
        intrusiveThoughtsBank[4] = "Ur mom gay";
    }

    void RefreshIntrusiveThoughts()
    {
        for (int i = 0; i < intrusiveThoughtsBank.Length; i++)
        {
            intrusiveThoughts.Add(intrusiveThoughtsBank[i]);
        }
    }
}
