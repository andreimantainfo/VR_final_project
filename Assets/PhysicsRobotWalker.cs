using UnityEngine;

public class PhysicsRobotWalker : MonoBehaviour
{
    [Header("Left Leg Joints")]
    public HingeJoint leftThighJoint;
    public HingeJoint leftShinJoint;
    public HingeJoint leftFootJoint;

    [Header("Right Leg Joints")]
    public HingeJoint rightThighJoint;
    public HingeJoint rightShinJoint;
    public HingeJoint rightFootJoint;

    [Header("Left Arm Joints")]
    public HingeJoint leftShoulderJoint;
    public HingeJoint leftElbowJoint;
    public HingeJoint leftWristJoint;

    [Header("Right Arm Joints")]
    public HingeJoint rightShoulderJoint;
    public HingeJoint rightElbowJoint;
    public HingeJoint rightWristJoint;

    [Header("Animation")]
    public float walkCyclePeriod = 2f;

    [Header("Left Thigh")]
    public float leftThighSwingAngle = 40f;
    public float leftThighSwingOffsetAngle = -10f;
    public float leftThighSwingPhaseOffsetTime = 0f;
    public float leftThighForce = 100f;
    public float leftThighDamping = 10f;

    [Header("Left Shin")]
    public float leftShinSwingAngle = 30f;
    public float leftShinSwingOffsetAngle = 0f;
    public float leftShinSwingPhaseOffsetTime = 0.25f;
    public float leftShinForce = 50f;
    public float leftShinDamping = 10f;

    [Header("Left Foot")]
    public float leftFootSwingAngle = 15f;
    public float leftFootSwingOffsetAngle = 0f;
    public float leftFootSwingPhaseOffsetTime = 0f;
    public float leftFootForce = 30f;
    public float leftFootDamping = 5f;

    [Header("Right Thigh")]
    public float rightThighSwingAngle = 40f;
    public float rightThighSwingOffsetAngle = -10f;
    public float rightThighSwingPhaseOffsetTime = 0.5f;
    public float rightThighForce = 100f;
    public float rightThighDamping = 10f;

    [Header("Right Shin")]
    public float rightShinSwingAngle = 30f;
    public float rightShinSwingOffsetAngle = 0f;
    public float rightShinSwingPhaseOffsetTime = 0.75f;
    public float rightShinForce = 50f;
    public float rightShinDamping = 10f;

    [Header("Right Foot")]
    public float rightFootSwingAngle = 15f;
    public float rightFootSwingOffsetAngle = 0f;
    public float rightFootSwingPhaseOffsetTime = 0.5f;
    public float rightFootForce = 30f;
    public float rightFootDamping = 5f;

    [Header("Left Shoulder")]
    public float leftShoulderSwingAngle = 25f;
    public float leftShoulderSwingOffsetAngle = 0f;
    public float leftShoulderSwingPhaseOffsetTime = 0.5f;
    public float leftShoulderForce = 50f;
    public float leftShoulderDamping = 8f;

    [Header("Left Elbow")]
    public float leftElbowSwingAngle = 20f;
    public float leftElbowSwingOffsetAngle = -15f;
    public float leftElbowSwingPhaseOffsetTime = 0.75f;
    public float leftElbowForce = 30f;
    public float leftElbowDamping = 5f;

    [Header("Left Wrist")]
    public float leftWristSwingAngle = 10f;
    public float leftWristSwingOffsetAngle = 0f;
    public float leftWristSwingPhaseOffsetTime = 0.5f;
    public float leftWristForce = 20f;
    public float leftWristDamping = 3f;

    [Header("Right Shoulder")]
    public float rightShoulderSwingAngle = 25f;
    public float rightShoulderSwingOffsetAngle = 0f;
    public float rightShoulderSwingPhaseOffsetTime = 0f;
    public float rightShoulderForce = 50f;
    public float rightShoulderDamping = 8f;

    [Header("Right Elbow")]
    public float rightElbowSwingAngle = 20f;
    public float rightElbowSwingOffsetAngle = -15f;
    public float rightElbowSwingPhaseOffsetTime = 0.25f;
    public float rightElbowForce = 30f;
    public float rightElbowDamping = 5f;

    [Header("Right Wrist")]
    public float rightWristSwingAngle = 10f;
    public float rightWristSwingOffsetAngle = 0f;
    public float rightWristSwingPhaseOffsetTime = 0f;
    public float rightWristForce = 20f;
    public float rightWristDamping = 3f;

    [Header("Forward Push")]
    public Rigidbody torsoRigidbody;
    public float forwardForce = 5f;
    public float turnTorque = 2f;

