using UnityEngine;

public class HammerController /* по совместительству аниматор контроллер и uiController*/: MonoBehaviour
{
	public CapsuleCollider SmashCollider;
	public CapsuleCollider TackleCollider;
	public GameObject InGameUI;
	public GameObject EndGameUI;
	public Transform HammerFull;
	public Transform HammerHandle;
	public Transform HammerMainPart;
	public Animator PlayerAnimator;
	public Animator CameraAnimator;

	public GameObject MainPlane;
	public GameObject[] FloorParts;



	private RageController _rageController;
	private PlayerController _playerController;
	private SpawnController _spawnController;
	private Vector3 _startHammerPosition;
	private Vector3 _startHammerRotation;
	private Vector3 VectorForTransformingHammer;
	private float _increaseForSingleRagePoints;
	private float _invokeTime;
	private int _ragePoints;
	private void Start()
	{
		_rageController = FindObjectOfType<RageController>();
		_playerController = FindObjectOfType<PlayerController>();
		_spawnController = FindObjectOfType<SpawnController>();
		_startHammerPosition = HammerFull.localPosition;
		_startHammerRotation = HammerFull.localRotation.eulerAngles;
		VectorForTransformingHammer = new Vector3();
	}
	public void MakeHammerBigger()
	{
		_playerController.CanHit = false;
		_playerController.ChangePlayerState();
		PlayerAnimator.gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);
		CameraAnimator.SetTrigger("MoveToPlayer");
		Invoke("ActivatePlayerStartAnimation", 1f);
		Invoke("StartMakingHammerBigger", 2f);
		_ragePoints = 0;
		_increaseForSingleRagePoints = 1 / _rageController.RageMaxAmount;
		for (int i = 0; i < _rageController.RageCurrentAmount; i++)
		{
			_ragePoints++;
		}
		SmashCollider.radius += _increaseForSingleRagePoints * _ragePoints;
		TackleCollider.radius += _increaseForSingleRagePoints * _ragePoints;
	}
	private void ActivatePlayerStartAnimation()
	{
		//HammerFull.localPosition = new Vector3(0.155f, 0.343f, 0);
		//HammerFull.localRotation = Quaternion.Euler(15, 75, -20);
		PlayerAnimator.SetTrigger("RaiseHammer");
	}
	private void ActivatePlayerEndAnimation()
	{
		InGameUI.SetActive(false);
		CameraAnimator.SetTrigger("MoveFromPlayer");
		PlayerAnimator.SetTrigger("MakeABigHit");
		_spawnController.SpawnBonusEnemyes();
		_playerController.CanHit = true;

		//HammerFull.localPosition = _startHammerPosition;
		//8HammerFull.localRotation = Quaternion.Euler(_startHammerRotation);
	}
	private void StartMakingHammerBigger()
	{
		for (int i = 0; i < _rageController.RageCurrentAmount; i++)
		{
			for (int j = 0; j < 4; j++)
			{
				Invoke("MakeHammerALittleBigger", _invokeTime);
				_invokeTime += 0.003f;
			}
		}
		Invoke("ActivatePlayerEndAnimation", _invokeTime);
	}
	private void MakeHammerALittleBigger()
	{
		_rageController.TurnDownRageALittle();

		VectorForTransformingHammer = HammerMainPart.localScale;
		VectorForTransformingHammer.x += 0.003f;
		VectorForTransformingHammer.y += 0.002f;
		VectorForTransformingHammer.z += 0.003f;
		HammerMainPart.localScale = VectorForTransformingHammer;

		VectorForTransformingHammer = HammerHandle.localScale;
		VectorForTransformingHammer.y += 0.002f;
		HammerHandle.localScale = VectorForTransformingHammer;

		VectorForTransformingHammer = HammerMainPart.localPosition;
		VectorForTransformingHammer.y += 0.003f;
		VectorForTransformingHammer.z = 0;
		VectorForTransformingHammer.x = 0;
		HammerMainPart.localPosition = VectorForTransformingHammer;
	}
	public void PlayEndGameAnimations()
	{
		Invoke("PlayEndAnimations", 2f);
	}
	public void PlayEndAnimations()
	{
		_playerController.CanHit = false;
		CameraAnimator.SetTrigger("MoveToPlayer");
		PlayerAnimator.gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);
		PlayerAnimator.SetTrigger("Dance");
		Invoke("ActivateEndGameUi", 1f);
	}
	private void ActivateEndGameUi()
	{
		EndGameUI.SetActive(true);
	}
	public void BreakFloor()
	{
		_playerController.CanHit = false; 
		PlayerAnimator.SetBool("IsFalling", true);
		PlayerAnimator.SetTrigger("Blow");
		Invoke("ActivateBrokenFloor", 0.5f);
	}
	private void ActivateBrokenFloor()
	{
		_playerController.PlayerRigidbody.isKinematic = false;
		for (int i = 0; i < FloorParts.Length; i++)
		{
			FloorParts[i].SetActive(true);
		}
		MainPlane.SetActive(false);
	}
}
