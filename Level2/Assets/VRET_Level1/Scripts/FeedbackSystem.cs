using UnityEngine;
using UnityEngine.SceneManagement; // Needed to restart the level
using TMPro;

public class FeedbackSystem : MonoBehaviour
{
    [Header("UI Connections")]
    public GameObject feedbackPanel;
    public GameObject endScreen;
    public MonoBehaviour mouseLookScript; // We use MonoBehaviour so we can disable ANY look script
    public MonoBehaviour anxietyScript;
    public TextMeshProUGUI statsText;
    public AnxietyTracker playerTracker;

    void Start()
    {
        // Force the panel to hide when the level begins
        if (feedbackPanel != null) feedbackPanel.SetActive(false);
        if (endScreen != null) endScreen.SetActive(false);
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // 1. Call this when the level ends
    public void ShowFeedback()
    {
        // Turn on the UI
        feedbackPanel.SetActive(true);

        // Unlock the cursor so you can click the numbers
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Stop the Camera and Anxiety Timer
        if (mouseLookScript != null) mouseLookScript.enabled = false;
        if (anxietyScript != null) anxietyScript.enabled = false;
    }

    // 2. The Buttons call this function
    // The "score" variable comes from the Inspector settings
    public void SubmitRating(int score)
    {
        Debug.Log("FINAL REPORT: Patient rated anxiety " + score + "/5");

        // Save data here (future step)

        // Restart the level
        ShowEndScreen();
    }

    void ShowEndScreen()
    {
        if (feedbackPanel != null) feedbackPanel.SetActive(false);
        if (endScreen != null) endScreen.SetActive(true);

        // CALCULATE THE STATS
            if (playerTracker != null && statsText != null)
            {
                float avgJitter = playerTracker.GetAverageJitter();
                float peakJitter = playerTracker.highestJitter;

                // Format the string
                // "Avg Movement: 12.5 deg/s | Peak: 45.0 deg/s"
                statsText.text = $"Stability Report:\nAverage Tremor: {avgJitter:F1}\nPeak Tremor: {peakJitter:F1}";
            }
    }

    void FinishLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}