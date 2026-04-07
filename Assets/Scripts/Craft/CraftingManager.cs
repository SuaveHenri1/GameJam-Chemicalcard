using System.Collections.Generic;
using UnityEngine;

public class CraftingManager : MonoBehaviour
{
    public List<Receita> receitasList; // Lista de receitas carregadas do JSON

    void Start()
    {
        // Carrega o arquivo JSON das receitas da pasta Resources
        TextAsset jsonText = Resources.Load<TextAsset>("receitas");
        if (jsonText != null)
        {
            // Desserializa o JSON para a estrutura de dados ReceitaLista
            ReceitaLista receitaLista = JsonUtility.FromJson<ReceitaLista>(jsonText.text);
            receitasList = receitaLista.receitas; // Atribui a lista de receitas carregadas
            Debug.Log($"Receitas carregadas: {receitasList.Count}");
        }
        else
        {
            Debug.LogError("Não foi possível carregar o arquivo de receitas.");
        }
    }

    public string Craft(List<string> ingredientes)
    {
        foreach (Receita receita in receitasList)
        {
            if (PodeCraftar(ingredientes, receita.ingredientes))
            {
                return receita.resultado; // Retorna o resultado da receita se os ingredientes corresponderem
            }
        }
        return null; // Retorna null se nenhuma receita corresponder aos ingredientes fornecidos
    }

    public bool PodeCraftar(List<string> ingredientes, List<string> receitas)
    {
        if (ingredientes.Count != receitas.Count)
        {
            return false; // Retorna false se o número de ingredientes não corresponder ao número de receitas
        }

        ingredientes.Sort(); // Ordena os ingredientes para comparação
        receitas.Sort(); // Ordena as receitas para comparação

        for (int i = 0; i < receitas.Count; i++)
        {
            if (ingredientes[i] != receitas[i])
            {
                return false; // Retorna false se algum ingrediente não corresponder à receita correspondente
            }
        }
        return true; // Retorna true se todos os ingredientes corresponderem às receitas
    }
}