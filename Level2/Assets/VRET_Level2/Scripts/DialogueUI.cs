using UnityEngine;
using TMPro;

public class DialogueUI : MonoBehaviour
{
    [Header("UI References")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI npcGreetingText;
    public TextMeshProUGUI choiceAText;
    public TextMeshProUGUI choiceBText;
    public TextMeshProUGUI interactPromptText;
    public TextMeshProUGUI npcResponseText;

    [Header("Input")]
    public KeyCode interactKey = KeyCode.E;
    public KeyCode choiceAKey = KeyCode.A;
    public KeyCode choiceBKey = KeyCode.B;

    private NPCInteractable currentNearbyNPC = null;
    private NPCInteractable currentTalkingNPC = null;
    private DialogueNode currentNode;
    private bool waitingForChoice = false;

    void Start()
    {
        dialoguePanel.SetActive(false);
        interactPromptText.gameObject.SetActive(false);
    }

    void Update()
    {
        // Update interact prompt visibility
        interactPromptText.gameObject.SetActive(currentNearbyNPC != null && currentTalkingNPC == null);

        // Handle interaction
        if (currentNearbyNPC != null && currentTalkingNPC == null && Input.GetKeyDown(interactKey))
        {
            currentNearbyNPC.StartConversation();
        }

        // Handle dialogue choices
        if (waitingForChoice)
        {
            if (Input.GetKeyDown(choiceAKey))
            {
                OnChoiceSelected(true);
            }
            else if (Input.GetKeyDown(choiceBKey))
            {
                OnChoiceSelected(false);
            }
        }
    }

    public void SetCurrentNPC(NPCInteractable npc)
    {
        currentNearbyNPC = npc;
    }

    public void ClearCurrentNPC(NPCInteractable npc)
    {
        if (currentNearbyNPC == npc)
        {
            currentNearbyNPC = null;
        }
    }

    public NPCInteractable GetCurrentNPC()
    {
        return currentNearbyNPC;
    }

    public bool IsInConversation()
    {
        return currentTalkingNPC != null;
    }

    public void ShowDialogue(DialogueNode node, NPCInteractable npc)
    {
        currentNode = node;
        currentTalkingNPC = npc;
        waitingForChoice = true;

        dialoguePanel.SetActive(true);
        npcGreetingText.text = node.npcGreeting;
        choiceAText.text = "[F] " + node.choiceA.choiceText;
        choiceBText.text = "[G] " + node.choiceB.choiceText;
        npcResponseText.text = "";

        choiceAText.gameObject.SetActive(true);
        choiceBText.gameObject.SetActive(true);
    }

    void OnChoiceSelected(bool isChoiceA)
    {
        waitingForChoice = false;

        DialogueChoice selectedChoice = isChoiceA ? currentNode.choiceA : currentNode.choiceB;
        npcResponseText.text = selectedChoice.npcResponse;

        choiceAText.gameObject.SetActive(false);
        choiceBText.gameObject.SetActive(false);

        if (currentTalkingNPC != null)
        {
            currentTalkingNPC.OnChoiceSelected(isChoiceA);
        }

        Invoke("HideDialogue", 3f);
    }

    void HideDialogue()
    {
        dialoguePanel.SetActive(false);

        if (currentTalkingNPC != null)
        {
            currentTalkingNPC.EndConversation();
            currentTalkingNPC = null;
        }
    }
}