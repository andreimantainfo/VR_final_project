using UnityEngine;
using UnityEngine.UI; 
using TMPro;          

public class PresentationManager : MonoBehaviour
{
    [Header("Setup")]
    public Camera playerCamera;
    public MicInput micSystem;
    public MouseLook mouseLookScript;

    [Header("UI Connections")]
    public Slider anxietySlider;   
    public Slider progressSlider;  
    public TMP_Text timerText;     
    public GameObject winScreen;   
    public GameObject loseScreen;  

    [Header("Audio & Effects")]
    public AudioSource sfxSource;    
    public AudioClip clapSound;      

    [Header("Game Difficulty")]
    public float maxTime = 120f;     // Total game time
    public float maxNoteLookTime = 10f; // LOSE if you stare at notes longer than this

    [Header("Game State (Debug)")]
    public bool isLookingAtAudience;
    public bool isLookingAtNotes;
    public bool isSpeaking;
    private float timeLeft;
    private float noteGazeTimer = 0f; // Tracks how long you've been staring at notes
    private bool gameEnded = false; 
    private bool uiVisible = true;

    [Range(0, 100)] public float anxietyLevel = 0f;
    [Range(0, 100)] public float speechProgress = 0f;

    void Start()
    {
        timeLeft = maxTime;
        if (playerCamera == null) playerCamera = Camera.main;
        if (mouseLookScript == null) mouseLookScript = playerCamera.GetComponent<MouseLook>();

        if(winScreen) winScreen.SetActive(false);
        if(loseScreen) loseScreen.SetActive(false);
        
        if(anxietySlider) anxietySlider.maxValue = 100;
        if(progressSlider) progressSlider.maxValue = 100;
    }

    void Update()
    {
        // Press 'H' to Toggle UI
        if (Input.GetKeyDown(KeyCode.H)) ToggleHUD();

        if (gameEnded) return; 

        CheckGaze();
        CheckMic();
        CalculateLogic();
        UpdateUI();
        CheckWinCondition(); 
    }

    void CheckGaze()
    {
        isLookingAtAudience = false;
        isLookingAtNotes = false;

        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 50f))
        {
            if (hit.collider.CompareTag("Audience")) isLookingAtAudience = true;
            if (hit.collider.CompareTag("Notes")) isLookingAtNotes = true;
        }
    }

    void CheckMic()
    {
        if (micSystem != null) isSpeaking = micSystem.loudness > 1.0f;
    }

    void CalculateLogic()
    {
        if (timeLeft > 0) timeLeft -= Time.deltaTime;

        // --- ANXIETY LOGIC ---
        if (isLookingAtAudience) 
        {
            anxietyLevel += 5f * Time.deltaTime; 
            noteGazeTimer = 0f; // Reset note timer
        }
        else if (isLookingAtNotes) 
        {
            anxietyLevel -= 25f * Time.deltaTime; 
            noteGazeTimer += Time.deltaTime; // COUNT UP!
        }
        else 
        {
            anxietyLevel -= 5f * Time.deltaTime;
            noteGazeTimer = 0f; // Reset note timer
        }

        // --- PROGRESS LOGIC ---
        if (isSpeaking) speechProgress += 5f * Time.deltaTime;

        anxietyLevel = Mathf.Clamp(anxietyLevel, 0, 100);
        speechProgress = Mathf.Clamp(speechProgress, 0, 100);
    }

    void UpdateUI()
    {
        if (anxietySlider != null) anxietySlider.value = anxietyLevel;
        if (progressSlider != null) progressSlider.value = speechProgress;

        if (timerText != null)
        {
            float minutes = Mathf.FloorToInt(timeLeft / 60);
            float seconds = Mathf.FloorToInt(timeLeft % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    void CheckWinCondition()
    {
        // 1. WIN
        if (speechProgress >= 100f) 
        {
            EndGame(true);
        }
        // 2. LOSE (Time Up OR Anxiety Maxed OR Stared at Notes too long)
        else if (timeLeft <= 0 || anxietyLevel >= 100f || noteGazeTimer >= maxNoteLookTime) 
        {
            if(noteGazeTimer >= maxNoteLookTime) Debug.Log("Lost due to avoidance (staring at notes)");
            EndGame(false);
        }
    }

    void EndGame(bool didWin)
    {
        gameEnded = true;
        
        if(anxietySlider) anxietySlider.gameObject.SetActive(true);
        if(progressSlider) progressSlider.gameObject.SetActive(true);

        if (didWin) winScreen.SetActive(true);
        else loseScreen.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        if(mouseLookScript) mouseLookScript.enabled = false;

        // Only play sound - removed the animation trigger causing the error
        if (sfxSource != null && clapSound != null) sfxSource.PlayOneShot(clapSound);
    }

    void ToggleHUD()
    {
        uiVisible = !uiVisible;
        if(anxietySlider) anxietySlider.gameObject.SetActive(uiVisible);
        if(progressSlider) progressSlider.gameObject.SetActive(uiVisible);
    }
}