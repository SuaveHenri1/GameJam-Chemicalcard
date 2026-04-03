using System.Collections.Generic;
using UnityEngine;

public class CraftingMager : MonoBehaviour
{
    public ReceitaLista receitaLista;

    public string Craft(List<string> ingredientes)
    {
        foreach (Receita receita in receitaLista.receitas)
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