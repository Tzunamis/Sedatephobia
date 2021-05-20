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
                // GAME STATE
                playerHasEntered = true;
                StaticSave.InRoom++;
                // DIALOGUE
                Debug.Log("Entered Room" + StaticSave.InRoom);
                dialogueManager.EnterSafeZone(StaticSave.InRoom);
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
