using UnityEngine;

public class NPCDialogo : MonoBehaviour
{

    bool player_detection = false;
    public DialogueTrigger trigger;
    public BarraController barraController;
    public string QualCarta;
    public bool MissaoComprida = false;
    public Cartas CartaNPC = null;
    public bool PlayerJaConversou = false;

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            barraController = player.GetComponent<BarraController>();
        }
    }

    void Update()
    {
        if(player_detection && Input.GetKeyDown(KeyCode.E)) // So entra quando o jogador estiver perto e apertar o E
        {
            // Estado na onde o jogador ja fez tudo com o NPC
            if(PlayerJaConversou && CartaNPC != null)
            {
                trigger.FinalDialogue();
                return;
            }

            // Se o jogador ja conversou com o NPC vai fazer a verificaçao se o jogador tem a carta
            if (PlayerJaConversou && CartaNPC == null)
            {
                // Se achar a carta ele completa a missao
                Cartas carta = barraController.BuscaCartaNaBarra(QualCarta);
                if (carta != null)
                {
                    CartaNPC = carta;
                    MissaoComprida = true;
                    trigger.FinalDialogue();
                    return;
                }
            }

            // Estado de inicio de dialgo sem ter completado a missao
            if(!DialogueManager.isActive && !MissaoComprida) // vai verificar se esta no meio do dialogo e se ja completou a missao
            {
                trigger.StartDialogue();
                if(DialogueManager.isActive) PlayerJaConversou = true;
                return;
            }
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