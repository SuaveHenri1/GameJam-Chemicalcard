using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ResultadoManager : MonoBehaviour
{

    public Cartas CartaAgua; // Referência à carta de água
    public Cartas CartaSal; // Referência à carta de sal
    public Cartas CartaNitratoPotassio; // Referência à carta de fertilizante

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
            default:
                Debug.Log("Combinação desconhecida.");
                return null;

        }
    }
}