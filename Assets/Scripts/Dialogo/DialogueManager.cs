using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public TMP_Text actorName;
    public TMP_Text messageText;
    public RectTransform backgroundBox;

    Message[] currentMessages;
    Actor[] currentActors;
    int activeMessage = 0;
    public static bool isActive = false;

    public void OpenDialogue(Message[] messages, Actor[] actors) 
    {
        currentMessages = messages;
        currentActors = actors;
        activeMessage = 0;
        isActive = true;

        Debug.Log("Conversa Inicializada! Mensagens carregadas: " + messages.Length);
        DisplayMessage();
    }

    void DisplayMessage() 
    {
        Message messageToDisplay = currentMessages[activeMessage];
        messageText.text = messageToDisplay.message;

        Actor actorToDisplay = currentActors[messageToDisplay.actorID];
        actorName.text = actorToDisplay.name;
    }

    public void NextMessage() 
    {
        activeMessage++;
        if (activeMessage < currentMessages.Length) 
        {
            DisplayMessage();
        }
        else 
        {
            Debug.Log("Acabou a conversa!");
            isActive = false;
        }  
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isActive == true)
        {
            NextMessage();
        }
        
    }
}
