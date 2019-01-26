using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MGJW9;
using MGJW9.JobySystem;
using System.Linq;

public class Planet : MonoBehaviour
{

    public List<Ship> inhabitants;

    public float radius = 1f;
    public float orbitRadius = 2f;
    public float orbitSpeed = 1f;

    const float pi2 = 2 * Mathf.PI;
    public MeshRenderer playerOwnerMeshRenderer;

    private void Awake()
    {
        inhabitants = new List<Ship>();
    }

    private void Start()
    {
        GameManager.instance.planets.Add(this);
        playerOwnerMeshRenderer.gameObject.SetActive(false);
    }

    private void OnDrawGizmosSelected()
    {
        float alpha = 0;
        float beta = 0;

        for (int i = 0; i < 100; i++)
        {
            alpha = (float)i / 100f;
            beta = (float)(i+1) / 100f;
            Debug.DrawLine(GetOrbitPosition(alpha, 0), GetOrbitPosition(beta, 0), Color.yellow);
        }

        Debug.DrawLine(transform.position, GetOrbitPosition(0, 0), Color.yellow);
        Debug.DrawLine(transform.position, GetOrbitPosition(0.25f, 0), Color.yellow);
        Debug.DrawLine(transform.position, GetOrbitPosition(0.5f, 0), Color.yellow);
        Debug.DrawLine(transform.position, GetOrbitPosition(0.75f, 0), Color.yellow);
    }

    public Vector3 GetOrbitPosition(float randomValue, float randomValue2)
    {
        var phase = Time.time * orbitSpeed + randomValue * pi2;
        var delta = new Vector3(Mathf.Cos(phase), Mathf.Sin(phase), Mathf.Sin(phase)).normalized;
        return transform.position + orbitRadius * delta * (1f + randomValue2);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var ship = collision.gameObject.GetComponent<Ship>();
        if (ship != null && ship.currentTarget == this)
        {
            inhabitants.Add(ship);
            ComputePlanetOwner();
        }
    }
     
    private void OnTriggerExit2D(Collider2D collision)
    {
        var ship = collision.gameObject.GetComponent<Ship>();
        if (ship != null)
        {
            inhabitants.Remove(ship);
            ComputePlanetOwner();
        }
    }

    private void ComputePlanetOwner() 
    {
        // Do not update owner if less than 5 inhabitants
        if (inhabitants == null || inhabitants.Count() <= 5) {
            return;
        }

        int ownerPlayerId = inhabitants.GroupBy(ship => ship.owner != null ? ship.owner.playerId : 0)
            .OrderBy(group => group.Count())
            .Select(group => group.Key).FirstOrDefault();

        Player player = GameManager.instance.players.SingleOrDefault(p => p.playerId == ownerPlayerId);
        if (player != null) {
            playerOwnerMeshRenderer.gameObject.SetActive(true);
            playerOwnerMeshRenderer.material = player.playerPlanetMaterial;
        }
    }

}
