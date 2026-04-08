using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ResultadoManager : MonoBehaviour
{

    public Cartas CartaAgua;
    public Cartas CartaSal;
    public Cartas CartaAcidoCloridrico;
    public Cartas CartaHidroxidoSodio;

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