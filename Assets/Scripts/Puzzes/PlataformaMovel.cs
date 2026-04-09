using UnityEngine;
using System.Collections;

public class PlataformaMovel : MonoBehaviour
{
    [Header("Configuração de Movimento")]
    [SerializeField] private Vector3 direcao = Vector3.up;    // Direção do movimento (cima, lado, frente)
    [SerializeField] private float distancia = 5f;            // Distância total do movimento
    [SerializeField] private float velocidade = 2f;           // Velocidade do movimento
    
    [Header("Configuração Opcional")]
    [SerializeField] private bool iniciarParado = false;      // Começa parado?
    [SerializeField] private bool movimentaSoComJogador = false; // Move apenas quando o jogador estiver em cima?
    [SerializeField] private float tempoEspera = 0f;          // Tempo de pausa nos extremos (segundos)
    [SerializeField] private bool moverJogador = true;        // Jogador se move junto com a plataforma?
    
    // Controle interno
    private Vector3 posicaoInicial;
    private Vector3 posicaoFinal;
    private Vector3 ultimaPosicao; // Para calcular movimento da plataforma
    private bool indoParaFrente = true;
    private bool fimDoCaminho = false;
    private float temporizadorEspera = 0f;
    private bool esperando = false;
    
    // Referência ao jogador (para movê-lo junto)
    private Transform jogador;
    
    void Start()
    {
        // Guarda posição inicial
        posicaoInicial = transform.position;
        ultimaPosicao = transform.position;
        
        // Calcula posição final baseado na direção e distância
        posicaoFinal = posicaoInicial + direcao.normalized * distancia;
        
        // Se começar parado, fica no início
        if (iniciarParado)
        {
            indoParaFrente = false;
            esperando = true;
            temporizadorEspera = tempoEspera;
        }
    }
    
    void Update()
    {
        if(movimentaSoComJogador)
        {
            MovimentoJogador();
        }else
        {
            MovimentoPadrao();
        }
    }

    void MovimentoJogador()
    {
        if (!fimDoCaminho && jogador != null)
        {
            MoverPlataforma();
        }
        else if(fimDoCaminho && jogador == null) // Se o jogador sair da plataforma, ela para ou volta
        {
            MoverPlataforma();
        }
    }

    void MovimentoPadrao()
    {
         if (esperando)
        {
            temporizadorEspera -= Time.deltaTime;
            if (temporizadorEspera <= 0f)
            {
                esperando = false;
                // Inverte direção após pausa
                indoParaFrente = !indoParaFrente;
            }
            return;
        }

        // Move a plataforma
        MoverPlataforma();
    }
    
    void MoverPlataforma()
    {
        // Define destino baseado na direção atual
        Vector3 destino = indoParaFrente ? posicaoFinal : posicaoInicial;
        
        // Move suavemente em direção ao destino
        transform.position = Vector3.MoveTowards(transform.position, destino, velocidade * Time.deltaTime);
        
        // Verifica se chegou ao destino
        if (Vector3.Distance(transform.position, destino) < 0.01f)
        {
            // Se tiver tempo de espera, ativa pausa
            if (!movimentaSoComJogador && tempoEspera > 0f)
            {
                esperando = true;
                temporizadorEspera = tempoEspera;
            }
            else
            {
                // Sem pausa, inverte imediatamente
                indoParaFrente = !indoParaFrente;
            }
            fimDoCaminho = !fimDoCaminho; // Marca que chegou ao fim do caminho para controle de movimento com jogador
        }
    }
    
    // Método público para o detector filho definir o jogador
    public void SetJogador(Transform jogadorTransform)
    {
        jogador = jogadorTransform;
    }

    public void SetFimDoCaminho(bool valor)
    {
        fimDoCaminho = valor;
    }

    public bool GetFimDoCaminho()
    {
        return fimDoCaminho;
    }

    public void SetIndoParaFrente(bool valor)
    {
        indoParaFrente = valor;
    }

    public bool GetindoParaFrente()
    {
        return indoParaFrente;
    }
    
    void LateUpdate()
    {
        // Move o jogador junto com a plataforma
        if (jogador != null && moverJogador)
        {
            // Calcula quanto a plataforma se moveu nesse frame
            Vector3 movimento = transform.position - ultimaPosicao; // Simplificado
            jogador.position += movimento;
            // Versão correta (precisa guardar posição anterior)
        }
        ultimaPosicao = transform.position;
        
    }
}