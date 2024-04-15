using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Indicator1 : MonoBehaviour
{
	public FillIndicator1 fillIndicator;
	public TMP_Text number;

	public void SetFill(float fill)
	{
		number.text = fill.ToString("F3");
		fillIndicator.FilledLevel = fill;
	}
}
