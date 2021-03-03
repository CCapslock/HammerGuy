using System.Globalization;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
	public GameObject NormalEnemy;
	public GameObject SplashObject;
	public GameObject[] NormalEnemyes;
	public Rigidbody HipsRigidBody;
	public ParticleSystem DeathParticles;
	public ParticleSystem PlayerDeathParticles;
	public ParticleSystem CoinParticles;
	public float EnemySpeed;
	public float AttackDistance;
	public float TimeOfKnockout;
	public float LenghtOfKickUpAnimation;
	public float ForceOfTackle;
	public bool IsBigEnemy;

	private MainGameController _mainGameController;
	private Rigidbody[] _ragdollRigidBodyes;
	private Rigidbody _capsuleRigidBody;
	private Collider[] _ragdollColliders;
	private CapsuleCollider _capsuleCollider;
	private Animator _enemyAnimator;
	private Transform _playerTransform;
	private Transform _hammerCenterTransform;
	private RageController _rageController;
	private float _distanceVector;
	private bool _usingRagdoll;
	private bool _isRunning;
	private bool _inKnockout;
	private bool _isBroken;//для того что бы проверять был ли BigEnemy разделен перед уничтоженеим
	private bool _isActive;
	private bool _isAttacking;
	private bool _needToCalculateDistance;

	private void Awake()
	{
		DeathParticles.Pause();
		DeathParticles.Clear();
		if (CoinParticles != null)
		{
			CoinParticles.Pause();
			CoinParticles.Clear();
		}
		PlayerDeathParticles.Stop();
		PlayerDeathParticles.Clear();
		_enemyAnimator = GetComponent<Animator>();
		_capsuleCollider = GetComponent<CapsuleCollider>();
		_capsuleRigidBody = GetComponent<Rigidbody>();
		_ragdollColliders = GetComponentsInChildren<Collider>();
		_ragdollRigidBodyes = GetComponentsInChildren<Rigidbody>();
		TurnOffRagdoll();
		switch (Random.Range(1, 4))
		{
			case 1:
				_enemyAnimator.SetBool("Run1", true);
				break;
			case 2:
				_enemyAnimator.SetBool("Run2", true);
				break;
			case 3:
				_enemyAnimator.SetBool("Run3", true);
				break;
		}
		_isRunning = true;
	}
	public void SetParameters(MainGameController mainGameController, Transform playerTransform, Transform hammerCenterTransform, RageController rageController)
	{
		_rageController = rageController;
		_playerTransform = playerTransform;
		_mainGameController = mainGameController;
		_hammerCenterTransform = hammerCenterTransform;
		if (IsBigEnemy)
		{
			for (int i = 0; i < NormalEnemyes.Length; i++)
			{
				NormalEnemyes[i].GetComponent<EnemyController>().SetParameters(mainGameController, playerTransform, hammerCenterTransform, rageController);
			}
		}
		_isActive = true;
	}
	private void TurnOffRagdoll()
	{
		_inKnockout = false;
		transform.position = HipsRigidBody.transform.position;
		_usingRagdoll = false;
		_capsuleCollider.isTrigger = false;
		for (int i = 0; i < _ragdollColliders.Length; i++)
		{
			if (_ragdollColliders[i] != _capsuleCollider)
			{
				_ragdollColliders[i].isTrigger = true;
			}
			if (_ragdollRigidBodyes[i] != _capsuleRigidBody)
			{
				_ragdollRigidBodyes[i].isKinematic = true;
			}
		}
		_capsuleRigidBody.isKinematic = false;
		_enemyAnimator.enabled = true;
		_enemyAnimator.SetTrigger("StandUp");
	}
	private void TurnOnRagdoll()
	{
		_usingRagdoll = true;
		_capsuleRigidBody.isKinematic = true;
		_capsuleCollider.isTrigger = true;
		for (int i = 0; i < _ragdollColliders.Length; i++)
		{
			if (_ragdollColliders[i] != _capsuleCollider)
			{
				_ragdollColliders[i].isTrigger = false;
			}
			if (_ragdollRigidBodyes[i] != _capsuleRigidBody)
			{
				_ragdollRigidBodyes[i].isKinematic = false;
			}
		}
		_enemyAnimator.enabled = false;
	}
	private void MoveEnemy()
	{
		if (_isActive)
		{
			transform.position = Vector3.MoveTowards(transform.position, _playerTransform.position, EnemySpeed);
		}
	}
	private void FixedUpdate()
	{
		if (!_usingRagdoll)
		{
			transform.LookAt(_playerTransform, Vector3.up);
		}
		if (_isRunning)
		{
			MoveEnemy();
		}
		if (_needToCalculateDistance)
		{
			CalculateDistance();
		}
	}
	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Smash"))
		{
			if (!IsBigEnemy)
			{
				DeathParticles.transform.parent = null;
				DeathParticles.Play();
				if (CoinParticles != null)
				{
					CoinParticles.transform.parent = null;
					CoinParticles.Play();
				}
				Instantiate(SplashObject, new Vector3(transform.position.x, 0.001f, transform.position.z), Quaternion.Euler(0, Random.Range(0, 360), 0)).GetComponent<SplashController>().ActivateSplash();
				_rageController.AddRage();
				if (DeathParticles.GetComponent<ParticleSystemDestroyer>() != null)
				{
					DeathParticles.GetComponent<ParticleSystemDestroyer>().DestroyParticles();
				}
				if (_isAttacking)
				{
					_rageController.RemoveEnemy();
				}
				DestroyEnemy();
			}
			else
			{
				_isBroken = true;
				DeathParticles.transform.parent = null;
				DeathParticles.Play();
				if (CoinParticles != null)
				{
					CoinParticles.transform.parent = null;
					CoinParticles.Play();
				}
				Instantiate(SplashObject, new Vector3(transform.position.x, 0.001f, transform.position.z), Quaternion.Euler(0, Random.Range(0, 360), 0)).GetComponent<SplashController>().ActivateSplash();
				//float distance = Vector3.Distance(transform.position, Vector3.zero);
				//float angle =  Mathf.Atan(0.5f / distance) * Mathf.Rad2Deg;
				//Instantiate(NormalEnemy, new Vector3(transform.position.x + 0.5f, 0.001f, transform.position.z), Quaternion.Euler(0, Random.Range(0, 360), 0)).GetComponent<EnemyController>().ThrowEnemy();
				//Instantiate(NormalEnemy, new Vector3(transform.position.x - 0.5f, 0.001f, transform.position.z), Quaternion.Euler(0, Random.Range(0, 360), 0)).GetComponent<EnemyController>().ThrowEnemy();
				for (int i = 0; i < NormalEnemyes.Length; i++)
				{
					NormalEnemyes[i].transform.parent = null;
					NormalEnemyes[i].SetActive(true);
					NormalEnemyes[i].GetComponent<EnemyController>().ThrowEnemy();
				}
				if (DeathParticles.GetComponent<ParticleSystemDestroyer>() != null)
				{
					DeathParticles.GetComponent<ParticleSystemDestroyer>().DestroyParticles();
				}
				_enemyAnimator.SetBool("Attack1", false);
				if (_isAttacking)
				{
					_rageController.RemoveEnemy();
				}
				DestroyEnemy();
			}
		}
		if (other.CompareTag("Player") && _isRunning)
		{
			_rageController.AddEnemy();
			StartAttacking();
			_needToCalculateDistance = true;
		}
		if (other.CompareTag("Tackle"))
		{
			_enemyAnimator.SetBool("Attack1", false);
			ThrowEnemy();
		}
	}
	private void OnTriggerExit2D(Collider2D other)
	{
		Debug.Log("TriggerExit");
		if (other.CompareTag("Player") && !_isRunning)
		{
			ThrowEnemy();
			Debug.Log("TriggerExit Player");
			/*
			StartRunning();
			StartAttacking();
			_enemyAnimator.SetBool("Attack1", false);
			switch (Random.Range(1, 4))
			{
				case 1:
					_enemyAnimator.SetBool("Run1", true);
					break;
				case 2:
					_enemyAnimator.SetBool("Run2", true);
					break;
				case 3:
					_enemyAnimator.SetBool("Run3", true);
					break;
			}*/
		}
	}
	private void ThrowEnemy()
	{
		_inKnockout = true;
		_isRunning = false;
		TurnOnRagdoll();
		HipsRigidBody.AddForce((_hammerCenterTransform.position + transform.position) * ((2.5f - Vector3.Distance(transform.position, _hammerCenterTransform.position)) * ForceOfTackle));
		Invoke("TurnOffRagdoll", TimeOfKnockout);
		if (IsBigEnemy)
		{
			Invoke("StartRunning", TimeOfKnockout + LenghtOfKickUpAnimation + 1f);
		}
		else
		{
			Invoke("StartRunning", TimeOfKnockout + LenghtOfKickUpAnimation);
		}
	}
	private void StartRunning()
	{
		if (!_inKnockout)
		{
			_isRunning = true;
		}
	}
	private void StartAttacking()
	{
		_isAttacking = true;
		_isRunning = false;
		switch (Random.Range(1, 2))
		{
			case 1:
				_enemyAnimator.SetBool("Attack1", true);
				break;
		}
	}
	public void DestroyEnemy()
	{
		if (IsBigEnemy && !_isBroken)
		{
			_mainGameController.DeleteEnemyFromCounter();
			_mainGameController.DeleteEnemyFromCounter();
		}
		_mainGameController.DeleteEnemyFromCounter();
		_mainGameController.CheckIfAllEnemyesDefeated();
		Destroy(this.gameObject);
	}
	private void CalculateDistance()
	{
		_distanceVector = Vector3.Distance(transform.position, _playerTransform.position);
		Debug.Log("Distance: " + _distanceVector);
		if (_distanceVector > AttackDistance)
		{
			_needToCalculateDistance = false;
			_rageController.RemoveEnemy();
			_isAttacking = false;
			StartRunning();
			//StartAttacking();
			_enemyAnimator.SetBool("Attack1", false);
			switch (Random.Range(1, 4))
			{
				case 1:
					_enemyAnimator.SetBool("Run1", true);
					break;
				case 2:
					_enemyAnimator.SetBool("Run2", true);
					break;
				case 3:
					_enemyAnimator.SetBool("Run3", true);
					break;
			}
			_enemyAnimator.SetTrigger("StartRunning");
		}
	}
}
