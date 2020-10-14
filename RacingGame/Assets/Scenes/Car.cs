using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    public WheelCollider wheelLeftRearColider;
    public WheelCollider wheelRightRearColider;
    public WheelCollider wheelLeftFrontColider;
    public WheelCollider wheelRightFrontColider;
    public Transform wheelLeftRearMesh;
    public Transform wheelRightRearMesh;
    public Transform wheelLeftFrontMesh;
    public Transform wheelRightFrontMesh;
    public float maxTurn;
    public float currentTurn;
    [SerializeField]
    public Quaternion quatMeshRotation;

    // Start is called before the first frame update
    void Start()
    {
        maxTurn = 45;
        currentTurn = 0;
    }

    // Update is called once per frame
    void Update()
    {
        SetWheelsPosition();
        
        if (Input.GetKey(KeyCode.UpArrow))
        {
            var speed = 1200;
            //wheelLeftFrontColider.motorTorque = speed;
            //wheelRightFrontColider.motorTorque = speed;
            wheelLeftRearColider.motorTorque = speed;
            wheelRightRearColider.motorTorque = speed;
        }
        else
        {
            var speed = 0;
            wheelLeftFrontColider.motorTorque = speed;
            wheelRightFrontColider.motorTorque = speed;
            wheelLeftRearColider.motorTorque = speed;
            wheelRightRearColider.motorTorque = speed;
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            if (currentTurn < maxTurn)
                currentTurn += Time.deltaTime * maxTurn * 2;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            if (currentTurn > -maxTurn)
                currentTurn -= Time.deltaTime * maxTurn * 2;
        }
        else
        {
            if (currentTurn > 4)
                currentTurn -= Time.deltaTime * maxTurn;
            else if (currentTurn < -4)
                currentTurn += Time.deltaTime * maxTurn;
            else
                currentTurn = 0;
        }
        wheelRightFrontColider.steerAngle = currentTurn;
        wheelLeftFrontColider.steerAngle = currentTurn;
    }

    void SetWheelsPosition()
    {
        SetWheelPosition(wheelLeftRearColider, wheelLeftRearMesh);
        SetWheelPosition(wheelRightRearColider, wheelRightRearMesh);
        SetWheelPosition(wheelLeftFrontColider, wheelLeftFrontMesh);
        SetWheelPosition(wheelRightFrontColider, wheelRightFrontMesh);
    }

    void SetWheelPosition(WheelCollider wheelCollider, Transform wheelMesh)
    {
        wheelCollider.GetWorldPose(out Vector3 pos, out Quaternion quat);
        
        //quatMeshRotation = Quaternion.Euler(quat.x, quat.y, 90f);
        wheelMesh.position = pos;
        wheelMesh.rotation = quat;
        wheelMesh.Rotate(new Vector3(quat.x, quat.y, 90));
    }
}
