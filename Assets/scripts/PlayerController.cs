using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public GameObject SmashColliderObject;
	public GameObject TackleColliderObject;
	public Animator PlayerAnimator;
	public Animator RingAnimator;
	public Animator CameraAnimator;
	public Transform LookAtPoint;
	public Transform PlayerTransform;
	public Rigidbody PlayerRigidbody;
	public float BlowTime;
	public float TimeBeforeHittingFloor;

	private InputController _inputController;
	private Vector3 _startPosition;
	private Vector3 LookAtVector;
	private bool _isBlowing;
	public bool CanHit;
	private bool _directionIsCounting;
	private bool _isBonusPart;
	private bool _isFalling = true;

	private void Start()
	{
		_inputController = FindObjectOfType<InputController>();
		CanHit = true;
	}
	private void FixedUpdate()
	{
		if (_isFalling)
		{
			if (PlayerTransform.position.y <= 0)
			{
				_isFalling = false;
				PlayerRigidbody.isKinematic = true;
				PlayerTransform.position = Vector3.zero;
			}
		}
		if (_inputController.InputStarted && !_isBlowing && !_directionIsCounting)
		{
			_directionIsCounting = true;
			_startPosition = _inputController.TouchPosition;
		}
		else if (!_inputController.InputStarted && _directionIsCounting && CanHit)
		{
			_directionIsCounting = false;
			_isBlowing = true;
			LookAtVector.x = _inputController.TouchPosition.x - _startPosition.x;
			LookAtVector.z = _inputController.TouchPosition.y - _startPosition.y;
			LookAtPoint.position = LookAtVector;
			PlayerTransform.LookAt(LookAtPoint);
			MakeBlow();
			Invoke("ShakeCamera", TimeBeforeHittingFloor);
			Invoke("TurnOnSmashCollider", TimeBeforeHittingFloor);
			Invoke("TurnOffSmashCollider", TimeBeforeHittingFloor + 0.1f);
			Invoke("TurnOnTackleCollider", TimeBeforeHittingFloor);
			Invoke("TurnOffTackleCollider", TimeBeforeHittingFloor + 0.1f);
			Invoke("ChangeBlowingBool", BlowTime);
		}
	}
	public void ChangePlayerState()
	{
		_isBonusPart = true;
	}
	public void MakeBlow()
	{
		PlayerAnimator.SetTrigger("Blow");
		if (_isBonusPart)
		{
			RingAnimator.SetTrigger("BigBlow");
		}
		else
		{
			RingAnimator.SetTrigger("Blow");
		}
	}
	public void ShakeCamera()
	{
		if (_isBonusPart)
		{
			CameraAnimator.SetTrigger("Shake");
		}
		else
		{
			CameraAnimator.SetTrigger("Shake");
		}
	}
	public void PlayerDead()
	{
		CanHit = false;
		PlayerAnimator.SetTrigger("Die");
	}
	private void ChangeBlowingBool()
	{
		_isBlowing = false;
	}
	private void TurnOnSmashCollider()
	{
		SmashColliderObject.SetActive(true);
	}
	private void TurnOffSmashCollider()
	{
		SmashColliderObject.SetActive(false);
	}
	private void TurnOnTackleCollider()
	{
		TackleColliderObject.SetActive(true);
	}
	private void TurnOffTackleCollider()
	{
		TackleColliderObject.SetActive(false);
	}
}
