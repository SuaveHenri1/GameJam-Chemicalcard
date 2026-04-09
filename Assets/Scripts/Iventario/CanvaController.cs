using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvaController : MonoBehaviour

{
    public GameObject interfaceInventario; // Referência ao objeto de interface do inventário
    public GameObject interfaceBarra; // Referência ao objeto de interface do crafting (se necessário)
    public GameObject InterfaceDialogo; // Referência ao objeto de interface do diálogo
    bool inventarioAtivo = false; // Controle de visibilidade do inventário
    public CanvasGroup barraDialogoCanvasGroup;
 
    void Update()
    {
        // Alterna a visibilidade do inventário ao pressionar a tecla "I"
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventarioAtivo = !inventarioAtivo; // Alterna o estado de visibilidade
            if (interfaceInventario != null)
            {
                interfaceInventario.SetActive(inventarioAtivo); // Ativa ou desativa a interface do inventário
                interfaceBarra.SetActive(!inventarioAtivo); // Ativa ou desativa a interface da barra de crafting (se necessário)
            }
            Debug.Log($"Inventário {(inventarioAtivo ? "ativado" : "desativado")}.");
        }
        if (DialogueManager.isActive == true)
        {
            if (barraDialogoCanvasGroup != null)
            {
                barraDialogoCanvasGroup.alpha = 1f;  // Transparente
            }
        }
        else
        {
            if (barraDialogoCanvasGroup != null)
            {
                barraDialogoCanvasGroup.alpha = 0f;  // Transparente
            }
        }
        if (inventarioAtivo)
        {
            Cursor.lockState = CursorLockMode.None; // Libera o cursor para interação com a interface
            // pausar o jogo
             Time.timeScale = 0f; // Pausa o jogo
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked; // Trava o cursor para controle do personagem
            Time.timeScale = 1f; // Retoma o jogo
        }
    }
     void Start()
    {
        // Pega o CanvasGroup automaticamente se não foi atribuído
        if (InterfaceDialogo != null && barraDialogoCanvasGroup == null)
        {
            barraDialogoCanvasGroup = InterfaceDialogo.GetComponent<CanvasGroup>();
            if (barraDialogoCanvasGroup == null)
                barraDialogoCanvasGroup = InterfaceDialogo.AddComponent<CanvasGroup>();
        }
    }
}
