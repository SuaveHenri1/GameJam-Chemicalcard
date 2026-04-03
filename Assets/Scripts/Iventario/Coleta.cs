using System.Collections.Generic;
using UnityEngine;

public class Coleta : MonoBehaviour
{
    public Elemento elemento; // Símbolo do elemento a ser coletado

    public void ColetarElementos(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Iventario iventario = other.GetComponent<Iventario>();
            if (iventario != null)
            {
                iventario.adicionarElemento(elemento, 1); // Adiciona 1 unidade do elemento ao inventário
                Debug.Log($"Coletado: {elemento.nome}");
                Destroy(gameObject); // Destroi o objeto coletável após a coleta
            }
            else
            {
                Debug.LogWarning("O jogador não possui um componente de inventário.");
            }
        }
    }
}