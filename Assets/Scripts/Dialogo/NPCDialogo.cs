using UnityEngine;

public class NPCDialogo : MonoBehaviour
{

    bool player_detection = false;
    public DialogueTrigger trigger;

    void Update()
    {
        if(player_detection && Input.GetKeyDown(KeyCode.E) && !DialogueManager.isActive)
        {
            Debug.Log("Conversa Inicializada!");
            trigger.StartDialogue();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.name == "Player")
        {
            player_detection = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.name == "Player")
        {
            player_detection = false;
        }
    }
}