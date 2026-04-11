using UnityEngine;
using System.Collections;

// Classe que controla o comportamento do jogador em um ambiente 3D
public class PlayerSimples : MonoBehaviour
{
    // Define os estados possíveis para a máquina de estados do personagem
    public enum EstadoPlayer { Idle, Run, Jump, Interact, Dash, Attack }
    public EstadoPlayer estadoAtual = EstadoPlayer.Idle; // Armazena o estado ativo
    
    private EstadoPlayer estadoAnterior = EstadoPlayer.Idle; // Armazena o estado do frame anterior

    public CharacterController controller; // Referência ao componente de movimento da Unity
    
    [Header("UI (Interface)")]
    public GameObject textoInteragir; // Objeto visual para o menu básico de interação

    [Header("Animação 2D")]
    public Animator animador2D; // Gerencia a troca de sprites animados do personagem

    [Header("Movimento")]
    public float speed = 6f; // Velocidade de deslocamento horizontal       
    public float mouseSensitivity = 300f; // sensibilidade da rotação da câmera
    public float jumpForce = 6f; // Força aplicada para o pulo
    public float gravity = -20f; // Valor da aceleração da gravidade

    [Header("Ação de Ataque (Tecla Q)")]
    public float raioAtaque = 2.0f; // Distância de alcance do golpe
    public float tempoAtaque = 0.5f; // Tempo de travamento durante o ataque
    private int hitsNoInimigo = 0; // Contador para o sistema de 3 acertos para vencer

    //[Header("Interação")]
    public float raioDeInteracao = 2.5f; // Distância para detectar itens coletáveis
    private bool tinhaObjetoPerto = false; // Controle de proximidade de itens

    [Header("Dash (Habilidade)")]
    public float dashSpeed = 20f; // Velocidade impulsionada do dash
    public float dashDuration = 0.2f; // Tempo total de duração do dash
    private float dashTimer = 0f; // Cronômetro regressivo do dash

    [Header("Combate e Status")]
    public int PHJogador = 0;

    private Vector3 velocity; // Vetor para controle de física vertical
    private bool estavaNoChao = true; // Variável para detectar transição de pouso (agora em uso)

    void Start()
    {
        // Inicializa o componente de controle e trava o mouse na tela
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        
        // Garante que o texto de interface comece oculto
        if (textoInteragir != null) textoInteragir.SetActive(false);
        Debug.Log("SISTEMA: Jogador inicializado corretamente.");
    }

    void Update()
    {
        //Bloqueia a tela quando dialogo está ativo
        if(DialogueManager.isActive == true)
            return;

        // Bloqueia comandos se o jogador estiver ocupado atacando ou interagindo
        if (estadoAtual == EstadoPlayer.Interact || estadoAtual == EstadoPlayer.Attack) return;

        // Gerencia o início do sistema de ataque por tecla
        if (Input.GetKeyDown(KeyCode.Q) && controller.isGrounded)
        {
            StartCoroutine(ExecutarAtaque());
        }

        // Gerencia o início do sistema de dash
        if (Input.GetKeyDown(KeyCode.F) && estadoAtual != EstadoPlayer.Dash)
        {
            estadoAtual = EstadoPlayer.Dash;
            dashTimer = dashDuration; 
            Debug.Log("MOVIMENTO: Dash iniciado.");
        }

        // Controla a duração do estado de dash
        if (estadoAtual == EstadoPlayer.Dash)
        {
            dashTimer -= Time.deltaTime;
            if (dashTimer <= 0)
            {
                estadoAtual = EstadoPlayer.Idle; 
                Debug.Log("MOVIMENTO: Dash finalizado.");
            }
        }

        // Executa os subsistemas organizados por funções
        ManejarInteracao();
        ManejarMovimentacaoEolhar();
        ManejarFisicaEpulo();

        // Monitora e loga mudanças na máquina de estados
        if (estadoAtual != estadoAnterior)
        {
            Debug.Log("ESTADO: Transição de [" + estadoAnterior + "] para [" + estadoAtual + "]");
            estadoAnterior = estadoAtual;
        }

        // Sincroniza o estado de movimento com o componente de animação
        if (animador2D != null)
        {
            animador2D.SetBool("Correndo", estadoAtual == EstadoPlayer.Run);
        }
    }

