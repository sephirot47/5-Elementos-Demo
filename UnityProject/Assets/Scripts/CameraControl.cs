using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour 
{

	public Transform target;
	public float targetHeight = 2.0f;
	public float distance = 2.8f;
	public float maxDistance = 10;
	public float minDistance = 0.5f;
	public float xSpeed = 250.0f;
	public float ySpeed = 1000.0f;
	public float yMinLimit = -40;
	public float yMaxLimit = 80;
	public float zoomRate = 20;
	public float rotationDampening = 3.0f;
	public bool rotationDampeningEnabled = false;

	public float camSmoothing = 0.2f;

	private Vector3 smoothCamVel = Vector3.zero; //don't touch
	private Vector3 camPosition;
	private float x = 0.0f;
	private float y = 0.0f;

	void Start () 
	{
		Vector3 angles = transform.eulerAngles;
		x = angles.y;
		y = angles.x;

		camPosition = transform.position; 
		
		// Make the rigid body not change rotation
		if (GetComponent<Rigidbody>()) GetComponent<Rigidbody>().freezeRotation = true;
	}
	
	void LateUpdate () 
	{
		if(!GameState.IsPlaying() || GameState.AllPlayersDead()) return;
		if(target == null) return;

		if(Input.GetMouseButton(1) || true)
		{
			x += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
			y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
		}

		float targetRotationAngle = target.eulerAngles.y;
		float currentRotationAngle = transform.eulerAngles.y;
		if(rotationDampeningEnabled) 
			x = Mathf.LerpAngle(currentRotationAngle, targetRotationAngle, rotationDampening * Time.deltaTime);

		distance -= (Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime) * zoomRate * Mathf.Abs(distance);
		distance = Mathf.Clamp(distance, minDistance, maxDistance);
		
		y = ClampAngle(y, yMinLimit, yMaxLimit);
		
		// ROTATE CAMERA:
		Quaternion rotation = Quaternion.Euler(y, x, 0);
		transform.rotation = rotation;
		
		// POSITION CAMERA:
		//transform.position = target.position -(rotation * Vector3.forward * distance + new Vector3(0, -targetHeight, 0));
		camPosition = Vector3.SmoothDamp(camPosition, target.position, ref smoothCamVel, camSmoothing);
		transform.position = camPosition - (rotation * Vector3.forward * distance + new Vector3(0, -targetHeight, 0));

		// IS VIEW BLOCKED?
		RaycastHit hit; 
		Vector3 trueTargetPosition = target.position + Vector3.up * 0.5f;
		// Cast the line to check:
		if( Physics.Linecast(trueTargetPosition, transform.position, out hit, (1 << LayerMask.NameToLayer("Scenario")) ) ) 
		{  
			// If so, shorten distance so camera is in front of object:
			float tempDistance = Vector3.Distance(trueTargetPosition, hit.point) - 0.28f;
			// Finally, rePOSITION the CAMERA:
			transform.position = target.position - (rotation * Vector3.forward * tempDistance + new Vector3(0,-targetHeight,0));
		}
	}

	public void SelectTarget(Transform go)
	{
		target = go;
	}
	
	private float ClampAngle(float angle, float min, float max) 
	{
		if (angle < -360.0f) angle += 360.0f;
		if (angle > 360.0f) angle -= 360.0f;
		return Mathf.Clamp (angle, min, max);
	}

	public static Vector3 GetLookPoint()
	{
		Vector3 screenPoint = GetLookScreenPoint();
		Ray ray = Camera.main.ScreenPointToRay(screenPoint);
		RaycastHit hit;
		
		int layer = (1 << LayerMask.NameToLayer("Scenario")) | (1 << LayerMask.NameToLayer("Enemies"));
		
		if(Physics.Raycast(ray, out hit, 99999.9f, layer)) return hit.point;
		return Vector3.zero;
	}

	public static Vector3 GetLookScreenPoint()
	{
		Vector2 offset = new Vector2(0, Screen.height * 0.15f);
		return new Vector2(Screen.width / 2, Screen.height / 2) + offset;
	}
}
