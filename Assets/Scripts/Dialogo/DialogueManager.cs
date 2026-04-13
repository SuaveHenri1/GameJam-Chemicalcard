using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    // Referências para os elementos de UI (nome do personagem e texto da fala)
    public TMP_Text actorName;
    public TMP_Text messageText;
    public RectTransform backgroundBox;

    // Armazena as mensagens e atores da conversa atual
    Message[] currentMessages;
    Actor[] currentActors;

    // Controla qual mensagem está sendo exibida no momento
    int activeMessage = 0;

    // Indica se um diálogo está ativo (usado por outros scripts)
    public static bool isActive = false;

    // Inicia um novo diálogo
    public void OpenDialogue(Message[] messages, Actor[] actors) 
    {
        currentMessages = messages;
        currentActors = actors;
        activeMessage = 0;
        isActive = true;

        Debug.Log("Conversa Inicializada! Mensagens carregadas: " + messages.Length);
        DisplayMessage();
    }

    // Exibe a mensagem atual na tela
    void DisplayMessage() 
    {
        Message messageToDisplay = currentMessages[activeMessage];
        messageText.text = messageToDisplay.message;

        // Usa o actorID da mensagem para pegar o nome correto do ator
        Actor actorToDisplay = currentActors[messageToDisplay.actorID];
        actorName.text = actorToDisplay.name;
    }

    // Avança para a próxima mensagem
    public void NextMessage() 
    {
        activeMessage++;

        // Se ainda houver mensagens, continua o diálogo
        if (activeMessage < currentMessages.Length) 
        {
            DisplayMessage();
        }
        else 
        {
            // Finaliza o diálogo
            Debug.Log("Acabou a conversa!");
            isActive = false;
        }  
    }

    void Start()
    {
        // Não está sendo usado no momento
    }

    void Update()
    {
        // Permite avançar o diálogo pressionando espaço, apenas se estiver ativo
        if (Input.GetKeyDown(KeyCode.Space) && isActive == true)
        {
            NextMessage();
        }
    }
}