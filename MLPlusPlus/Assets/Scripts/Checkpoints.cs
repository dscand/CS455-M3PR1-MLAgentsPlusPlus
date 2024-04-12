using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoints : MonoBehaviour
{
	[HideInInspector] public Checkpoint[] checkpoints;
	[HideInInspector] public int CheckpointLength;

	void Start ()
	{
		checkpoints = GetComponentsInChildren<Checkpoint>();
		CheckpointLength = checkpoints.Length;
		
		for (int i = 0; i < CheckpointLength; i++)
		{
			checkpoints[i].checkpointIndex = i;
		}
	}
}
