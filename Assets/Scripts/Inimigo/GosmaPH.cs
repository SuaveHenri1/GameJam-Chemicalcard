using UnityEngine;


[RequireComponent(typeof(Collider))]
public class GosmaPH : MonoBehaviour
{
    [Header("Estado atual Gosma")]
    [Range(-7, 7)]
    [Tooltip("Define qual o ph que tem")]
    public int phInicial = -7;

    private int phAtual;
    private Renderer gosmaRenderer;
    private float tempoUltimoAtaque = 0f;

    [Header ("Cores do PH")]
    [Tooltip("Define a cor para cada PH")]
    public Color corAcidoZero = Color.red;       
    public Color corAcido1 = new Color(1f, 0.5f, 0f); 
    public Color corAcido3 = Color.yellow;    
    public Color corNeutro = Color.green;       
    public Color corAlcalino9 = Color.cyan;       
    public Color corAlcalino10 = Color.blue;       
    public Color corAlcalino14 = new Color(0.5f, 0f, 0.5f);

    void Start()
    {
        gosmaRenderer = GetComponentInChildren<Renderer>();
        phAtual = phInicial;
        AtualizaCorPH();
    }

    public void RecebeAtaque(int phJogador)
    {
        if (Time.time - tempoUltimoAtaque < 0.5f) return;

        tempoUltimoAtaque = Time.time;

        phAtual = phAtual + phJogador;
        phAtual = Mathf.Clamp(phAtual, -7, 7);

        Debug.Log($" Sofreu ataque! Novo pH: {phAtual}");

        if (phAtual == 0)
        {
            NeutralizarCriatura();
        }
        else
        {
            AtualizaCorPH();
            
        }

    }

    private void NeutralizarCriatura()
    {
        Debug.Log("Gosma Neutralizada!");
        gosmaRenderer.material.color = corNeutro;

        Destroy(gameObject, 0.5f);
    }

    public void AtualizaCorPH()
    {
        Color novaCor = corNeutro;

        switch(phAtual)
        {
            case -7: novaCor = corAcidoZero; break;
            case -3: novaCor = corAcido1; break;
            case -2: novaCor = corAcido3; break;
            case 0: novaCor = corNeutro; break;
            case 2: novaCor = corAlcalino9; break;
            case 3: novaCor = corAlcalino10; break;
            case 7: novaCor = corAlcalino14; break;
        }

        gosmaRenderer.material.color = novaCor;
        gosmaRenderer.material.SetColor("_BaseColor", novaCor);
    }

}
