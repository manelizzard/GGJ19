using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayersInfo : MonoBehaviour
{
	internal List<Player> players { get { return GameManager.instance.players; } }
}
