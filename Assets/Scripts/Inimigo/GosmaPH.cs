using UnityEngine;

[RequireComponent(typeof(Renderer))]
[RequireComponent(typeof(Collider))]
public class GosmaPH : MonoBehaviour
{
    [Header("Estado atual Gosma")]
    [Range(-3, 3)]
    [Tooltip("Define qual o ph que tem")]
    public int phInicial = -2;

    private int phAtual;
    private Renderer gosmaRenderer;
    private float tempoUltimoAtaque = 0f;

    [Header ("Cores do PH")]
    [Tooltip("Define a cor para cada PH")]
    public Color corMenos3 = Color.red;       
    public Color corMenos2 = new Color(1f, 0.5f, 0f); 
    public Color corMenos1 = Color.yellow;    
    public Color corZero = Color.white;       
    public Color corMais1 = Color.cyan;       
    public Color corMais2 = Color.blue;       
    public Color corMais3 = new Color(0.5f, 0f, 0.5f);

    void Start()
    {
        gosmaRenderer = GetComponent<Renderer>();
        phAtual = phInicial;
        AtualizaCorPH();
    }

    public void RecebeAtaque(int phJogador)
    {
        if (Time.time - tempoUltimoAtaque < 0.5f) return;

        tempoUltimoAtaque = Time.time;

        phAtual = phAtual + phJogador;
        phAtual = Mathf.Clamp(phAtual, -3, 3);

        Debug.Log($"[GOSMA] Sofreu ataque! Novo pH: {phAtual}");

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
        Debug.Log("[GOSMA] Gosma Neutralizada!");
        gosmaRenderer.material.color = corZero;

        Destroy(gameObject, 0.5f);
    }

    public void AtualizaCorPH()
    {
        Color novaCor = corZero;

        switch(phAtual)
        {
            case -3: novaCor = corMenos3; break;
            case -2: novaCor = corMenos2; break;
            case -1: novaCor = corMenos1; break;
            case 0: novaCor = corZero; break;
            case 1: novaCor = corMais1; break;
            case 2: novaCor = corMais2; break;
            case 3: novaCor = corMais3; break;
        }

        gosmaRenderer.material.color = novaCor;
    }

}
