using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Indicator : MonoBehaviour
{
	public FillIndicator fillIndicator;
	public TMP_Text number;

	public void SetFill(float fill)
	{
		number.text = fill.ToString("F3");
		fillIndicator.FilledLevel = fill;
	}
}
