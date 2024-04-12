using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using Unity.VisualScripting;

[RequireComponent(typeof(CarController))]
public class MLController : Agent
{
	CarController car;
	
	public Checkpoints checkpoints;
	private int targetCheckPoint = 1;

	void Start()
	{
		car = GetComponent<CarController>();
	}

	public override void OnEpisodeBegin()
	{
		transform.localPosition = Vector3.zero;
		transform.localRotation = Quaternion.Euler(Vector3.zero);

		targetCheckPoint = 1;

		car.Reset();
	}

	public override void CollectObservations(VectorSensor sensor)
	{
		Vector3 checkpointForward = checkpoints.checkpoints[targetCheckPoint].transform.forward;
		float dotDirection = Vector3.Dot(transform.forward, checkpointForward);
		sensor.AddObservation(dotDirection);

		float checkpointDistance = Vector3.Distance(transform.localPosition, checkpoints.checkpoints[targetCheckPoint].transform.localPosition);
		sensor.AddObservation(checkpointDistance);

		sensor.AddObservation(car.GetComponent<Rigidbody>().velocity);
	}


	public override void OnActionReceived(ActionBuffers actions)
	{
		car.SteeringInput = Mathf.Clamp(actions.ContinuousActions[0], -1f, 1f);
		car.AccelInput = Mathf.Clamp(actions.ContinuousActions[1], 0f, 1f);
		car.BrakeInput = Mathf.Clamp(actions.ContinuousActions[2], 0f, 1f);

		AddReward(-0.1f * Time.deltaTime);
	}

	public override void Heuristic(in ActionBuffers actionsOut)
	{
		ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
		continuousActions[0] = 0f;
		continuousActions[1] = 0f;
		continuousActions[2] = 0f;

		if (Input.GetKey(KeyCode.RightArrow)) {
			continuousActions[0] = 1f;
		}
		else if (Input.GetKey(KeyCode.LeftArrow)) {
			continuousActions[0] = -1f;
		}

		if (Input.GetKey(KeyCode.UpArrow)) {
			continuousActions[1] = 1f;
		}
		else if (Input.GetKey(KeyCode.DownArrow)) {
			continuousActions[2] = 1f;
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.TryGetComponent<Checkpoint>(out Checkpoint checkpoint)) {
			if (checkpoint.checkpointIndex == targetCheckPoint) {
				AddReward(+1f);
				//Debug.Log("Got checkpoint: " + targetCheckPoint);
				targetCheckPoint++;
				if (targetCheckPoint >= checkpoints.CheckpointLength) targetCheckPoint = 0;
			}
			//EndEpisode();
		}
		else if (other.TryGetComponent<Wall>(out Wall wall)) {
			AddReward(-1f);
			EndEpisode();
		}
	}
	private void OnCollisionEnter(Collision collision)
	{
		OnTriggerEnter(collision.collider);
	}
}