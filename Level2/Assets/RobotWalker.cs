using UnityEngine;

public class RobotWalker : MonoBehaviour
{
    [Header("Left Leg")]
    public Transform torsoLeftThighConnector;
    public Transform leftThighShinConnector;
    public Transform leftShinFootConnector;

    [Header("Right Leg")]
    public Transform torsoRightThighConnector;
    public Transform rightThighShinConnector;
    public Transform rightShinFootConnector;

    [Header("Left Arm")]
    public Transform torsoLeftArmConnector;
    public Transform leftArmForearmConnector;
    public Transform leftForearmHandConnector;

    [Header("Right Arm")]
    public Transform torsoRightArmConnector;
    public Transform rightArmForearmConnector;
    public Transform rightForearmHandConnector;

    [Header("Body")]
    public Transform torso;
    public Transform torsoNeckConnector;
    public Transform neckHeadConnector;

    [Header("Body Animation")]
    public float bobAmplitude = 0.1f;           // How much the body bobs up/down
    public float bobFrequencyMultiplier = 2f;   // Bobs per walk cycle (2 = bob twice per cycle)
    public float bobPhaseOffset = 0f;           // Phase offset for bobbing (0-1)

    [Header("Torso-Neck")]
    public float torsoNeckSwingAngle = 6f;
    public float torsoNeckSwingOffsetAngle = 0f;
    public float torsoNeckSwingPhaseOffsetTime = 0f;

    [Header("Neck-Head")]
    public float neckHeadSwingAngle = 10f;
    public float neckHeadSwingOffsetAngle = 0f;
    public float neckHeadSwingPhaseOffsetTime = 0.1f;

    [Header("Animation")]
    public float walkCyclePeriod = 2f;  // Time in seconds for one full walk cycle

    [Header("Movement")]
    public float walkSpeed = 2f;  // Units per second

    [Header("Left Thigh")]
    public float leftThighSwingAngle = 40f;
    public float leftThighSwingOffsetAngle = -10f;
    public float leftThighSwingPhaseOffsetTime = 0f;

    [Header("Left Shin")]
    public float leftShinSwingAngle = 30f;
    public float leftShinSwingOffsetAngle = -20f;
    public float leftShinSwingPhaseOffsetTime = 0.25f;

    [Header("Left Foot")]
    public float leftFootSwingAngle = 15f;
    public float leftFootSwingOffsetAngle = 0f;
    public float leftFootSwingPhaseOffsetTime = 0f;

    [Header("Right Thigh")]
    public float rightThighSwingAngle = 40f;
    public float rightThighSwingOffsetAngle = -10f;
    public float rightThighSwingPhaseOffsetTime = 0.5f;

    [Header("Right Shin")]
    public float rightShinSwingAngle = 30f;
    public float rightShinSwingOffsetAngle = -20f;
    public float rightShinSwingPhaseOffsetTime = 0.75f;

    [Header("Right Foot")]
    public float rightFootSwingAngle = 15f;
    public float rightFootSwingOffsetAngle = 0f;
    public float rightFootSwingPhaseOffsetTime = 0.5f;

    [Header("Left Shoulder")]
    public float leftShoulderSwingAngle = 25f;
    public float leftShoulderSwingOffsetAngle = 0f;
    public float leftShoulderSwingPhaseOffsetTime = 0.5f;

    [Header("Left Elbow")]
    public float leftElbowSwingAngle = 20f;
    public float leftElbowSwingOffsetAngle = -15f;
    public float leftElbowSwingPhaseOffsetTime = 0.75f;

    [Header("Left Wrist")]
    public float leftWristSwingAngle = 10f;
    public float leftWristSwingOffsetAngle = 0f;
    public float leftWristSwingPhaseOffsetTime = 0.5f;

    [Header("Right Shoulder")]
    public float rightShoulderSwingAngle = 25f;
    public float rightShoulderSwingOffsetAngle = 0f;
    public float rightShoulderSwingPhaseOffsetTime = 0f;

    [Header("Right Elbow")]
    public float rightElbowSwingAngle = 20f;
    public float rightElbowSwingOffsetAngle = -15f;
    public float rightElbowSwingPhaseOffsetTime = 0.25f;

    [Header("Right Wrist")]
    public float rightWristSwingAngle = 10f;
    public float rightWristSwingOffsetAngle = 0f;
    public float rightWristSwingPhaseOffsetTime = 0f;

