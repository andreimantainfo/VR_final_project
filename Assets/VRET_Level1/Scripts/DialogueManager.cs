using UnityEngine;
using TMPro;
using UnityEngine.UI;

// MAKE SURE THIS MATCHES YOUR FILENAME (DialogueManager.cs)
public class DialogueManager : MonoBehaviour 
{
    [Header("UI Components")]
    public GameObject dialoguePanel;      // The Dialogue Box
    public GameObject endScreenPanel;     // NEW: Drag your End Screen here
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI bodyText;
    public TextMeshProUGUI option1Text;
    public TextMeshProUGUI option2Text;
    public TextMeshProUGUI feedbackText;  // The Timer

    [Header("Avatar Settings")]
    public Animator avatarAnimator;
    public Transform eyeContactTarget; 
    public LayerMask targetLayer;

    [Header("Mouse Settings")]
    public float mouseSensitivity = 200f;
    private Transform playerCamera; 
    private float xRotation = 0f;

    [Header("Session Stats")]
    public float totalEyeContactTime = 0f;
    private bool sessionActive = false; // Controls if we are "Playing"
    private int conversationStep = 0;
    private bool waitingForInput = false;

    void Start()
    {
        // 1. Setup
        if (Camera.main != null) playerCamera = Camera.main.transform;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // 2. UI Reset
        dialoguePanel.SetActive(false);
        if(endScreenPanel != null) endScreenPanel.SetActive(false); // Hide End Screen
        feedbackText.text = "";

        Invoke("StartConversation", 2.0f);
    }

    void Update()
    {
        // --- MOUSE LOOK ---
        if (playerCamera != null && Cursor.lockState == CursorLockMode.Locked)
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -70f, 70f);

            playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            
            if (playerCamera.parent != null) playerCamera.parent.Rotate(Vector3.up * mouseX);
            else playerCamera.Rotate(Vector3.up * mouseX); 
        }

        if (!sessionActive) return;

        CheckEyeContact();

        // --- WIN CONDITION: 30 SECONDS ---
        if (totalEyeContactTime >= 30.0f)
        {
            ShowEndScreen();
        }

        if (waitingForInput)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1)) HandleChoice(1);
            else if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2)) HandleChoice(2);
        }
    }

    void CheckEyeContact()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 10f, targetLayer))
        {
            if (hit.transform == eyeContactTarget || hit.transform.root == eyeContactTarget.root)
            {
                totalEyeContactTime += Time.deltaTime;
                feedbackText.text = "Eye Contact: " + totalEyeContactTime.ToString("F1") + " / 30s";
                feedbackText.color = Color.green;
            }
            else
            {
                feedbackText.color = Color.white;
            }
        }
    }

    void ShowEndScreen()
    {
        sessionActive = false; // Stop the game loop
        
        // Hide Gameplay UI
        dialoguePanel.SetActive(false);
        feedbackText.gameObject.SetActive(false); // Hide the timer

        // Show End Screen
        if (endScreenPanel != null) endScreenPanel.SetActive(true);

        // Unlock Mouse so they can click quit
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // --- CONVERSATION FLOW ---
    void StartConversation()
    {
        sessionActive = true;
        dialoguePanel.SetActive(true);
        nameText.text = "Therapist";
        conversationStep = 0;
        NewLine("Hi. I noticed you pausing at the door. It takes courage to step inside.", 
                "[1] Thank you.", "[2] I almost left.");
    }

    void HandleChoice(int choice)
    {
        waitingForInput = false;
        option1Text.text = "";
        option2Text.text = "";

        if (conversationStep == 0)
        {
            if (choice == 1) NewLine("You're welcome. Acknowledging that effort is the first step.", "", "");
            else             NewLine("But you didn't. You stayed. That proves you are stronger than the anxiety.", "", "");
            Invoke("Step1_Topic", 5.0f); 
        }
        else if (conversationStep == 1)
        {
            if (choice == 1) NewLine("It's common. The eyes feel like the most vulnerable part.", "", "");
            else             NewLine("Exactly. It triggers that 'fight or flight' instinct.", "", "");
            Invoke("Step2_Topic", 5.0f);
        }
        else if (conversationStep == 2)
        {
            if (choice == 1) NewLine("Great. Let's keep going. Just breathe.", "", "");
            else             NewLine("That's okay. Even 10 seconds is a victory.", "", "");
            
            // REMOVED: Invoke("EndSession")
            // Now we just wait for the timer in Update()!
        }
    }

    void Step1_Topic()
    {
        conversationStep = 1;
        NewLine("I can see you looking at me right now. Does it feel intense?", "[1] It feels judgmental.", "[2] I just want to look away.");
    }

    void Step2_Topic()
    {
        conversationStep = 2;
        NewLine("We've been talking for about 20 seconds. You're doing well. Shall we try for 10 more?", "[1] I can do it.", "[2] I'm overwhelmed.");
    }

    void NewLine(string speech, string opt1, string opt2)
    {
        bodyText.text = speech;
        if(avatarAnimator != null)
        {
            avatarAnimator.SetBool("isTalking", true);
            Invoke("StopTalking", 4.5f);
        }
        option1Text.text = opt1;
        option2Text.text = opt2;
        if (opt1 != "") waitingForInput = true;
    }

    void StopTalking()
    {
        if (avatarAnimator != null) avatarAnimator.SetBool("isTalking", false);
    }
}