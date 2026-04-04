using System.Collections.Generic; // Importa a biblioteca para usar listas
using UnityEngine; // Importa a biblioteca do Unity para usar ScriptableObject e outras funcionalidades

[CreateAssetMenu(fileName = "New Object", menuName = "Inventario/Card Object")]
public class Objects : ScriptableObject
{
    public string nome; // Nome da carta
    public int poder; // Poder da carta
    public Sprite imagem; // Imagem da carta
}