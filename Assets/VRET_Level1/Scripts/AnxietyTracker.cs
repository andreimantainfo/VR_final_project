using UnityEngine;
using UnityEngine.Rendering;                 // REQUIRED for Volume
using UnityEngine.Rendering.Universal;       // REQUIRED for Vignette
using TMPro;

public class AnxietyTracker : MonoBehaviour
{
    [Header("Configuration")]
    public Transform avatarHead;      
    public float anxietyThreshold = 5.0f;    // Sensitivity
    public VolumeProfile globalVolume;       // Drag the Volume Profile here
    public GameObject winText;
    public FeedbackSystem feedbackSystem;
    private bool levelEnded = false;

    [Header("Therapy Metrics")]
    public float currentMouseSpeed;   
    public float jitterAccumulator;   
    public bool isMakingEyeContact;
    public float eyeContactTimer;

    [Header("Jitter Analytics")]
    public float currentJitter; // What's happening right now
    public float highestJitter = 0f; // The "Panic Peak"
    private float totalJitter = 0f;
    private int sampleCount = 0;
    private Quaternion lastRotation;

    // Private variables
    private Vignette vignetteEffect; 

    void Start()
    {
        lastRotation = transform.rotation;
        // Setup the Visual Effect link
        if (globalVolume != null) 
        {
            globalVolume.TryGet(out vignetteEffect);
            if (vignetteEffect != null) vignetteEffect.intensity.value = 0f;
        }
    }

    void Update()
    {
        CalculateMouseAnxiety();
        CheckEyeContact();

        // 1. Calculate Jitter (Speed of head movement)
        // We compare current rotation to last frame's rotation
        float angularSpeed = Quaternion.Angle(transform.rotation, lastRotation) / Time.deltaTime;
        lastRotation = transform.rotation;

        // 2. Filter out "looking around" vs "trembling"
        // Normal looking speed is usually under 50-90 degrees/sec. 
        // Anything small and fast is jitter.

        currentJitter = angularSpeed;

        // 3. Record Data (Only if the level hasn't ended)
        if (!levelEnded)
        {
            totalJitter += currentJitter;
            sampleCount++;

            if (currentJitter > highestJitter)
            {
                highestJitter = currentJitter;
            }
        }
    }

    public float GetAverageJitter()
    {
        if (sampleCount == 0) return 0;
        return totalJitter / sampleCount;
    }

    void CalculateMouseAnxiety()
    {
        float moveX = Input.GetAxis("Mouse X");
        float moveY = Input.GetAxis("Mouse Y");
        
        float rawSpeed = (new Vector2(moveX, moveY)).magnitude / Time.deltaTime;
        currentMouseSpeed = rawSpeed;

        // Jitter Logic
        if (currentMouseSpeed > anxietyThreshold)
        {
            // Buildup speed (2x)
            jitterAccumulator += Time.deltaTime * 2; 
        }
        else
        {
            // Cooldown speed
            jitterAccumulator -= Time.deltaTime; 
        }
        
        // Keep between 0 and 100
        jitterAccumulator = Mathf.Clamp(jitterAccumulator, 0f, 100f);
    }

    void CheckEyeContact()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 20f))
        {
            if (hit.transform == avatarHead)
            {
                isMakingEyeContact = true;
                eyeContactTimer += Time.deltaTime;
                
                // WIN CONDITION LOGIC
                if (eyeContactTimer >= 30f && !levelEnded)
                {
                    levelEnded = true;

                    if (feedbackSystem != null)
                    {
                        feedbackSystem.ShowFeedback();
                    }
                }
            }
            else 
            {
                isMakingEyeContact = false;
            }
        }
        else
        {
            isMakingEyeContact = false;
        }
    }

    void UpdateVisuals()
    {
        if (vignetteEffect != null)
        {
            // Map Jitter (0-50) to Intensity (0.0-0.7)
            float targetIntensity = Mathf.Clamp01(jitterAccumulator / 50f) * 0.7f;
            
            // Smoothly animate the darkness
            vignetteEffect.intensity.value = Mathf.Lerp(vignetteEffect.intensity.value, targetIntensity, Time.deltaTime * 5f);
        }
    }
}