using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollCollider : MonoBehaviour
{
	public bool IsRolled = false;
	public float RollDelay = 2f;

	private bool rolling = false;
	private float rollTime;

	void Update()
	{
		if (rolling && Time.time - rollTime > RollDelay) {
			IsRolled = true;
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.TryGetComponent<Ground>(out Ground ground)) {
			rolling = true;
			rollTime = Time.time;
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (other.TryGetComponent<Ground>(out Ground ground)) {
			rolling = false;
			IsRolled = false;
		}
	}
}
