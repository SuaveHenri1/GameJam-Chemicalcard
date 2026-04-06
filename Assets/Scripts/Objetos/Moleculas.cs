using System.Collections.Generic; // Importa a biblioteca para usar listas
using UnityEngine; // Importa a biblioteca do Unity para usar ScriptableObject e outras funcionalidades

[CreateAssetMenu(fileName = "New Object", menuName = "Objects/Moleculas")] // Permite criar instâncias deste ScriptableObject no menu do Unity
public class Moleculas : ScriptableObject
{
    public string nome; // Nome da carta
    public int quantidade; // Quantidade da carta no inventário
    public Sprite imagem; // Imagem da carta
}