using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvaController : MonoBehaviour

{
    public GameObject interfaceInventario; // Referência ao objeto de interface do inventário
    public GameObject interfaceBarra; // Referência ao objeto de interface do crafting (se necessário)
    bool inventarioAtivo = false; // Controle de visibilidade do inventário
 
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
}