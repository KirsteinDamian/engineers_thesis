using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    public Rigidbody rb;
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
    public Vector3 oldPosition;
    [SerializeField]
    public Quaternion quatMeshRotation;
    [SerializeField]
    public float speed = 5000;
    public Collider carCollider;
    public SteeringWheel steeringWheel;
    public Accelerator accelerator;
    public BrakePedal brakePedal;
    public bool moveEnabled = false;


    void SetWheelsPosition()
    {
        SetWheelPosition(wheelLeftRearColider, wheelLeftRearMesh);
        SetWheelPosition(wheelRightRearColider, wheelRightRearMesh);
        SetWheelPosition(wheelLeftFrontColider, wheelLeftFrontMesh);
        SetWheelPosition(wheelRightFrontColider, wheelRightFrontMesh);

        Physics.IgnoreCollision(carCollider, wheelLeftRearColider);
        Physics.IgnoreCollision(carCollider, wheelRightRearColider);
        Physics.IgnoreCollision(carCollider, wheelLeftFrontColider);
        Physics.IgnoreCollision(carCollider, wheelRightFrontColider);
    }

    void SetWheelPosition(WheelCollider wheelCollider, Transform wheelMesh)
    {
        wheelCollider.GetWorldPose(out Vector3 pos, out Quaternion quat);

        //quatMeshRotation = Quaternion.Euler(quat.x, quat.y, 90f);
        //Vector3 offset = new Vector3(0.1f, 0, 0);
        wheelMesh.position = pos;
        wheelMesh.rotation = quat;
        wheelMesh.Rotate(new Vector3(quat.x, quat.y, 90));
    }

    void Turn2()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            if (currentTurn < maxTurn)
                currentTurn += Time.deltaTime * maxTurn * 4;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            if (currentTurn > -maxTurn)
                currentTurn -= Time.deltaTime * maxTurn * 4;
        }
        else
        {
            if (currentTurn > 4)
                currentTurn -= Time.deltaTime * maxTurn * 4;
            else if (currentTurn < -4)
                currentTurn += Time.deltaTime * maxTurn * 4;
            else
                currentTurn = 0;
        }
        wheelRightFrontColider.steerAngle = currentTurn;
        wheelLeftFrontColider.steerAngle = currentTurn;
    }

    void Turn()
    {
        wheelRightFrontColider.steerAngle = steeringWheel.GetWheelAngle() / 4;
        wheelLeftFrontColider.steerAngle = steeringWheel.GetWheelAngle() / 4;
    }

    void Accelerate(float angle)
    {
        var tmpSpeed = speed * (1 + angle / 72);
        if (accelerator.GetIsTouched() || Input.GetKey(KeyCode.UpArrow))
        {
            wheelLeftFrontColider.motorTorque = speed;
            wheelRightFrontColider.motorTorque = speed;
            wheelLeftRearColider.motorTorque = tmpSpeed;
            wheelRightRearColider.motorTorque = tmpSpeed;
            wheelLeftFrontColider.brakeTorque = 0;
            wheelRightFrontColider.brakeTorque = 0;
            wheelLeftRearColider.brakeTorque = 0;
            wheelRightRearColider.brakeTorque = 0;
        }
        else if (brakePedal.GetIsTouched() || Input.GetKey(KeyCode.DownArrow))
        {
            if (angle < 90 && rb.velocity.magnitude > 1)
            {
                var brakeForce = 1;
                wheelLeftFrontColider.brakeTorque = brakeForce / 300;
                wheelRightFrontColider.brakeTorque = brakeForce / 300;
                wheelLeftRearColider.brakeTorque = brakeForce;
                wheelRightRearColider.brakeTorque = brakeForce;
            }
            else
            {
                var reverseSpeed = -5000;
                wheelLeftFrontColider.motorTorque = reverseSpeed;
                wheelRightFrontColider.motorTorque = reverseSpeed;
                wheelLeftRearColider.motorTorque = reverseSpeed;
                wheelRightRearColider.motorTorque = reverseSpeed;

                wheelLeftFrontColider.brakeTorque = 0;
                wheelRightFrontColider.brakeTorque = 0;
                wheelLeftRearColider.brakeTorque = 0;
                wheelRightRearColider.brakeTorque = 0;
            }
        }
        else
        {
            var speed0 = 0;
            wheelLeftFrontColider.motorTorque = speed0;
            wheelRightFrontColider.motorTorque = speed0;
            wheelLeftRearColider.motorTorque = speed0;
            wheelRightRearColider.motorTorque = speed0;
            var brakeForce = 0;
            wheelLeftFrontColider.brakeTorque = brakeForce;
            wheelRightFrontColider.brakeTorque = brakeForce;
            wheelLeftRearColider.brakeTorque = brakeForce;
            wheelRightRearColider.brakeTorque = brakeForce;
        }
        //Debug.Log($"Speed: {tmpSpeed/50000}");
    }
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        maxTurn = 30;
        currentTurn = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (moveEnabled)
        {
            var dir = transform.position - oldPosition;
            var angle = Vector3.Angle(transform.forward, dir);

            SetWheelsPosition();
            Turn();
            //Turn2();
            Accelerate(angle);
            oldPosition = transform.position;
            //Debug.Log($"{angle}");
        }
    }
}
