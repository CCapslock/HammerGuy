using UnityEngine;
using UnityEngine.SceneManagement;

public class MainGameController : MonoBehaviour
{
	private HammerController _hammerController;
	[SerializeField] private int _amountOfEnemyes;
	private bool _isPhaseOne = true;
	private void Start()
	{
		_hammerController = FindObjectOfType<HammerController>();
	}
	public void AddEnemyToCounter(int numOfEnemyes)
	{
		_amountOfEnemyes += numOfEnemyes;
	}
	public void DeleteEnemyFromCounter()
	{
		_amountOfEnemyes--;
	}
	public void CheckIfAllEnemyesDefeated()
	{
		if (_isPhaseOne)
		{
			if (_amountOfEnemyes <= 0)
			{
				_isPhaseOne = false;
				Invoke("MakeHammerBigger", 1f);
			}
		}
		else
		{
			if (_amountOfEnemyes <= 0)
			{
				EndRound();
			}
		}
	}

	private void MakeHammerBigger()
	{
		_hammerController.MakeHammerBigger();
	}
	private void EndRound()
	{
		_hammerController.PlayEndGameAnimations();
	}
	public void StartNextLvl()
	{
		_hammerController.BreakFloor();
		Invoke("RestartScene", 4f);
	}
	public void PlayerLose()
	{
		Invoke("RestartScene", 4f);
	}
	private void RestartScene()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
}
