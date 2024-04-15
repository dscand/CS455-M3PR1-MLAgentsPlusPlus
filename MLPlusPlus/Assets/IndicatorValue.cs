using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IndicatorValue : MonoBehaviour
{
	public TMP_Text number;

	public void SetValue(float value)
	{
		number.text = value.ToString("F3");
	}
}