    // Detecta colisões físicas durante o movimento (ex: destruir baús no dash)
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.normal.y > 0.5f)
        {
            GosmaPH scriptGosma = hit.gameObject.GetComponent<GosmaPH>();
            if (scriptGosma != null)
            {
                scriptGosma.RecebeAtaque(PHJogador);
                QuicarNoInimigo();
            }
        }

        /*if (estadoAtual == EstadoPlayer.Dash && hit.collider.CompareTag("Interativo"))
        {
            Debug.Log("COLISÃO: Objeto destruído por impacto.");
            Destroy(hit.gameObject); 
            if (textoInteragir != null) textoInteragir.SetActive(false);
            tinhaObjetoPerto = false;
        }*/
    }

    // Corotina para processar a sequência de ataque e hits
    IEnumerator ExecutarAtaque()
    {
        estadoAtual = EstadoPlayer.Attack;
        Debug.Log("AÇÃO: Ataque em execução.");

        Collider[] inimigosAtingidos = Physics.OverlapSphere(transform.position + transform.forward, raioAtaque);

        foreach (Collider col in inimigosAtingidos)
        {
            if (col.CompareTag("Inimigo"))
            {
                hitsNoInimigo++;
                Debug.Log("COMBATE: Acerto " + hitsNoInimigo + " de 3.");

                if (hitsNoInimigo >= 3)
                {
                    Debug.Log("COMBATE: Inimigo eliminado.");
                    Destroy(col.gameObject);
                    hitsNoInimigo = 0;
                }
            }
        }

        yield return new WaitForSeconds(tempoAtaque);
        estadoAtual = EstadoPlayer.Idle;
    }

    // Gerencia a detecção e coleta de itens (interação)
    void ManejarInteracao()
    {
        Collider[] objetosPerto = Physics.OverlapSphere(transform.position, raioDeInteracao);
        GameObject objetoInterativo = null;

        foreach (Collider obj in objetosPerto)
        {
            /*if (obj.CompareTag("Interativo"))
            {
                objetoInterativo = obj.gameObject;
                break; 
            }*/
        }

        if (objetoInterativo != null && estadoAtual != EstadoPlayer.Dash)
        {
            if (!tinhaObjetoPerto)
            {
                tinhaObjetoPerto = true;
                Debug.Log("INTERAÇÃO: Item ao alcance.");
            }
            if (textoInteragir != null) textoInteragir.SetActive(true);

            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("INTERAÇÃO: Item coletado.");
                Destroy(objetoInterativo); 
                tinhaObjetoPerto = false; 
            }
        }
        else
        {
            if (tinhaObjetoPerto)
            {
                tinhaObjetoPerto = false;
                Debug.Log("INTERAÇÃO: Longe do item.");
            }
            if (textoInteragir != null) textoInteragir.SetActive(false);
        }
    }

    // Controla a rotação da câmera e o movimento WASD
    void ManejarMovimentacaoEolhar()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        transform.Rotate(0f, mouseX, 0f);

        float x = Input.GetAxis("Horizontal"); 
        float z = Input.GetAxis("Vertical");   

        float velocidadeAtual = (estadoAtual == EstadoPlayer.Dash) ? dashSpeed : speed;

        if (controller.isGrounded && estadoAtual != EstadoPlayer.Dash) 
        {
            estadoAtual = (x != 0 || z != 0) ? EstadoPlayer.Run : EstadoPlayer.Idle;
        }

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * velocidadeAtual * Time.deltaTime);
    }

    // Gerencia gravidade, pulo e detecção de pouso
    void ManejarFisicaEpulo()
    {
        // Verifica se o personagem está no solo por componente ou raio físico
        bool noChao = controller.isGrounded || Physics.Raycast(transform.position, Vector3.down, 1.1f);

        // Lógica que resolve o aviso CS0414: Usa o valor para detectar o momento do pouso
        if (noChao && !estavaNoChao)
        {
            Debug.Log("FÍSICA: Jogador aterrissou no solo.");
            estavaNoChao = true; // Atualiza o estado para o próximo frame
        }
        else if (!noChao && estavaNoChao)
        {
            estavaNoChao = false; // Detecta que o jogador saiu do chão
        }

        // Estabiliza a velocidade vertical ao tocar o chão
        if (noChao && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        else if (!noChao && estadoAtual != EstadoPlayer.Dash) 
        {
            estadoAtual = EstadoPlayer.Jump; 
        }

        // Aplica força de pulo se estiver no solo e pressionar Espaço
        if (Input.GetKeyDown(KeyCode.Space) && noChao && estadoAtual != EstadoPlayer.Dash) 
        {
            velocity.y = jumpForce;
            Debug.Log("FÍSICA: Pulo realizado.");
        }

        // Aplica a aceleração da gravidade frame a frame
        velocity.y += gravity * Time.deltaTime; 
        controller.Move(velocity * Time.deltaTime);
    }

    // Desenha uma esfera vermelha no editor para visualizar o alcance do ataque
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + transform.forward, raioAtaque);
    }

    public void SetPHJogador(int valor)
    {
        PHJogador = valor;
        Debug.Log($"STATUS: PH do jogador atualizado para {PHJogador}.");
    }

    public void QuicarNoInimigo()
    {
        velocity.y = jumpForce;
        estadoAtual = EstadoPlayer.Jump;
        Debug.Log("FÍSICA: Jogador quicou no inimigo!");
    }
}