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
    [SerializeField]
    public Quaternion quatMeshRotation;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        SetWheelsPosition();
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
        quatMeshRotation = Quaternion.Euler(quat.x, quat.y, 90f);
        wheelMesh.position = pos;
        wheelMesh.rotation = quatMeshRotation;
    }
}
