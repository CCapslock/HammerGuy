using UnityEngine;

public class InputController : MonoBehaviour
{
	//следит за пальцем на экране
	public Vector3 TouchPosition;
	public bool InputStarted = false;
	public bool UseMouse = false;
	public Camera CameraForInput;
	private Touch touch;

	private void Start()
	{
		//CameraForInput = FindObjectOfType<Camera>();
		TouchPosition = new Vector3(0, 0);
	}

	private void Update()
	{
		if (!UseMouse)
		{
			if (Input.touchCount > 0)
			{
				touch = Input.GetTouch(0);
				if (touch.phase == TouchPhase.Began)
				{
					TouchPosition = CameraForInput.ScreenToWorldPoint(touch.position);
					InputStarted = true;
				}
				else if (touch.phase == TouchPhase.Moved)
				{
					TouchPosition = CameraForInput.ScreenToWorldPoint(touch.position);
				}
			}
			else
			{
				InputStarted = false;
			}
		}
		else
		{
			if (Input.GetMouseButton(0))
			{
				InputStarted = true;
				TouchPosition = Input.mousePosition / 100;
			}
			else
			{
				InputStarted = false;
			}
		}
	}
}