    void FixedUpdate()
    {
        // Apply forward force to torso
        if (torsoRigidbody != null)
        {
            torsoRigidbody.AddForce(torsoRigidbody.transform.right * forwardForce, ForceMode.Force);
            torsoRigidbody.AddTorque(torsoRigidbody.transform.up * turnTorque, ForceMode.Force);
        }

        float cycleProgress = (Time.time / walkCyclePeriod) * Mathf.PI * 2f;

        // Left Leg
        float phase = cycleProgress + leftThighSwingPhaseOffsetTime * Mathf.PI * 2f;
        float target = leftThighSwingOffsetAngle + Mathf.Sin(phase) * (leftThighSwingAngle / 2f);
        ApplyTorqueToJoint(leftThighJoint, target, leftThighForce, leftThighDamping);

        phase = cycleProgress + leftShinSwingPhaseOffsetTime * Mathf.PI * 2f;
        target = leftShinSwingOffsetAngle + Mathf.Sin(phase) * (leftShinSwingAngle / 2f);
        ApplyTorqueToJoint(leftShinJoint, target, leftShinForce, leftShinDamping);

        phase = cycleProgress + leftFootSwingPhaseOffsetTime * Mathf.PI * 2f;
        target = leftFootSwingOffsetAngle + Mathf.Sin(phase) * (leftFootSwingAngle / 2f);
        ApplyTorqueToJoint(leftFootJoint, target, leftFootForce, leftFootDamping);

        // Right Leg
        phase = cycleProgress + rightThighSwingPhaseOffsetTime * Mathf.PI * 2f;
        target = rightThighSwingOffsetAngle + Mathf.Sin(phase) * (rightThighSwingAngle / 2f);
        ApplyTorqueToJoint(rightThighJoint, target, rightThighForce, rightThighDamping);

        phase = cycleProgress + rightShinSwingPhaseOffsetTime * Mathf.PI * 2f;
        target = rightShinSwingOffsetAngle + Mathf.Sin(phase) * (rightShinSwingAngle / 2f);
        ApplyTorqueToJoint(rightShinJoint, target, rightShinForce, rightShinDamping);

        phase = cycleProgress + rightFootSwingPhaseOffsetTime * Mathf.PI * 2f;
        target = rightFootSwingOffsetAngle + Mathf.Sin(phase) * (rightFootSwingAngle / 2f);
        ApplyTorqueToJoint(rightFootJoint, target, rightFootForce, rightFootDamping);

        // Left Arm
        phase = cycleProgress + leftShoulderSwingPhaseOffsetTime * Mathf.PI * 2f;
        target = leftShoulderSwingOffsetAngle + Mathf.Sin(phase) * (leftShoulderSwingAngle / 2f);
        ApplyTorqueToJoint(leftShoulderJoint, target, leftShoulderForce, leftShoulderDamping);

        phase = cycleProgress + leftElbowSwingPhaseOffsetTime * Mathf.PI * 2f;
        target = leftElbowSwingOffsetAngle + Mathf.Sin(phase) * (leftElbowSwingAngle / 2f);
        ApplyTorqueToJoint(leftElbowJoint, target, leftElbowForce, leftElbowDamping);

        phase = cycleProgress + leftWristSwingPhaseOffsetTime * Mathf.PI * 2f;
        target = leftWristSwingOffsetAngle + Mathf.Sin(phase) * (leftWristSwingAngle / 2f);
        ApplyTorqueToJoint(leftWristJoint, target, leftWristForce, leftWristDamping);

        // Right Arm
        phase = cycleProgress + rightShoulderSwingPhaseOffsetTime * Mathf.PI * 2f;
        target = rightShoulderSwingOffsetAngle + Mathf.Sin(phase) * (rightShoulderSwingAngle / 2f);
        ApplyTorqueToJoint(rightShoulderJoint, target, rightShoulderForce, rightShoulderDamping);

        phase = cycleProgress + rightElbowSwingPhaseOffsetTime * Mathf.PI * 2f;
        target = rightElbowSwingOffsetAngle + Mathf.Sin(phase) * (rightElbowSwingAngle / 2f);
        ApplyTorqueToJoint(rightElbowJoint, target, rightElbowForce, rightElbowDamping);

        phase = cycleProgress + rightWristSwingPhaseOffsetTime * Mathf.PI * 2f;
        target = rightWristSwingOffsetAngle + Mathf.Sin(phase) * (rightWristSwingAngle / 2f);
        ApplyTorqueToJoint(rightWristJoint, target, rightWristForce, rightWristDamping);
    }

    void ApplyTorqueToJoint(HingeJoint joint, float targetAngle, float force, float damping)
    {
        float currentAngle = joint.angle;
        float angleError = targetAngle - currentAngle;

        while (angleError > 180f) angleError -= 360f;
        while (angleError < -180f) angleError += 360f;

        float angularVelocity = joint.velocity;
        float torque = (angleError * force) - (angularVelocity * damping);

        Vector3 torqueAxis = joint.transform.TransformDirection(joint.axis);
        joint.GetComponent<Rigidbody>().AddTorque(torqueAxis * torque, ForceMode.Force);
    }
}