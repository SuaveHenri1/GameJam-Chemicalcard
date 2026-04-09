using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvaController : MonoBehaviour
{
    public GameObject interfaceInventario;
    public GameObject interfaceBarra;
    public CanvasGroup barraCanvasGroup; // Arraste o CanvasGroup aqui
    
    bool inventarioAtivo = false;
    
    void Start()
    {
        // Pega o CanvasGroup automaticamente se não foi atribuído
        if (interfaceBarra != null && barraCanvasGroup == null)
        {
            barraCanvasGroup = interfaceBarra.GetComponent<CanvasGroup>();
            if (barraCanvasGroup == null)
                barraCanvasGroup = interfaceBarra.AddComponent<CanvasGroup>();
        }
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventarioAtivo = !inventarioAtivo;
            
            if (interfaceInventario != null)
            {
                interfaceInventario.SetActive(inventarioAtivo);
                
                // Usa CanvasGroup para esconder sem desativar o GameObject
                if (barraCanvasGroup != null)
                {
                    barraCanvasGroup.alpha = inventarioAtivo ? 0f : 1f;  // Transparente
                    barraCanvasGroup.interactable = !inventarioAtivo;     // Não interage quando invisível
                    barraCanvasGroup.blocksRaycasts = !inventarioAtivo;   // Não bloqueia cliques
                }
            }
            Debug.Log($"Inventário {(inventarioAtivo ? "ativado" : "desativado")}.");
        }
        
        if (inventarioAtivo)
        {
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0f;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1f;
        }
    }
}