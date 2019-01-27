using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayersInfo : MonoBehaviour
{
	public static PlayersInfo instance;
	public List<PlayerInfo> playerInfo = new List<PlayerInfo>();
	public GameObject playerInfoPrefab;

	List<int> rankedPlayers = new List<int>();

	private void Awake()
	{
		instance = this;
	}

	[ContextMenu("AddPlayerInfo")]
	public void AddPlayerInfo()
	{
		GameObject playerAdded = Instantiate(playerInfoPrefab);
		playerAdded.transform.SetParent(transform);
		PlayerInfo info = playerInfo.Find(x => x.gameObject == playerAdded);
	}

	public void AddPlayerInfo(Player player)
	{
		GameObject playerAdded = Instantiate(playerInfoPrefab, transform);
		PlayerInfo newInfo = playerAdded.GetComponent<PlayerInfo>();
		playerInfo.Add(newInfo);
		newInfo.player = player;
	}

	public PlayerInfo GetPlayerInfo(int playerId)
	{
		return playerInfo.Find(x => x.playerId == playerId);
	}

}
