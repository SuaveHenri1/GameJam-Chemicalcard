using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    // Mensagens e atores que esse trigger vai usar
    public Message[] messages;
    public Actor[] actors;

    // Chama o DialogueManager para iniciar o diálogo
    public void StartDialogue() 
    {
        FindFirstObjectByType<DialogueManager>().OpenDialogue(messages, actors);
    }
}

// Representa uma fala no diálogo
[System.Serializable]
public class Message {
    public int actorID;   // Índice do ator que fala
    public string message; // Conteúdo da fala
}

// Representa um personagem do diálogo
[System.Serializable]
public class Actor {
    public string name; // Nome exibido na UI
}