using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct AxleInfo
{
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;
    public bool motor;
    public bool steering;
}
public class CarController : MonoBehaviour
{
    public List<AxleInfo> axleInfos;

    public float maxMotorTorque;
    public float maxSteeringAngle;
    public float maxBrakeTorque;
    public float maxSpeed;

    WheelFrictionCurve wheelSidewayFriction;
    WheelFrictionCurve wheelForwardFriction;

    private float motor = 0f;
    private float steering = 0f;
    private float braking = 0f;
    private float currentSpeed = 0f;

    private void Start()
    {
        wheelForwardFriction.extremumSlip = 0.4f;
        wheelForwardFriction.extremumValue = 1f;
        wheelForwardFriction.asymptoteSlip = 0.8f;
        wheelForwardFriction.asymptoteValue = 0.5f;

        wheelSidewayFriction.extremumSlip = 0.2f;
        wheelSidewayFriction.extremumValue = 1f;
        wheelSidewayFriction.asymptoteSlip = 0.5f;
        wheelSidewayFriction.asymptoteValue = 0.75f;

    }

    public void ApplyLocalPositionToVisuals(WheelCollider collider)
    {
        if (collider.transform.childCount == 0)
        {
            return;
        }

        Transform visualWheel = collider.transform.GetChild(0);

        Vector3 position;
        Quaternion rotation;
        collider.GetWorldPose(out position, out rotation);

        visualWheel.transform.position = position;
        visualWheel.transform.rotation = rotation;
    }

    private void Update()
    {
        motor = Input.GetAxis("Vertical") * maxMotorTorque;
        steering = Input.GetAxis("Horizontal") * maxSteeringAngle;
        braking = Input.GetAxis("Jump") * maxBrakeTorque;
    }

    private void FixedUpdate()
    {

        foreach (AxleInfo axleInfo in axleInfos)
        {
            if (axleInfo.motor)
            {
                currentSpeed = Mathf.Abs(axleInfo.leftWheel.rpm) / 10;
                if(currentSpeed > maxSpeed)
                {
                    motor = 0;
                }

                axleInfo.leftWheel.motorTorque = motor;
                axleInfo.rightWheel.motorTorque = motor;
            }
            if (axleInfo.steering)
            {
                axleInfo.leftWheel.steerAngle = steering;
                axleInfo.rightWheel.steerAngle = steering;
            }
            if (braking > 0)
            {          
                if(axleInfo.leftWheel.name == "BL")
                {
                    wheelSidewayFriction.stiffness = 0f;
                    wheelForwardFriction.stiffness = 0f;
                }
                else
                {
                    wheelSidewayFriction.stiffness = 0.5f;
                    wheelForwardFriction.stiffness = 0.5f;
                }
            }
            else
            {
                wheelSidewayFriction.stiffness = 1f;
                wheelForwardFriction.stiffness = 1f;
            }

            axleInfo.leftWheel.sidewaysFriction = wheelSidewayFriction;
            axleInfo.rightWheel.sidewaysFriction = wheelSidewayFriction;

            axleInfo.leftWheel.forwardFriction = wheelForwardFriction;
            axleInfo.rightWheel.forwardFriction = wheelForwardFriction;

            axleInfo.leftWheel.brakeTorque = braking;
            axleInfo.rightWheel.brakeTorque = braking;

            ApplyLocalPositionToVisuals(axleInfo.leftWheel);
            ApplyLocalPositionToVisuals(axleInfo.rightWheel);
        }
    }
}
