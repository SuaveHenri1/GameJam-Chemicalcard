using UnityEngine;

public class DetectaColisao : MonoBehaviour
{
    private PlataformaMovel plataformaPai;
    
    void Start()
    {
        // Encontra o componente PlataformaMovel no objeto PAI
        plataformaPai = GetComponentInParent<PlataformaMovel>();
        
        if (plataformaPai == null)
        {
            Debug.LogError("DetectorPlataforma: Não encontrou PlataformaMovel no pai!");
        }
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && plataformaPai != null)
        {
            Debug.Log("Jogador entrou na plataforma (detector filho), movendo junto...");
            plataformaPai.SetJogador(other.transform);
        }
    }
    
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && plataformaPai != null)
        {
            Debug.Log("Jogador saiu da plataforma (detector filho), parando movimento...");
            if (!plataformaPai.GetFimDoCaminho())
            {
                plataformaPai.SetIndoParaFrente(!plataformaPai.GetindoParaFrente()); // Inverte direção para voltar
            }
            plataformaPai.SetJogador(null);
            plataformaPai.SetFimDoCaminho(true); // Informa a plataforma que o jogador saiu, para ela parar ou voltar
        }
    }
}