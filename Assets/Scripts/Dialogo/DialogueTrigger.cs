using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Message[] messages;
    public Actor[] actors;
    public Message[] messagemFinal;

    public void StartDialogue() 
    {
        FindFirstObjectByType<DialogueManager>().OpenDialogue(messages, actors);
    }
    public void FinalDialogue()
    {
        FindFirstObjectByType<DialogueManager>().OpenDialogue(messagemFinal, actors);
    }
}

[System.Serializable]
public class Message {
    public int actorID;
    public string message;
}

[System.Serializable]
public class Actor {
    public string name;
}