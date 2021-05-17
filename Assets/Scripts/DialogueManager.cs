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
    TextMeshProUGUI previousText;
    TextMeshProUGUI titleText;

    TextMeshProUGUI currentTextBox;

    // DIALOGUE

    Dialogue currentNarrativeDialogue; // Current narrative dialogue
    int currentNarrativeDialogueID;
    bool firstNarrativeLine = true;
    bool lastNarrativeLine = false;
    string currentIntrusiveDialogue; // Intrusive dialogue to be printed
    int dialogueLocation; // Location of intrusive thought
    int previousDialogueLocation; // Tracks location of previous intrusive thought
    float fadeTime; // Duration of text fade

    // Dialogue for safe areas that is essential for the player to see 
    public Dialogue[] narrativeDialogueBank;
    // Dialogue for darkness, which will likely be randomized
    string[] groundedThoughtsBank; // "Grounded" thoughts that appear on the bottom of the screen
    public string[] intrusiveThoughtsBank; // "Intrusive" thoughts that appear all over the screen
    ArrayList intrusiveThoughts = new ArrayList();

    // Variables below allow us to randomize position of "intrusive" thoughts within a range
    int minPosX = 0;
    int maxPosX = 0;
    int minPosY = 0;
    int maxPosY = 0;

    // PLAYER STATUS
    // These will be tracked using trigger zones
    bool isSafe = true; // Tracks whether player is in a safe zone or in darkness
    [HideInInspector]
    int safeZoneID = 1; // Tracks which location the player is in (or last visited)
    float darknessTimer = 0; // Tracks time since player entered darkness

    // AUDIO
    AudioManager audioManager;
    int windDelay;
    int heartbeatDelay;
    int breathingDelay;

    // Start is called before the first frame update
    void Start()
    {
        // Assign variables
        bottomText = GameObject.Find("Text_Bottom").GetComponent<TextMeshProUGUI>();
        topLeftText = GameObject.Find("Text_TopLeft").GetComponent<TextMeshProUGUI>();
        midLeftText = GameObject.Find("Text_MidLeft").GetComponent<TextMeshProUGUI>();
        topRightText = GameObject.Find("Text_TopRight").GetComponent<TextMeshProUGUI>();
        midRightText = GameObject.Find("Text_MidRight").GetComponent<TextMeshProUGUI>();
        previousText = GameObject.Find("Text_Previous").GetComponent<TextMeshProUGUI>();
        titleText = GameObject.Find("Text_Title").GetComponent<TextMeshProUGUI>();
        currentNarrativeDialogue = narrativeDialogueBank[0];
        currentNarrativeDialogueID = 0;
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();

        // Fill intrusive thoughts arraylist
        RefreshIntrusiveThoughts();

        // Empty UI boxes
        bottomText.text = "";
        topLeftText.text = "";
        midLeftText.text = "";
        topRightText.text = "";
        midRightText.text = "";
        previousText.text = "";
        titleText.color = new Color(titleText.color.r, titleText.color.g, titleText.color.b, 0);

        // Display title
        Invoke("DisplayTitleCard", 5);

        // Set initial sound delay values
        windDelay = 0;
        heartbeatDelay = 20;
        breathingDelay = 40;

        // Start room 1 dialogue
        DisplayNarrativeDialogue();
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
            //FADE OUT DARKNESS SOUNDS

            // Wind 1
            if (audioManager.sounds[2].source.isPlaying)
            {
                Debug.Log("Fadeout wind");
                audioManager.FadeTheSound("Wind1", false);
            }

            // Wind 2
            if (audioManager.sounds[3].source.isPlaying)
            {
                audioManager.FadeTheSound("Wind2", false);
            }

            // Heartbeat
            if (audioManager.sounds[5].source.isPlaying)
            {
                audioManager.FadeTheSound("Heartbeat", false);
            }

            // Breathing
            if (audioManager.sounds[6].source.isPlaying)
            {
                audioManager.FadeTheSound("Breathing", false);
            }
        }

        else
        {
            darknessTimer += Time.deltaTime;
            // Manage darkness audio

            if (darknessTimer > windDelay)
            {
                if (!audioManager.sounds[2].source.isPlaying)
                {
                    audioManager.Play("Wind1");
                    audioManager.FadeTheSound("Wind1", true);
                }
                if (!audioManager.sounds[3].source.isPlaying)
                {
                    audioManager.Play("Wind2");
                    audioManager.FadeTheSound("Wind2", true);
                }
            }
            
            if(darknessTimer > breathingDelay)
            {
                if (!audioManager.sounds[5].source.isPlaying)
                {
                    audioManager.Play("Heartbeat");
                    audioManager.FadeTheSound("Heartbeat", true);
                }
            }
            if(darknessTimer > heartbeatDelay)
            {
                if (!audioManager.sounds[6].source.isPlaying)
                {
                    audioManager.Play("Breathing");
                    audioManager.FadeTheSound("Breathing", true);
                }
            }

        }
    }

    void DisplayTitleCard()
    {
        fadeTime = 10;
        StartCoroutine(FadeText(titleText, true));
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
        currentIntrusiveDialogue = (string)intrusiveThoughts[dialogueIndex - 1];
        // Remove selected dialogue
        intrusiveThoughts.RemoveAt(dialogueIndex - 1);

        // Check if intrusive thoughts list needs refreshing
        if (intrusiveThoughts.Count == 0)
        {
            RefreshIntrusiveThoughts();
        }
        // Display dialogue
        RandomizeTextPosition(currentTextBox, dialogueLocation);
        DisplayIntrusiveDialogue(currentTextBox, currentIntrusiveDialogue, 3, true);
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

    void DisplayIntrusiveDialogue(TextMeshProUGUI textBox, string dialogue, float duration, bool fadeIn)
    {
        textBox.text = dialogue;
        StartCoroutine(FadeText(textBox, true));
    }

    void RefreshIntrusiveThoughts()
    {
        for (int i = 0; i < intrusiveThoughtsBank.Length; i++)
        {
            intrusiveThoughts.Add(intrusiveThoughtsBank[i]);
        }
    }

    void DisplayNarrativeDialogue()
    {
        
        // Check if dialogue triggers safezone "destruction"
        
        int[] darknessTriggers = new int[] {1, 10, 15, 20, 25};
        for(int i = 0; i < darknessTriggers.Length; i++)
        {
            if (currentNarrativeDialogueID == darknessTriggers[i])
            {
                // Trigger darkness
                ExitSafeZone();
                GameObject.Find("Room" + currentNarrativeDialogueID + "Spot").GetComponent<Light>().enabled = false;
                
            }

            if(currentNarrativeDialogueID == 1)
            {
                GameObject.Find("Camera").GetComponent<AudioSource>().Stop();
                titleText.text = "";
            }
        }
        

        // If last line was just displayed, fade out and stop displaying dialogue
        if(lastNarrativeLine)
        {
            lastNarrativeLine = false;
            StartCoroutine(FadeText(bottomText, false));
            Invoke("ClearPreviousText", currentNarrativeDialogue.duration);
            return;
        }
        // Change text to current line
        bottomText.text = currentNarrativeDialogue.text;
        if(firstNarrativeLine && currentNarrativeDialogueID != 0)
        {
            StartCoroutine(FadeText(bottomText, true));
            firstNarrativeLine = false;
        }
        else if (currentNarrativeDialogueID != 0)
        {
            previousText.text = narrativeDialogueBank[currentNarrativeDialogueID - 1].text;
        }
        // Change to next line
        currentNarrativeDialogueID++;
        currentNarrativeDialogue = narrativeDialogueBank[currentNarrativeDialogueID];

        // Check if last line was just displayed
        if (currentNarrativeDialogue.safeZoneID != safeZoneID)
        {
            lastNarrativeLine = true;
            
        }

        // Invoke this function after dialogue duration
        Invoke("DisplayNarrativeDialogue", narrativeDialogueBank[currentNarrativeDialogueID - 1].duration);
    }

    public void ClearPreviousText()
    {
        StartCoroutine(FadeText(previousText, false));
    }

    public void EnterSafeZone(int safeZone)
    {
        if(safeZone != safeZoneID) // If the player has entered a new safe zone
        {
            isSafe = true; // Player is safe
            safeZoneID = safeZone; // Set new safezone
            // Trigger first dialogue of area
            firstNarrativeLine = true;
            DisplayNarrativeDialogue();
        }
               
    }

    void ExitSafeZone()
    {
        darknessTimer = 0; // Reset timer
        isSafe = false;
        //Reduce darkness sound cooldowns
        heartbeatDelay -= 2;
        breathingDelay -= 4;
    }

    IEnumerator FadeText(TextMeshProUGUI textBox, bool fadeIn)
    {
        if (fadeIn)
        {
            textBox.color = new Color(textBox.color.r, textBox.color.g, textBox.color.b, 0);
            while (textBox.color.a < 1.0f)
            {
                textBox.color = new Color(textBox.color.r, textBox.color.g, textBox.color.b, textBox.color.a + (Time.deltaTime / fadeTime));
                yield return null;
            }
        }
        else
        {
            textBox.color = new Color(textBox.color.r, textBox.color.g, textBox.color.b, 1);
            while (textBox.color.a > 0.0f)
            {
                textBox.color = new Color(textBox.color.r, textBox.color.g, textBox.color.b, textBox.color.a - (Time.deltaTime / fadeTime));
                yield return null;
            }
        }
        fadeTime = 2;
    }
}
