using System.Collections.Generic;
using UnityEngine;

public class Coleta : MonoBehaviour
{
    public Elemento elemento; // Símbolo do elemento a ser coletado

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Inventario inventario = other.GetComponent<Inventario>();
            if (inventario != null)
            {
                inventario.AdicionarElemento(elemento, 1); // Adiciona 1 unidade do elemento ao inventário
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