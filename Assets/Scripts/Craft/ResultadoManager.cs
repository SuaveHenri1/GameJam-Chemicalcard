using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ResultadoManager : MonoBehaviour
{

    public Sprite[] spritesResultados; // Array de sprites para os resultados

    public Cartas ExibirResultado(string resultado)
    {
    
        Cartas resultadoCarta = ScriptableObject.CreateInstance<Cartas>(); // Cria uma nova instância de Cartas para o resultado
        resultadoCarta.quantidade = 1; // Define a quantidade do resultado como 1 (pode ser ajustado conforme necessário)
        switch (resultado)
        {
            case "Agua":
                Debug.Log("Você criou água!");
                resultadoCarta.nome = "Água"; // Define o nome do resultado
                resultadoCarta.tipo = "Liquido"; // Define o tipo do resultado
                resultadoCarta.imagem = spritesResultados[0]; // Atribui o sprite correspondente ao resultado
                return resultadoCarta;
            case "Sal":
                Debug.Log("Você criou sal!");
                resultadoCarta.nome = "Sal"; // Define o nome do resultado
                resultadoCarta.tipo = "Solido"; // Define o tipo do resultado
                resultadoCarta.imagem = spritesResultados[1]; // Atribui o sprite correspondente ao resultado
                return resultadoCarta;
            case "Explosao":
                Debug.Log("Você criou explosão!");
                resultadoCarta.nome = "Explosão"; // Define o nome do resultado
                resultadoCarta.tipo = "Gasoso"; // Define o tipo do resultado
                resultadoCarta.imagem = spritesResultados[2]; // Atribui o sprite correspondente ao resultado
                return resultadoCarta;
            default:
                Debug.Log("Combinação desconhecida.");
                return null;

        }
    }
}