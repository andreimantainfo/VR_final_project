using UnityEngine;

public class StandingCharacter : MonoBehaviour
{
    [Header("Animation Settings")]
    public bool useAnimator = true;

    [Header("Idle Movement (if not using Animator)")]
    public float breathingAmount = 0.015f;
    public float breathingSpeed = 2f;
    public float swayAmount = 3f;
    public float swaySpeed = 0.8f;

    private Animator animator;
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private float timeCounter = 0f;

    void Start()
    {
        animator = GetComponent<Animator>();
        initialPosition = transform.position;
        initialRotation = transform.rotation;

        if (useAnimator && animator != null)
        {
            // Set idle/standing animation
            animator.SetBool("isStanding", true);
        }
    }

    void Update()
    {
        if (!useAnimator || animator == null)
        {
            timeCounter += Time.deltaTime;

            float breathOffset = Mathf.Sin(timeCounter * breathingSpeed) * breathingAmount;

            float swayX = Mathf.Sin(timeCounter * swaySpeed * 0.7f) * 0.01f;

            transform.position = initialPosition + new Vector3(swayX, breathOffset, 0);

            float rotationOffset = Mathf.Sin(timeCounter * swaySpeed) * swayAmount;
            transform.rotation = initialRotation * Quaternion.Euler(0, rotationOffset, 0);
        }
    }
}