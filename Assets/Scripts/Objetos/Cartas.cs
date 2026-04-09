using System.Collections.Generic; // Importa a biblioteca para usar listas
using UnityEngine; // Importa a biblioteca do Unity para usar ScriptableObject e outras funcionalidades

[CreateAssetMenu(fileName = "New Object", menuName = "Objects/Carta")] // Permite criar instâncias deste ScriptableObject no menu do Unity
public class Cartas : ScriptableObject
{
    public string nome; // Nome da carta
    public string tipo; // tipo da carta 
    public int quantidade; // Quantidade da carta no inventário
    public int PH; // Valor de PH da carta
    public Sprite imagem; // Imagem da carta
}