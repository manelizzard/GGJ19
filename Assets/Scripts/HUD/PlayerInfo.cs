using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfo : MonoBehaviour
{
	public int playerId;

	public Text Name;
	public Text shipsAmount;
	public Text planetsAmount;

	public int rankPosition;

	private void Start()
	{
		PlayersInfo.instance.playerInfo.Add(this);
	}
}
