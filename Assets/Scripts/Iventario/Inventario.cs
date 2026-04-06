using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI; // Importa a biblioteca para usar UI, como Image

// Classe para representar o inventário do jogador, contendo uma lista de elementos.
public class Inventario : MonoBehaviour
{
     public Moleculas[] slots; // Lista para armazenar os elementos e suas quantidades
    public Image[] SlotImagem; // Lista para armazenar as imagens dos elementos (opcional, para interface)
    public int[] SlotQuantidade; // Lista para armazenar as SlotQuantidade correspondentes aos elementos
    public Text[] SlotQuantidadeText; // Lista para armazenar os textos de quantidade (opcional, para interface)


    void Start()
    {
        // Inicializa as listas de quantidade e imagens, se necessário
        foreach (Text quantidadeText in SlotQuantidadeText)
        {
            quantidadeText.gameObject.SetActive(false); // Esconde os textos de quantidade inicialmente
        }
    }

    // Método para adicionar um elemento ao inventário
    public void AdicionarElemento(Moleculas elemento, int quantidade)
    {
        for (int i=0; i < slots.Length; i++)
        {
            if (slots[i] == null || slots[i].nome == elemento.nome) // Verifica se o slot está vazio ou já contém o mesmo elemento
            {
                slots[i] = elemento; // Adiciona o elemento ao slot
                SlotQuantidade[i] += quantidade; // Incrementa a quantidade do elemento
                SlotImagem[i].sprite = elemento.imagem; // Atualiza a imagem do slot (opcional)
                SlotQuantidadeText[i].text = SlotQuantidade[i].ToString(); // Atualiza o texto de quantidade (opcional)
                SlotQuantidadeText[i].gameObject.SetActive(true); // Mostra o texto de quantidade
                Debug.Log($"Adicionado {quantidade} de {elemento.nome} no slot {i}. Total agora: {SlotQuantidade[i]}");
                break; // Sai do loop após adicionar o elemento
            }   
        }
    }
}