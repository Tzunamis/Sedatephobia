using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeZoneTrigger : MonoBehaviour
{
    CapsuleCollider triggerZone;
    AudioSource sound;
    DialogueManager dialogueManager;
    bool playerHasEntered = false;

    // Start is called before the first frame update
    void Awake()
    {
        dialogueManager = GameObject.Find("DialogueManager").GetComponent<DialogueManager>();
        if (gameObject.GetComponent<CapsuleCollider>() != null)
            triggerZone = gameObject.GetComponent<CapsuleCollider>();
        if (gameObject.GetComponent<AudioSource>() != null)
            sound = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Player")
        {
            // AUDIO
            // stop darkness sounds
            sound.Play();

            if(!playerHasEntered)
            {
                // Game state
                playerHasEntered = true;
                StaticSave.InRoom++;
                // Dialogue
                dialogueManager.EnterSafeZone(StaticSave.InRoom - 1, sound);
            }

        }
    }

    private void OnTriggerExit(Collider col)
    {
        if(col.tag == "Player")
        {
            sound.Stop();
            //start darkness sounds
        }
    }
}
