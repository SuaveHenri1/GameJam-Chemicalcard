using System.Collections.Generic;
using UnityEngine;

public class ReceitaLoad : MonoBehaviour
{
    public List<Receita> receitas; // Lista de receitas carregadas do JSON

    void Start()
    {
        // Carrega o arquivo JSON das receitas da pasta Resources
        TextAsset jsonText = Resources.Load<TextAsset>("receitas");
        if (jsonText != null)
        {
            // Desserializa o JSON para a estrutura de dados ReceitaLista
            ReceitaLista receitaLista = JsonUtility.FromJson<ReceitaLista>(jsonText.text);
            receitas = receitaLista.receitas; // Atribui a lista de receitas carregadas
            Debug.Log($"Receitas carregadas: {receitas.Count}");
        }
        else
        {
            Debug.LogError("Não foi possível carregar o arquivo de receitas.");
        }
    }
}