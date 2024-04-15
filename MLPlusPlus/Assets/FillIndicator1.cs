using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class FillIndicator1 : MonoBehaviour
{
	[Range(-1.0f, 1.0f)]
	public float FilledLevel = 1f;

	void Update()
	{
		if (FilledLevel > 0f) {
			transform.GetComponent<RectTransform>().anchorMax = new Vector2((FilledLevel + 1f) / 2f, 1);
			transform.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0);
		}
		else if (FilledLevel < 0f) {
			transform.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 1);
			transform.GetComponent<RectTransform>().anchorMin = new Vector2((FilledLevel + 1f) / 2f, 0);
		}
	}
}
