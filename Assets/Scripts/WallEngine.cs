using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallEngine : MonoBehaviour
{
    ParticleSystem Particle;
    int shuttick;
    bool shutdown;
    GameObject Player;
    void Start()
    {
        Particle = GameObject.Find("DustSystem").GetComponent<ParticleSystem>();
    }
    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            //collider.gameObject.GetComponent<SC_FPSController>().enabled = false;
            Player = collider.gameObject;
            shutdown = true;
        }
            
    }
    void Update()
    {
        if (shutdown)
        {
            if (shuttick == 2)
            {
                Debug.Log("Port Time");
               // Player.gameObject.GetComponent<SC_FPSController>().enabled = false;
                var main = Particle.main;
                main.simulationSpace = ParticleSystemSimulationSpace.Local;
                Player.gameObject.transform.position = StaticSave.Position1;
                GameObject.Find("Camera").gameObject.transform.eulerAngles = StaticSave.RotateZero;
                Player.gameObject.transform.eulerAngles = StaticSave.RotateZero;
                shuttick++;
                
            }
            if (shuttick >= 5)
            {
                var main = Particle.main;
               // Player.GetComponent<SC_FPSController>().enabled = true;
                main.simulationSpace = ParticleSystemSimulationSpace.World;
                shutdown = false;
            }
            else
            {
                shuttick++;
            }
        }
    } 
}
