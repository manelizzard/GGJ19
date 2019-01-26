using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayersInfo : MonoBehaviour
{
	public static PlayersInfo instance;
	internal List<Player> players { get { return GameManager.instance.players; } }
	public List<PlayerInfo> playerInfo = new List<PlayerInfo>();
	public GameObject playerInfoPrefab;

	private void Awake()
	{
		instance = this;
	}

	public void AddPlayerInfo(Player player)
	{
		GameObject playerAdded = Instantiate(playerInfoPrefab);
		playerAdded.transform.SetParent(transform);
		PlayerInfo info = playerInfo.Find(x => x.gameObject == playerAdded);
		info.playerId = player.playerId;
	}
}
