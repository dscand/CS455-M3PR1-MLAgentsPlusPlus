using UnityEngine;

public class CarController : MonoBehaviour
{
	// Settings
	[SerializeField] private float motorForce, breakForce, maxSteerAngle;

	// Wheel Colliders
	[SerializeField] private WheelCollider frontLeftWheelCollider, frontRightWheelCollider;
	[SerializeField] private WheelCollider rearLeftWheelCollider, rearRightWheelCollider;

	// Wheels
	[SerializeField] private Transform frontLeftWheelTransform, frontRightWheelTransform;
	[SerializeField] private Transform rearLeftWheelTransform, rearRightWheelTransform;

	[HideInInspector]
	public float SteeringInput, AccelInput, BrakeInput = 0f;
	private float currentSteerAngle, currentbreakForce;

	public void Reset() {
		SteeringInput = 0f;
		AccelInput = 0f;
		BrakeInput = 0f;

		GetComponent<Rigidbody>().velocity = Vector3.zero;
		GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

		FixedUpdate();
	}

	private void FixedUpdate() {
		HandleMotor();
		HandleSteering();
		UpdateWheels();
	}

	private void HandleMotor() {
		frontLeftWheelCollider.motorTorque = AccelInput * motorForce;
		frontRightWheelCollider.motorTorque = AccelInput * motorForce;
		currentbreakForce = BrakeInput * breakForce;
		ApplyBreaking();
	}

	private void ApplyBreaking() {
		frontRightWheelCollider.brakeTorque = currentbreakForce;
		frontLeftWheelCollider.brakeTorque = currentbreakForce;
		rearLeftWheelCollider.brakeTorque = currentbreakForce;
		rearRightWheelCollider.brakeTorque = currentbreakForce;
	}

	private void HandleSteering() {
		currentSteerAngle = maxSteerAngle * SteeringInput;
		frontLeftWheelCollider.steerAngle = currentSteerAngle;
		frontRightWheelCollider.steerAngle = currentSteerAngle;
	}
	
	private void UpdateWheels() {
		UpdateSingleWheel(frontLeftWheelCollider, frontLeftWheelTransform);
		UpdateSingleWheel(frontRightWheelCollider, frontRightWheelTransform);
		UpdateSingleWheel(rearRightWheelCollider, rearRightWheelTransform);
		UpdateSingleWheel(rearLeftWheelCollider, rearLeftWheelTransform);
	}

	private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform) {
		Vector3 pos;
		Quaternion rot; 
		wheelCollider.GetWorldPose(out pos, out rot);
		wheelTransform.rotation = rot;
		wheelTransform.position = pos;
	}
}