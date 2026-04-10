using UnityEngine;

public class NPCDialogo : MonoBehaviour
{

    bool player_detection = false;
    public DialogueTrigger trigger;
    public BarraController barraController;
    public string QualCarta;
    public bool MissaoComprida = false;
    private Cartas CartaNPC;

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
            if (MissaoComprida) // Se o jogador ja completou a missao ele recebe a mensagem final
            {
                trigger.FinalDialogue();
            }else if(!DialogueManager.isActive) // vai verificar se esta no meio do dialogo e se ja completou a missao
            {
                Debug.Log("Conversa Inicializada!");
                trigger.StartDialogue();
                CartaNPC = barraController.BuscaCartaNaBarra(QualCarta);
                if (CartaNPC != null) // Se achar a carta ele completa a missao
                {
                    MissaoComprida = true;
                    trigger.FinalDialogue();
                }
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