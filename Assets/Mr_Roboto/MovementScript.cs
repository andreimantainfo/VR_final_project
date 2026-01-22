using UnityEngine;

public class MovementScript : MonoBehaviour
{
    Transform leftHipJoint;
    Transform leftKneeJoint;
    Transform leftFootJoint;

    Transform rightHipJoint;
    Transform rightKneeJoint;
    Transform rightFootJoint;

    float hipMaxForwardAngle = 10.0f;
    float hipMaxBackwardAngle = 10.0f;
    float kneeMaxForwardAngle = 0.0f;
    float kneeMaxBackwardAngle = 20.0f;
    float footMaxForwardAngle = 10.0f;
    float footMaxBackwardAngle = 10.0f;

    float walkingPeriod = 1.5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
}
