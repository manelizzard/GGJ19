using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverPanel : MonoBehaviour
{
	public static GameOverPanel instance { get { if (instance_ == null) { instance_ = FindObjectOfType<GameOverPanel>(); }; return instance_; } }
	static GameOverPanel instance_;

	public Text winnerText;
	public Animator anim;

	string winnerName;

	public void GameOver()
	{
		winnerName = GameManager.instance.players.Find(x => x.removed == false).displayName;
		winnerText.text = winnerName + " has kicked you asses";
		Time.timeScale = 0.2f;
		Debug.Log("Game Over");
	}

	public void Replay()
	{
		//Restart Game
	}
}
