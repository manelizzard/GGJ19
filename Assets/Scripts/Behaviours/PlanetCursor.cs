using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetCursor : MonoBehaviour
{
    public float timeToJump;

    private float elapsedTime;
    private ParticleSystem particles;
    public Planet currentFocusedPlanet;

    void Awake() 
    {
        particles = GetComponent<ParticleSystem>();
        particles.Stop();
    }

    void Start() 
    {
        GameManager.instance.cursor = this;
    }
    void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime > timeToJump) {
            if (particles.isStopped) 
            {
                particles.Play();
            }

            elapsedTime = 0;
            // Place cursor at planet position
            currentFocusedPlanet = GameManager.instance.planets[Random.Range(0, GameManager.instance.planets.Count)];
            this.transform.position = currentFocusedPlanet.transform.position;

            Player.playersTargetedToCurrentPlanet = 0;
        }    
    }
}
