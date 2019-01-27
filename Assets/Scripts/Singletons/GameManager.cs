using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager instance { get { if (instance_ == null) { instance_ = FindObjectOfType<GameManager>(); }; return instance_; } }
	static GameManager instance_;
	public PlayersInfo PlayersInfo { get { return PlayersInfo.instance; } }

    public Material arrowMaterial;
    public float arrowOffsetSpeed = 1f;

    public PlanetCursor cursor;
    public List<Planet> planets;

	[HideInInspector]
	public List<Player> players;

    public Dictionary<int, List<Planet>> playerPlanets;

    public GameObject shipPrefab;
    public GameObject playerPrefab;

	public List<Player> removedPlayers;

    private void Awake()
    {
        playerPlanets = new Dictionary<int, List<Planet>>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            ObjectPool.Spawn(playerPrefab);
        }

        arrowMaterial.SetTextureOffset("_MainTex", new Vector2(-Time.time * arrowOffsetSpeed, 0));
    }
}
