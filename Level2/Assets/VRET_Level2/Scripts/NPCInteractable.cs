using UnityEngine;

public class NPCInteractable : MonoBehaviour
{
    [Header("Dialogue")]
    public DialogueNode[] dialogueNodes;
    private int currentNodeIndex = 0;
    private bool hasBeenTalkedTo = false; // NEW

    [Header("Look At Settings")]
    public Transform neckBone;
    [Range(0f, 1f)]
    public float lookAtBlend = 0.5f;
    public float lookAtSpeed = 2f;
    public float maxNeckRotation = 45f;

    [Header("Interaction")]
    public float interactionDistance = 3f;

    private Transform player;
    private bool isInConversation = false;
    private float currentBlend = 0f;
    private Quaternion neckAnimRotation;

    private static DialogueUI dialogueUI;

    void Start()
    {
        if (dialogueUI == null)
        {
            dialogueUI = FindObjectOfType<DialogueUI>();
        }

        player = Camera.main.transform;
    }

    void Update()
    {
        if (player == null || dialogueUI == null) return;

        float distance = Vector3.Distance(transform.position, player.position);
        bool inRange = distance <= interactionDistance;

        if (inRange && !isInConversation && !dialogueUI.IsInConversation())
        {
            dialogueUI.SetCurrentNPC(this);
        }
        else if (dialogueUI.GetCurrentNPC() == this && (!inRange || isInConversation))
        {
            dialogueUI.ClearCurrentNPC(this);
        }

        float targetBlend = isInConversation ? lookAtBlend : 0f;
        currentBlend = Mathf.Lerp(currentBlend, targetBlend, Time.deltaTime * lookAtSpeed);
    }

    void LateUpdate()
    {
        if (neckBone == null || player == null) return;

        neckAnimRotation = neckBone.rotation;

        if (currentBlend > 0.01f)
        {
            ApplyNeckLookAt();
        }
    }

    void ApplyNeckLookAt()
    {
        Vector3 playerHeadPos = player.position - Vector3.up * 0.5f;
        Vector3 neckToPlayer = playerHeadPos - neckBone.position;

        Quaternion neckLookRotation = Quaternion.LookRotation(neckToPlayer);

        Quaternion neckTargetLocal = Quaternion.Inverse(neckBone.parent.rotation) * neckLookRotation;
        Quaternion neckAnimLocal = Quaternion.Inverse(neckBone.parent.rotation) * neckAnimRotation;

        float neckAngle = Quaternion.Angle(neckAnimLocal, neckTargetLocal);
        if (neckAngle > maxNeckRotation)
        {
            neckTargetLocal = Quaternion.Slerp(neckAnimLocal, neckTargetLocal, maxNeckRotation / neckAngle);
        }

        Quaternion neckFinal = Quaternion.Slerp(neckAnimLocal, neckTargetLocal, currentBlend);
        neckBone.rotation = neckBone.parent.rotation * neckFinal;
    }

    public void StartConversation()
    {
        if (dialogueNodes.Length == 0) return;

        isInConversation = true;
        hasBeenTalkedTo = true; // NEW - Mark as talked to
        dialogueUI.ShowDialogue(dialogueNodes[currentNodeIndex], this);
    }

    public void OnChoiceSelected(bool isChoiceA)
    {
        currentNodeIndex = (currentNodeIndex + 1) % dialogueNodes.Length;
    }

    public void EndConversation()
    {
        isInConversation = false;
    }

    // NEW - Public method to check if talked to
    public bool HasBeenTalkedTo()
    {
        return hasBeenTalkedTo;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionDistance);
    }

    // Add this public method at the end of the NPCInteractable class

    public bool IsInConversation()
    {
        return isInConversation;
    }
}