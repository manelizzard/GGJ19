using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MGJW9;
using MGJW9.JobySystem;

public class Planet : MonoBehaviour
{

    public List<Ship> inhabitants;

    public float radius = 1f;
    public float orbitRadius = 2f;
    public float orbitSpeed = 1f;

    const float pi2 = 2 * Mathf.PI;

    private void Awake()
    {
        inhabitants = new List<Ship>();
    }

    private void Start()
    {
        GameManager.instance.planets.Add(this);
    }

    private void OnDrawGizmosSelected()
    {
        float alpha = 0;
        float beta = 0;

        for (int i = 0; i < 100; i++)
        {
            alpha = (float)i / 100f;
            beta = (float)(i+1) / 100f;
            Debug.DrawLine(GetOrbitPosition(alpha), GetOrbitPosition(beta), Color.yellow);
        }

        Debug.DrawLine(transform.position, GetOrbitPosition(0), Color.yellow);
        Debug.DrawLine(transform.position, GetOrbitPosition(0.25f), Color.yellow);
        Debug.DrawLine(transform.position, GetOrbitPosition(0.5f), Color.yellow);
        Debug.DrawLine(transform.position, GetOrbitPosition(0.75f), Color.yellow);
    }

    public Vector3 GetOrbitPosition(float randomValue)
    {
        var phase = Time.time * orbitSpeed + randomValue * pi2;
        var delta = new Vector3(Mathf.Cos(phase), Mathf.Sin(phase), Mathf.Sin(phase)).normalized;
        return transform.position + orbitRadius * delta;
    }

    public void ColonizedByPlayer(Player player)
    {
        // TODO: Make shader to identify planet as belonging to the player
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var ship = collision.gameObject.GetComponent<Ship>();
        if (ship != null && ship.currentTarget == this)
        {
            inhabitants.Add(ship);
        }
    }
     
    private void OnTriggerExit2D(Collider2D collision)
    {
        var ship = collision.gameObject.GetComponent<Ship>();
        if (ship != null)
        {
            inhabitants.Remove(ship);
        }
    }

}
