using UnityEngine;

public class NPCDialogo : MonoBehaviour
{

    bool player_detection = false;
    public DialogueTrigger trigger;
    public BarraController barraController;
    public string QualCarta;
    private bool MissaoComprida = false;
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
        if(player_detection && Input.GetKeyDown(KeyCode.E) && !DialogueManager.isActive && !MissaoComprida)
        {
            Debug.Log("Conversa Inicializada!");
            trigger.StartDialogue();
            CartaNPC = barraController.BuscaCartaNaBarra(QualCarta);
            if (CartaNPC != null)
            {
                MissaoComprida = true;
                trigger.FinalDialogue();
            }
        }
        else
        {
            trigger.FinalDialogue();
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