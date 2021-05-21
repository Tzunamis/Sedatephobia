using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    // This script manages more than just dialogue, heh. A little messy.

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
    bool playingNarrativeDialogue = true;
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
    float intrusiveThoughtsTimer = 0; // Tracks time since last intrusive thought

    // AUDIO
    AudioManager audioManager;
    int windDelay;
    int heartbeatDelay;
    int breathingDelay;
    int fastBreathingDelay;
    int eyeballDelay;
    float intrusiveThoughtsDelay;
    float intrusiveThoughtsDelayModifier;
    int minimumIntrusiveThoughtsDelay;
    AudioSource safezoneSound;

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

        // Set initial darkness trigger delay values
        windDelay = 0;
        heartbeatDelay = 30;
        breathingDelay = 50;
        fastBreathingDelay = 70;
        eyeballDelay = 100;
        intrusiveThoughtsDelay = 1000;
        intrusiveThoughtsDelayModifier = 1.5f;
        minimumIntrusiveThoughtsDelay = 20;

        // Start room 1 dialogue
        DisplayNarrativeDialogue();
    }

    // Update is called once per frame
    void Update()
    {
        if (isSafe)
        {
            //FADE OUT DARKNESS SOUNDS

            // Wind 1
            if (audioManager.sounds[2].source.isPlaying)
            {
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
            // Increment timers
            darknessTimer += Time.deltaTime;
            intrusiveThoughtsTimer += Time.deltaTime;
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
            if(darknessTimer > heartbeatDelay)
            {
                if (!audioManager.sounds[5].source.isPlaying)
                {
                    audioManager.Play("Heartbeat");
                    audioManager.FadeTheSound("Heartbeat", true);
                }
            }
            if(darknessTimer > breathingDelay)
            {
                if (!audioManager.sounds[6].source.isPlaying)
                {
                    audioManager.Play("Breathing");
                    audioManager.FadeTheSound("Breathing", true);
                    if(darknessTimer < fastBreathingDelay)
                        audioManager.sounds[6].source.pitch = 0.9f;
                }
            }
            
            if(darknessTimer > fastBreathingDelay)
            {
                if(audioManager.sounds[6].source.isPlaying && audioManager.sounds[6].source.pitch < 1)
                {
                    audioManager.sounds[6].source.pitch += 0.0005f;
                }
            }

            // EYEBALL STUFF
            if(darknessTimer > eyeballDelay)
            {
                // SPAWN EYEBALL
            }

            // Intrusive thought stuff
            if(intrusiveThoughtsTimer > intrusiveThoughtsDelay)
            {
                // Generate intrusive thoughts
                DisplayIntrusiveThought();
                // Manage intrusive thoughts timer
                intrusiveThoughtsTimer = 0;
                intrusiveThoughtsDelay -= intrusiveThoughtsDelayModifier;
                if (intrusiveThoughtsDelay < minimumIntrusiveThoughtsDelay)
                    intrusiveThoughtsDelay = minimumIntrusiveThoughtsDelay;
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

        // Set text box according to location, and prepare to wipe text
        switch (dialogueLocation)
        {
            case 1:
                currentTextBox = topLeftText;
                Invoke("KillTopLeftText", 5);
                break;
            case 2:
                currentTextBox = midLeftText;
                Invoke("KillMidLeftText", 5);
                break;
            case 3:
                currentTextBox = topRightText;
                Invoke("KillTopRightText", 5);
                break;
            case 4:
                currentTextBox = midRightText;
                Invoke("KillMidRightText", 5);
                break;
        }

        // Check if intrusive thoughts list needs refreshing
        if (intrusiveThoughts.Count == 0)
        {
            RefreshIntrusiveThoughts();
        }

        // Randomize dialogue
        int dialogueIndex = Random.Range(1, intrusiveThoughts.Count + 1);
        currentIntrusiveDialogue = (string)intrusiveThoughts[dialogueIndex - 1];
        // Remove selected dialogue
        intrusiveThoughts.RemoveAt(dialogueIndex - 1);

        
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
        while(currentNarrativeDialogue.safeZoneID < safeZoneID)
        {
            //Debug.Log("current dialogue: " + narrativeDialogueBank[currentNarrativeDialogueID].text);
            //Debug.Log("current dialogue ID: " + currentNarrativeDialogueID);
            //Debug.Log("safezoneID in dialogue array: " + currentNarrativeDialogue.safeZoneID);
            //Debug.Log("safezoneID in dialogue manager: " + safeZoneID);
            Debug.Log("skipping dialogue");

            currentNarrativeDialogue = narrativeDialogueBank[currentNarrativeDialogueID + 1];
            currentNarrativeDialogueID++;
        }
        //Debug.Log("current dialogue: " + narrativeDialogueBank[currentNarrativeDialogueID].text);
        //Debug.Log("current dialogue ID: " + currentNarrativeDialogueID);

        // Check if dialogue triggers safezone "destruction"
        int[] darknessTriggers = new int[] {1, 19, 30, 37, 48, 54, 62, 69, 77};
        for(int i = 0; i < darknessTriggers.Length; i++)
        {
            if (currentNarrativeDialogueID == darknessTriggers[i])
            {
                // Trigger darkness
                ExitSafeZone(safezoneSound);
                Debug.Log("currentsafezone: " + currentNarrativeDialogue.safeZoneID);
                GameObject.Find("Room" + currentNarrativeDialogue.safeZoneID + "Spot").GetComponent<Light>().enabled = false;
                audioManager.Play("LightsOut");
            }

            // Initial darkness
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
            playingNarrativeDialogue = false;
            StartCoroutine(FadeText(bottomText, false));
            ClearPreviousText();
            return;
        }
        // Change text to current line
        bottomText.text = currentNarrativeDialogue.text;
        if(firstNarrativeLine && currentNarrativeDialogueID != 0)
        {
            StartCoroutine(FadeText(bottomText, true));
            StartCoroutine(FadeText(previousText, true));
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

        Debug.Log("duration: " + narrativeDialogueBank[currentNarrativeDialogueID - 1].duration);

        // Invoke this function after dialogue duration
        Invoke("DisplayNarrativeDialogue", narrativeDialogueBank[currentNarrativeDialogueID - 1].duration);
    }

    public void ClearPreviousText()
    {
        StartCoroutine(FadeText(previousText, false));
    }

    public void EnterSafeZone(int safeZone, AudioSource objectSound)
    {
        if(safeZone != safeZoneID) // If the player has entered a new safe zone
        {
            isSafe = true; // Player is safe
            safeZoneID = safeZone; // Set new safezone
            // Intrusive thoughts delay reduced in each zone
            intrusiveThoughtsDelay = 20 - safeZoneID;
            intrusiveThoughtsDelayModifier = 1.5f + safeZoneID * 0.2f;
            minimumIntrusiveThoughtsDelay = 20 - safeZoneID * 2;
            if (minimumIntrusiveThoughtsDelay < 2)
                minimumIntrusiveThoughtsDelay = 2;

            // Trigger first dialogue of area
            if(!playingNarrativeDialogue)
            {
                firstNarrativeLine = true;
                playingNarrativeDialogue = true;
                DisplayNarrativeDialogue();
            }

            safezoneSound = objectSound;

            // HIDE EYEBALL
            
        }
               
    }

    void ExitSafeZone(AudioSource objectSound)
    {
        darknessTimer = 0; // Reset timer
        intrusiveThoughtsTimer = 0;
        isSafe = false;
        //Reduce darkness sound cooldowns
        heartbeatDelay -= 2;
        breathingDelay -= 4;
        fastBreathingDelay -= 4;
        eyeballDelay -= 4;
        if(safezoneSound != null)
            safezoneSound.Stop();
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

    void KillTopLeftText()
    {
        topLeftText.text = "";
    }

    void KillMidLeftText()
    {
        midLeftText.text = "";
    }

    void KillTopRightText()
    {
        topRightText.text = "";
    }

    void KillMidRightText()
    {
        midRightText.text = "";
    }
}
