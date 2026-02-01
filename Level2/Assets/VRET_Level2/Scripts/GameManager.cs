using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Win Conditions")]
    public float requiredTimeInRoom = 60f; // 1 minute
    public int requiredNPCCount = 3; // Must talk to 3 NPCs
    public NPCInteractable[] requiredNPCs; // Drag all NPCs here

    [Header("Anxiety Tracking")]
    public Transform playerHead; // Drag your VR camera or player head
    public float jitterSampleRate = 0.1f; // Sample every 0.1 seconds

    [Header("UI")]
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI winText;
    public Image backgroundFade; // Black background image
    public float fadeSpeed = 1f;

    [Header("Optional")]
    public string nextSceneName = ""; // Leave empty to just show win screen

    private float timeInRoom = 0f;
    private bool hasWon = false;
    private bool isFading = false;

    // Jitter tracking
    private Vector3 lastHeadPosition;
    private float totalJitter = 0f;
    private int jitterSamples = 0;
    private float jitterTimer = 0f;
    private bool isInConversation = false;

    void Start()
    {
        if (winText != null)
        {
            winText.gameObject.SetActive(false);
        }

        if (backgroundFade != null)
        {
            Color c = backgroundFade.color;
            c.a = 0f;
            backgroundFade.color = c;
            backgroundFade.gameObject.SetActive(true);
        }

        if (playerHead != null)
        {
            lastHeadPosition = playerHead.position;
        }

        // Validate setup
        if (requiredNPCs.Length == 0)
        {
            Debug.LogWarning("GameManager: No NPCs assigned! Add NPCs to the Required NPCs array.");
        }
    }

    void Update()
    {
        if (hasWon)
        {
            if (isFading)
            {
                FadeToBlack();
            }
            return;
        }

        // Track time in room
        timeInRoom += Time.deltaTime;

        // Update timer UI
        UpdateTimer();

        // Track jitter during conversations
        UpdateJitterTracking();

        // Check win conditions
        if (timeInRoom >= requiredTimeInRoom && GetTalkedToCount() >= requiredNPCCount)
        {
            Win();
        }
    }

    void UpdateTimer()
    {
        if (timerText != null)
        {
            float remaining = Mathf.Max(0, requiredTimeInRoom - timeInRoom);
            int minutes = Mathf.FloorToInt(remaining / 60f);
            int seconds = Mathf.FloorToInt(remaining % 60f);
            timerText.text = $"Time: {minutes:00}:{seconds:00}";
        }
    }

    void UpdateJitterTracking()
    {
        // Check if any NPC is currently in conversation
        isInConversation = false;
        foreach (NPCInteractable npc in requiredNPCs)
        {
            if (npc != null && npc.IsInConversation())
            {
                isInConversation = true;
                break;
            }
        }

        if (!isInConversation || playerHead == null) return;

        jitterTimer += Time.deltaTime;

        if (jitterTimer >= jitterSampleRate)
        {
            jitterTimer = 0f;

            // Calculate movement since last sample
            float movementDistance = Vector3.Distance(playerHead.position, lastHeadPosition);

            // Only count horizontal movement (XZ plane) to ignore walking
            Vector3 currentPos = playerHead.position;
            Vector3 lastPos = lastHeadPosition;
            currentPos.y = 0;
            lastPos.y = 0;
            float horizontalMovement = Vector3.Distance(currentPos, lastPos);

            totalJitter += horizontalMovement;
            jitterSamples++;

            lastHeadPosition = playerHead.position;
        }
    }

    int GetTalkedToCount()
    {
        int count = 0;
        foreach (NPCInteractable npc in requiredNPCs)
        {
            if (npc != null && npc.HasBeenTalkedTo())
            {
                count++;
            }
        }
        return count;
    }

    float GetAverageJitter()
    {
        if (jitterSamples == 0) return 0f;
        return 10000 * totalJitter / jitterSamples;
    }

    string GetAnxietyLevel()
    {
        float avgJitter = GetAverageJitter();

        if (avgJitter < 2000f)
            return "Very Low";
        else if (avgJitter < 5000f)
            return "Low";
        else if (avgJitter < 10000f)
            return "Moderate";
        else if (avgJitter < 100000f)
            return "High";
        else
            return "Very High";
    }

    void Win()
    {
        hasWon = true;
        isFading = true;

        int talkedTo = GetTalkedToCount();
        float avgJitter = GetAverageJitter();
        string anxietyLevel = GetAnxietyLevel();

        Debug.Log($"You win! Talked to {talkedTo} people. Average jitter: {avgJitter:F4}, Anxiety: {anxietyLevel}");

        // Hide timer
        if (timerText != null)
        {
            timerText.gameObject.SetActive(false);
        }

        // Start fade, then show text after fade is complete
        Invoke("ShowWinText", 1f / fadeSpeed);
    }

    void ShowWinText()
    {
        if (winText != null)
        {
            string anxietyLevel = GetAnxietyLevel();
            float avgJitter = GetAverageJitter();

            winText.text = $"Great job! You talked to everyone and spent time in the room.\n\n" +
                          $"Anxiety Level During Conversations: {anxietyLevel}\n" +
                          $"(Movement Score: {avgJitter:F4})\n\n" +
                          $"You're getting more comfortable with social situations!";

            winText.gameObject.SetActive(true);
        }

        // Optional: Load next scene after showing text
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            Invoke("LoadNextScene", 3f);
        }
    }

    void FadeToBlack()
    {
        if (backgroundFade == null) return;

        Color c = backgroundFade.color;
        c.a = Mathf.Lerp(c.a, 1f, Time.deltaTime * fadeSpeed);
        backgroundFade.color = c;
    }

    void LoadNextScene()
    {
        SceneManager.LoadScene(nextSceneName);
    }

    // Public method to restart
    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}