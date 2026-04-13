using UnityEngine;

public class NPCDialogo : MonoBehaviour
{
    // Indica se o jogador está dentro da área de interação
    bool player_detection = false;

    // Referência ao trigger que contém o diálogo
    public DialogueTrigger trigger;

    void Update()
    {
        // Inicia diálogo ao pressionar "E", se o jogador estiver perto e não houver diálogo ativo
        if(player_detection && Input.GetKeyDown(KeyCode.E) && !DialogueManager.isActive)
        {
            Debug.Log("Conversa Inicializada!");
            trigger.StartDialogue();
        }
    }

    // Detecta quando o jogador entra na área do NPC
    private void OnTriggerEnter(Collider other)
    {
        if(other.name == "Player")
        {
            player_detection = true;
        }
    }

    // Detecta quando o jogador sai da área do NPC
    private void OnTriggerExit(Collider other)
    {
        if(other.name == "Player")
        {
            player_detection = false;
        }
    }
}