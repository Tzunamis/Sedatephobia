using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallEngine : MonoBehaviour
{
    ParticleSystem Particle;
    GameObject Player;
    GameObject Camera;
    void Start()
    {
        Particle = GameObject.Find("DustSystem").GetComponent<ParticleSystem>();
        Camera = GameObject.Find("Camera");
        Player = GameObject.Find("Player");
    }
    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            Debug.Log("Player Hit Outside Wall");
            NoPlayingAllowed();
            GoWhereWeWantYou();
            OkayYouCanPlay();
        }
            
    }
    void NoPlayingAllowed()
    {
        var main = Particle.main; // I dunno why i need this, the legacy system was way less cringe.
        main.simulationSpace = ParticleSystemSimulationSpace.Local; // Ties all particles to the player so they get ported with them.
        Player.GetComponent<MovementEngine>().controllable = false; // Keeps the movement script from rustling my jimmies
    }
    void GoWhereWeWantYou()
    {
        Camera.transform.eulerAngles = new Vector3(Camera.transform.eulerAngles.x, 0, Camera.transform.eulerAngles.z); // Makes the player face the way we want, (hopefully without them noticing)
        WarpTime();// Decides the player's fate based on "InRoom", which we'll change when they reach the center properly;        
    }
    void OkayYouCanPlay()
    {
        var main = Particle.main; // Still kinda cringe.
        main.simulationSpace = ParticleSystemSimulationSpace.World; // Lets the player move through particles again.
        Player.GetComponent<MovementEngine>().OutsideSync();
        Player.GetComponent<MovementEngine>().MakeOutSideControllable();
    }
    void WarpTime()
    {
        Player.transform.position = Player.GetComponent<StaticSave>().WarpPositions[StaticSave.InRoom];
    }
}
