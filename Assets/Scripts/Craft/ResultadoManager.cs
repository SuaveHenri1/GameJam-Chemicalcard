using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ResultadoManager : MonoBehaviour
{
    public void ExibirResultado(string resultado)
    {
        switch (resultado)
        {
            case "Agua":
                Debug.Log("Você criou água!");
                break;
            case "Sal":
                Debug.Log("Você criou sal!");
                break;
            case "Explosao":
                Debug.Log("Você criou explosão!");
                break;
            default:
                Debug.Log("Combinação desconhecida.");
                break;
        }
    }
}