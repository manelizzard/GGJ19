using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfo : MonoBehaviour
{
	public int playerId;
	[HideInInspector]
	public Player player;

	internal string displayName { get { return player.displayName; } }
	internal int shipsCount { get { return player.ships.Count; } }
	internal int planetsCount { get { return GameManager.instance.playerPlanets[playerId].Count; } }
	internal Color playerColor { get { return player.color; } }

	public Image background;
	public Text Name;
	public Text shipsCountText;
	public Text planetsCountText;

	public int rankPosition;

	private void Start()
	{
		rankPosition = transform.GetSiblingIndex();
		PlayersInfo.instance.playerInfo.Add(this);
		SetNameAndColor();
	}

	public void SetNameAndColor()
	{
		Name.text = displayName;
		background.color = playerColor;
	}

	public void PrintShipCount()
	{
		shipsCountText.text = "Naves: " + shipsCount.ToString();
	}

	public void PrintPlanetsCount()
	{
		planetsCountText.text = "Planetas: " + planetsCount.ToString();

	}

	public void SetRankPosition()
	{
		PlayerInfo nearPlayer = PlayersInfo.instance.playerInfo.Find(x => x.planetsCount < planetsCount);

		int newIndex = PlayersInfo.instance.playerInfo.FindIndex(x => x == nearPlayer);
		int oldIndex = PlayersInfo.instance.playerInfo.FindIndex(x => x == this);

		PlayersInfo.instance.playerInfo[newIndex] = this;
		PlayersInfo.instance.playerInfo[oldIndex] = nearPlayer;

		transform.SetSiblingIndex(newIndex);
	}
}
