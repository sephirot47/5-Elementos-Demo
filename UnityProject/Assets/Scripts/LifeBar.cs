using UnityEngine;
using System.Collections;

public class LifeBar : MonoBehaviour 
{
	public Enemy enemy;

	private Vector3 originalLifeBarFillScale;
	private Transform lifeBarFill;

	void Start () 
	{
		foreach(Transform t in transform)
		{
			if(t.gameObject.name == "Fill")
			{
				lifeBarFill = t;
				break;
			}
		}

		originalLifeBarFillScale = lifeBarFill.localScale;
	}

	void LateUpdate()
	{
		ScaleLifeBar();
		LookAtCamera();
	}

	void ScaleLifeBar()
	{
		float percent = Mathf.Max(0.0f, (enemy.GetCurrentLife() / enemy.GetMaxLife())); 
		Vector3 s = new Vector3(originalLifeBarFillScale.x * percent, 
		                        originalLifeBarFillScale.y, 
		                        originalLifeBarFillScale.z);
		
		lifeBarFill.localScale = s;
	}

	void LookAtCamera()
	{
		transform.rotation = Quaternion.LookRotation(Camera.main.transform.position - transform.position);
	}
}
