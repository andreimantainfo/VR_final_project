using UnityEngine;

[System.Serializable]
public class DialogueChoice
{
    [TextArea(2, 4)]
    public string choiceText;
    [TextArea(2, 4)]
    public string npcResponse;
}

[System.Serializable]
public class DialogueNode
{
    [TextArea(2, 4)]
    public string npcGreeting;
    public DialogueChoice choiceA;
    public DialogueChoice choiceB;
}