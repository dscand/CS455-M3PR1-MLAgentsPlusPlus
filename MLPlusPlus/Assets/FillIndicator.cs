using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class FillIndicator : MonoBehaviour
{
	[Range(0.0f, 1.0f)]
	public float FilledLevel = 1f;

	void Update()
	{
		transform.GetComponent<RectTransform>().anchorMax = new Vector2(1, FilledLevel);
	}
}
