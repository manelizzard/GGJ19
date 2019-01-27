using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverPanel : MonoBehaviour
{
	public static GameOverPanel instance { get { if (instance_ == null) { instance_ = FindObjectOfType<GameOverPanel>(); }; return instance_; } }
	static GameOverPanel instance_;

	public Text winnerText;
	public Animator anim;

	string winnerName;
	float timeScale;

	public void GameOver()
	{
		winnerName = GameManager.instance.players.Find(x => x.removed == false).displayName;
		winnerText.text = winnerName + " has kicked your asses";
		timeScale = Time.timeScale;
		Time.timeScale = 0.2f;
		anim.SetTrigger("GameOver");
	}

	public void Replay()
	{
		//Restart Game
		Time.timeScale = timeScale;
		SceneManager.LoadScene("SampleJesus");
	}
}
