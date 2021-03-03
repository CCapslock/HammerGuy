using UnityEngine;
using UnityEngine.UI;

public class RageController : MonoBehaviour
{
	public Slider _slider;
	public Text[] _texts;
	public float RageMaxAmount;
	public float RageCurrentAmount;
	public float HealthMaxAmount;
	public float HealthCurrentAmount;

	private Color _tempColor;
	private float _healthDamage = 0.5f;
	private float _rageDamage = 0.02f;
	private float _maxRecoveryTime = 100f;
	private float _maxTextVisibility = 300f;
	private float _currentTextVisibility;
	private float _currentRecoveryTime;
	[SerializeField] private int EnemyesAttacking;
	[SerializeField] private bool _textIsVisible;
	private void Start()
	{
		HealthCurrentAmount = HealthMaxAmount;
		_slider.maxValue = RageMaxAmount;
		TurnTextInvisible();
	}
	private void FixedUpdate()
	{
		if (EnemyesAttacking > 0)
		{
			_currentRecoveryTime = 0;
			TurnDownRage();
			TurnDownHealth();
			if (!_textIsVisible)
			{
				TurnTextVisible();
			}
		}
		else
		{
			if (_currentRecoveryTime < _maxRecoveryTime)
			{
				_currentRecoveryTime++;
			}
			else
			{
				if (HealthCurrentAmount != HealthMaxAmount)
				{
					TurnUpHealth();
				}
			}
		}
	}

	public void AddRage()
	{
		if (RageCurrentAmount + 5 <= RageMaxAmount)
		{
			RageCurrentAmount += 5;
		}
		_slider.value = RageCurrentAmount;
	}
	public void TurnDownRage()
	{
		if (RageCurrentAmount - _rageDamage * EnemyesAttacking > 0)
		{
			RageCurrentAmount -= _rageDamage * EnemyesAttacking;
			_slider.value = RageCurrentAmount;
		}
	}
	public void TurnDownHealth()
	{
		HealthCurrentAmount -= _healthDamage * EnemyesAttacking;
		for (int i = 0; i < _texts.Length; i++)
		{
			_texts[i].text = Mathf.Floor(HealthCurrentAmount).ToString();
		}
		if(HealthCurrentAmount <= 0)
		{
			EnemyesAttacking = -100;
			TurnTextInvisible();
			FindObjectOfType<PlayerController>().PlayerDead();
			FindObjectOfType<MainGameController>().PlayerLose();
			Destroy(this);
		}
	}
	public void TurnUpHealth()
	{
		if (HealthCurrentAmount + _healthDamage * 2f < HealthMaxAmount)
		{
			HealthCurrentAmount += _healthDamage * 2f;
		}
		else if (HealthCurrentAmount + _healthDamage * 2f >= HealthMaxAmount)
		{
			HealthCurrentAmount = HealthMaxAmount;
			TurnTextInvisible();
		}
		for (int i = 0; i < _texts.Length; i++)
		{
			_texts[i].text = Mathf.Floor(HealthCurrentAmount).ToString();
		}
	}
	public void TurnDownRageALittle()
	{
		RageCurrentAmount -= 0.25f;
		_slider.value = RageCurrentAmount;
	}
	public void AddEnemy()
	{
		EnemyesAttacking++;
	}
	public void RemoveEnemy()
	{
		EnemyesAttacking--;
	}
	public void TurnTextVisible()
	{
		_textIsVisible = true;
		for (int i = 0; i < _texts.Length; i++)
		{
			_tempColor = _texts[i].color;
			_tempColor.a = 1f;
			_texts[i].color = _tempColor;
		}
	}
	public void TurnTextInvisible()
	{
		_textIsVisible = false;
		for (int i = 0; i < _texts.Length; i++)
		{
			_tempColor = _texts[i].color;
			_tempColor.a = 0f;
			_texts[i].color = _tempColor;
		}
	}
}
