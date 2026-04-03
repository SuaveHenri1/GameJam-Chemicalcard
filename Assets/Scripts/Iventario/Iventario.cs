using System.Collections.Generic;
using UnityEngine;

// Classe para representar o inventário do jogador, contendo uma lista de elementos.
public class Iventario : MonoBehaviour
{
     // Um dicionario para guardar os elementos e suas quantidades
    private Dictionary<Elemento, int> elementos;
    
    Iventario()
    {
        elementos = new Dictionary<Elemento, int>();
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