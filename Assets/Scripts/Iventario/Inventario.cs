using System.Collections.Generic;
using System.Collections;
using UnityEngine;

// Classe para representar o inventário do jogador, contendo uma lista de elementos.
public class Inventario : MonoBehaviour
{
     // Um dicionario para guardar os elementos e suas quantidades
    public Dictionary<Elemento, int> elementos;
    public GameObject interfaceInventario; // Referência ao objeto de interface do inventário

    bool inventarioAtivo = false; // Controle de visibilidade do inventário
    
    void Awake()
    {
        elementos = new Dictionary<Elemento, int>();
    } 


    void Update()
    {
        // Alterna a visibilidade do inventário ao pressionar a tecla "I"
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventarioAtivo = !inventarioAtivo; // Alterna o estado de visibilidade
            if (interfaceInventario != null)
            {
                interfaceInventario.SetActive(inventarioAtivo); // Ativa ou desativa a interface do inventário
            }
            Debug.Log($"Inventário {(inventarioAtivo ? "ativado" : "desativado")}.");
        }
        if (inventarioAtivo)
        {
            //Cursor.lockState = CursorLockMode.None; // Libera o cursor para interação com a interface
        }
    }

    // Método para adicionar um elemento ao inventário
    public void AdicionarElemento(Elemento elemento, int quantidade)
    {
        if (elementos.ContainsKey(elemento))
        {
            elementos[elemento] += quantidade; // Se o elemento já existe, incrementa a quantidade
        }
        else
        {
            elementos[elemento] = quantidade; // Caso contrário, adiciona o elemento ao dicionário
        }
        Debug.Log($"Adicionado {quantidade} de {elemento.nome}. Total agora: {elementos[elemento]}");
    }

    // Método para verificar se um elemento existe no inventário com uma quantidade suficiente
    public bool VerificarElemento(Elemento elemento, int quantidade)
    {
        return elementos.ContainsKey(elemento) && elementos[elemento] >= quantidade; // Verifica se o elemento existe e se a quantidade é suficiente
    }

    // Método para remover um elemento do inventário
    public void RemoverElemento(Elemento elemento, int quantidade)
    {
        if (elementos.ContainsKey(elemento))
        {
            elementos[elemento] -= quantidade; // Decrementa a quantidade do elemento
            if (elementos[elemento] <= 0)
            {
                elementos.Remove(elemento); // Remove o elemento se a quantidade for zero ou negativa
                Debug.Log($"{elemento.nome} removido do inventário.");
            }
            else
            {
                Debug.Log($"Removido {quantidade} de {elemento.nome}. Total agora: {elementos[elemento]}");
            }
        }
        else
        {
            Debug.LogWarning($"Não é possível remover {elemento.nome} - elemento não encontrado no inventário.");
        }
    }
}