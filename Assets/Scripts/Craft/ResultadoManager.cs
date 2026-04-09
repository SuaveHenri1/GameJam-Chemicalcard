using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ResultadoManager : MonoBehaviour
{

    [Header("Cartas de Missoes")]
    public Cartas CartaAgua; // Referência à carta de água
    public Cartas CartaSal; // Referência à carta de sal
    public Cartas CartaNitratoPotassio; // Referência à carta de fertilizante

    [Header("Cartas de Combate")]
    public Cartas CartaAcidoCloridrico; // Referência à carta de ácido clorídrico
    public Cartas CartaHidroxidoSodio; // Referência à carta de hidróxido de sódio

    public Cartas ExibirResultado(string resultado)
    {
    
        switch (resultado)
        {
            case "Agua":
                Debug.Log("Você criou água!");
                return CartaAgua; // Retorna a carta de água
            case "Sal":
                Debug.Log("Você criou sal!");
                return CartaSal; // Retorna a carta de sal
            case "Nitrato de Potassio":
                Debug.Log("Você criou nitrato de potássio!");
                return CartaNitratoPotassio; // Retorna a carta de nitrato de potássio
            case "Acido Cloridrico":
                Debug.Log("Você criou ácido clorídrico!");
                return CartaAcidoCloridrico; // Retorna a carta de ácido clorídrico
            case "Hidroxido de Sodio":
                Debug.Log("Você criou hidróxido de sódio!");
                return CartaHidroxidoSodio; // Retorna a carta de hidróxido de sódio
            default:
                Debug.Log("Combinação desconhecida.");
                return null;

        }
    }
}