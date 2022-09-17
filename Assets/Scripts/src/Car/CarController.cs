using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AxleInfo
{
    public WheelCollider leftWheelCollider;
    public WheelCollider rightWheelCollider;
    public GameObject leftWheelMesh;
    public GameObject rightWheelMesh;
    public bool motor;
    public bool steering;
}

public class CarController : MonoBehaviour
{
    public Transform DriverSeat;
    public GameObject Driver;

    float rotationX = 0, rotationY = 0;
    public float lookSpeed = 2.0f;
    public float lookXLimit = 80.0f;

    public List<AxleInfo> axleInfos;
    public float maxMotorTorque;
    public float maxSteeringAngle;
    public float brakeTorque;
    public float decelerationForce;

    void FixedUpdate()
    {
        float motor = maxMotorTorque * Input.GetAxis("Vertical");
        float steering = maxSteeringAngle * Input.GetAxis("Horizontal");
        for (int i = 0; i < axleInfos.Count; i++)
        {
            if (axleInfos[i].motor && Driver)
            {
                Acceleration(axleInfos[i], motor);
            }
            else
            {
                Deceleration(axleInfos[i]);
            }
            if (axleInfos[i].steering && Driver)
            {
                Steering(axleInfos[i], steering);
            }
            if (Input.GetKey(KeyCode.Space) && Driver)
            {
                Brake(axleInfos[i]);
            }
            ApplyLocalPositionToVisuals(axleInfos[i]);
        }

    }

    private void Acceleration(AxleInfo axleInfo, float motor)
    {
        if (motor != 0f)
        {
            axleInfo.leftWheelCollider.brakeTorque = 0;
            axleInfo.rightWheelCollider.brakeTorque = 0;
            axleInfo.leftWheelCollider.motorTorque = motor;
            axleInfo.rightWheelCollider.motorTorque = motor;
        }

    }

    private void Deceleration(AxleInfo axleInfo)
    {
        axleInfo.leftWheelCollider.brakeTorque = decelerationForce;
        axleInfo.rightWheelCollider.brakeTorque = decelerationForce;
    }

    private void Steering(AxleInfo axleInfo, float steering)
    {
        axleInfo.leftWheelCollider.steerAngle = steering;
        axleInfo.rightWheelCollider.steerAngle = steering;
    }

    private void Brake(AxleInfo axleInfo)
    {
        axleInfo.leftWheelCollider.brakeTorque = brakeTorque;
        axleInfo.rightWheelCollider.brakeTorque = brakeTorque;
    }

	public void GetIn(FPSPlayer player)
    {
        player.gameObject.GetComponent<FPSPlayer>().enabled = false;
        player.gameObject.GetComponent<CharacterController>().enabled = false;
		player.gameObject.GetComponent<ChunkInteracter>().enabled = false;
        player.gameObject.GetComponent<CapsuleCollider>().enabled = false;
		

        player.gameObject.transform.position = DriverSeat.transform.position;
        player.gameObject.transform.rotation = DriverSeat.transform.rotation;
        Driver = player.gameObject;
        player.gameObject.transform.SetParent(gameObject.transform);
    }

    void Update()
    {
        if (!Driver)
            return;

        rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
        Camera.main.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        Driver.transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);

        if (Input.GetKeyDown(KeyCode.Q))
        {
            GetOut();
        }
    }

    private void GetOut()
    {
        Driver.gameObject.GetComponent<CharacterController>().enabled = true;
        Driver.gameObject.GetComponent<FPSPlayer>().enabled = true;
		Driver.gameObject.GetComponent<ChunkInteracter>().enabled = true;
		
        Driver.gameObject.transform.position = DriverSeat.transform.position + Vector3.up;

        Driver.gameObject.transform.SetParent(null);
        Driver.gameObject.GetComponent<CapsuleCollider>().enabled = true;
		Driver = null;
    }

    private void ApplyLocalPositionToVisuals(AxleInfo axleInfo)
    {
        Vector3 position;
        Quaternion rotation;
        axleInfo.leftWheelCollider.GetWorldPose(out position, out rotation);
        axleInfo.leftWheelMesh.transform.position = position;
        axleInfo.leftWheelMesh.transform.rotation = rotation * Quaternion.Euler(0, 90, 0);
        axleInfo.rightWheelCollider.GetWorldPose(out position, out rotation);
        axleInfo.rightWheelMesh.transform.position = position;
        axleInfo.rightWheelMesh.transform.rotation = rotation * Quaternion.Euler(0, -90, 0);
    }
}
