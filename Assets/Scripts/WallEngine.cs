using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallEngine : MonoBehaviour
{
    ParticleSystem Particle;
    void Start()
    {
        Particle = GameObject.Find("DustSystem").GetComponent<ParticleSystem>();
    }
    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            Debug.Log("Port Time");
            collider.gameObject.GetComponent<SC_FPSController>().enabled = false;
            var main = Particle.main;
            main.simulationSpace = ParticleSystemSimulationSpace.Local;
            collider.gameObject.transform.position = StaticSave.Position1;
            GameObject.Find("Camera").transform.rotation = StaticSave.RotateZero;
            collider.gameObject.transform.rotation = StaticSave.RotateZero;
            main.simulationSpace = ParticleSystemSimulationSpace.World;
            collider.gameObject.GetComponent<SC_FPSController>().enabled = true;
        }
    }
}