    void Update()
    {
        // Move forward
        transform.RotateAround(transform.position, transform.up, 30 * Time.deltaTime);
        transform.position += transform.right * walkSpeed * Time.deltaTime;

        // Calculate phase based on walk cycle period
        float cycleProgress = (Time.time / walkCyclePeriod) * Mathf.PI * 2f;

        // Body bobbing
        float bobPhase = cycleProgress * bobFrequencyMultiplier + bobPhaseOffset * Mathf.PI * 2f;
        float bobHeight = Mathf.Sin(bobPhase) * bobAmplitude;
        Vector3 torsoPos = torso.localPosition;
        torsoPos.y = bobHeight;
        torso.localPosition = torsoPos;

        // Neck/Head
        float phase = 2 * (cycleProgress + torsoNeckSwingPhaseOffsetTime * Mathf.PI * 2f);
        float angle = torsoNeckSwingOffsetAngle + Mathf.Sin(phase) * (torsoNeckSwingAngle / 2f);
        torsoNeckConnector.localRotation = Quaternion.Euler(0, 0, angle);

        phase = 2 * (cycleProgress + neckHeadSwingPhaseOffsetTime * Mathf.PI * 2f);
        angle = neckHeadSwingOffsetAngle + Mathf.Sin(phase) * (neckHeadSwingAngle / 2f);
        neckHeadConnector.localRotation = Quaternion.Euler(0, 0, angle);

        // Left Leg
        phase = cycleProgress + leftThighSwingPhaseOffsetTime * Mathf.PI * 2f;
        angle = leftThighSwingOffsetAngle + Mathf.Sin(phase) * (leftThighSwingAngle / 2f);
        torsoLeftThighConnector.localRotation = Quaternion.Euler(0, 0, angle);

        phase = cycleProgress + leftShinSwingPhaseOffsetTime * Mathf.PI * 2f;
        angle = leftShinSwingOffsetAngle + Mathf.Sin(phase) * (leftShinSwingAngle / 2f);
        leftThighShinConnector.localRotation = Quaternion.Euler(0, 0, angle);

        phase = cycleProgress + leftFootSwingPhaseOffsetTime * Mathf.PI * 2f;
        angle = leftFootSwingOffsetAngle + Mathf.Sin(phase) * (leftFootSwingAngle / 2f);
        leftShinFootConnector.localRotation = Quaternion.Euler(0, 0, angle);

        // Right Leg
        phase = cycleProgress + rightThighSwingPhaseOffsetTime * Mathf.PI * 2f;
        angle = rightThighSwingOffsetAngle + Mathf.Sin(phase) * (rightThighSwingAngle / 2f);
        torsoRightThighConnector.localRotation = Quaternion.Euler(0, 0, angle);

        phase = cycleProgress + rightShinSwingPhaseOffsetTime * Mathf.PI * 2f;
        angle = rightShinSwingOffsetAngle + Mathf.Sin(phase) * (rightShinSwingAngle / 2f);
        rightThighShinConnector.localRotation = Quaternion.Euler(0, 0, angle);

        phase = cycleProgress + rightFootSwingPhaseOffsetTime * Mathf.PI * 2f;
        angle = rightFootSwingOffsetAngle + Mathf.Sin(phase) * (rightFootSwingAngle / 2f);
        rightShinFootConnector.localRotation = Quaternion.Euler(0, 0, angle);

        // Left Arm
        phase = cycleProgress + leftShoulderSwingPhaseOffsetTime * Mathf.PI * 2f;
        angle = leftShoulderSwingOffsetAngle + Mathf.Sin(phase) * (leftShoulderSwingAngle / 2f);
        torsoLeftArmConnector.localRotation = Quaternion.Euler(0, 0, angle);

        phase = cycleProgress + leftElbowSwingPhaseOffsetTime * Mathf.PI * 2f;
        angle = leftElbowSwingOffsetAngle + Mathf.Sin(phase) * (leftElbowSwingAngle / 2f);
        leftArmForearmConnector.localRotation = Quaternion.Euler(0, 0, angle);

        phase = cycleProgress + leftWristSwingPhaseOffsetTime * Mathf.PI * 2f;
        angle = leftWristSwingOffsetAngle + Mathf.Sin(phase) * (leftWristSwingAngle / 2f);
        leftForearmHandConnector.localRotation = Quaternion.Euler(0, 0, angle);

        // Right Arm
        phase = cycleProgress + rightShoulderSwingPhaseOffsetTime * Mathf.PI * 2f;
        angle = rightShoulderSwingOffsetAngle + Mathf.Sin(phase) * (rightShoulderSwingAngle / 2f);
        torsoRightArmConnector.localRotation = Quaternion.Euler(0, 0, angle);

        phase = cycleProgress + rightElbowSwingPhaseOffsetTime * Mathf.PI * 2f;
        angle = rightElbowSwingOffsetAngle + Mathf.Sin(phase) * (rightElbowSwingAngle / 2f);
        rightArmForearmConnector.localRotation = Quaternion.Euler(0, 0, angle);

        phase = cycleProgress + rightWristSwingPhaseOffsetTime * Mathf.PI * 2f;
        angle = rightWristSwingOffsetAngle + Mathf.Sin(phase) * (rightWristSwingAngle / 2f);
        rightForearmHandConnector.localRotation = Quaternion.Euler(0, 0, angle);
    }
}