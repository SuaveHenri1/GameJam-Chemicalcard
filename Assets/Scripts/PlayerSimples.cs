using UnityEngine; // Biblioteca base da engine, obrigatória para usar componentes da Unity (MonoBehaviour, Vector3, Physics, etc).
using System.Collections; // Biblioteca necessária para utilizar Coroutines (como IEnumerator).

// Classe que controla o comportamento do jogador em um ambiente 3D. O MonoBehaviour permite anexar este script a um GameObject na cena.
public class PlayerSimples : MonoBehaviour
{
    // Define os estados possíveis para a máquina de estados do personagem. Evita que o jogador realize ações conflitantes simultaneamente.
    public enum EstadoPlayer { Idle, Run, Jump, Interact, Dash, Attack }
    public EstadoPlayer estadoAtual = EstadoPlayer.Idle; // Armazena o estado ativo do jogador neste frame.
    
    private EstadoPlayer estadoAnterior = EstadoPlayer.Idle; // Armazena o estado do frame anterior para detectar transições de estado.

    public CharacterController controller; // Referência ao componente de movimento da Unity, usado no lugar do Rigidbody para movimento arcade.
    
    // [Header] organiza as variáveis no painel (Inspector) da Unity, criando categorias visuais.
    [Header("UI (Interface)")]
    public GameObject textoInteragir; // Objeto visual para o menu básico de interação (ex: texto "Aperte E").

    [Header("Animação 2D")]
    public Animator animador2D; // Gerencia a troca de sprites animados do personagem baseado em parâmetros (ex: "Correndo").

    [Header("Movimento")]
    // [System.NonSerialized] mantém a variável pública para outros scripts acessarem, mas esconde ela do Inspector da Unity.
    [System.NonSerialized] public float speed = 6f; // Velocidade de deslocamento horizontal (Andar/Correr).
    public float mouseSensitivity = 300f; // Sensibilidade da rotação da câmera controlada pelo mouse.
    [System.NonSerialized] public float jumpForce = 8f; // Força inicial aplicada ao eixo vertical (Y) quando o jogador pula.
    [System.NonSerialized] public float gravity = -25f; // Valor constante da aceleração da gravidade puxando o jogador para baixo.

    [Header("Ação de Ataque (Tecla Q)")]
    public float raioAtaque = 2.0f; // Distância de alcance do golpe em formato de esfera à frente do jogador.
    public float tempoAtaque = 0.5f; // Tempo de travamento durante o ataque (duração da Coroutine).
    private int hitsNoInimigo = 0; // Contador para o sistema de 3 acertos para vencer um inimigo.

    //[Header("Interação")] (Sessão comentada temporariamente no Inspector)
    public float raioDeInteracao = 2.5f; // Distância de detecção esférica para encontrar itens coletáveis.
    private bool tinhaObjetoPerto = false; // Controle (flag) para não ativar/desativar a interface de texto a todo frame desnecessariamente.

    [Header("Dash (Habilidade)")]
    [System.NonSerialized] public float dashSpeed = 20f; // Velocidade impulsionada durante a duração do dash.
    [System.NonSerialized] public float dashDuration = 0.2f; // Tempo total de duração do estado de dash.
    private float dashTimer = 0f; // Cronômetro regressivo que controla o fim do dash.
    
    [Header("Combate e Status")]
    public int PHJogador = 0; // "Pontos de Habilidade" ou poder de ataque base do jogador.

    private Vector3 velocity; // Vetor responsável exclusivamente por simular a física vertical (Pulo e Gravidade).
    private bool estavaNoChao = true; // Variável para detectar transição de pouso (agora em uso) ou queda de beiradas.

    // Chamado uma vez no primeiro frame em que o script é ativado.
    void Start()
    {
        // Inicializa o componente de controle pegando a referência do GameObject atual.
        controller = GetComponent<CharacterController>();
        // Trava o ponteiro do mouse no centro da tela e o torna invisível (comum em jogos 3D).
        Cursor.lockState = CursorLockMode.Locked;
        
        // Garante que o texto de interface comece oculto, prevenindo que apareça ativo por erro na cena.
        if (textoInteragir != null) textoInteragir.SetActive(false);
        Debug.Log("SISTEMA: Jogador inicializado corretamente.");
    }

    // Chamado a cada frame (ex: 60 vezes por segundo).
    void Update()
    {
        // Cláusula de Guarda: Bloqueia a tela/movimento quando um dialogo (de outro script) está ativo.
        if(DialogueManager.isActive == true)
            return;

        // Cláusula de Guarda: Bloqueia comandos de movimento se o jogador estiver ocupado em ações restritivas.
        if (estadoAtual == EstadoPlayer.Interact || estadoAtual == EstadoPlayer.Attack) return;

        // Gerencia o início do sistema de ataque por tecla (Q). Só permite ataque se o jogador estiver no chão.
        if (Input.GetKeyDown(KeyCode.Q) && controller.isGrounded)
        {
            // Inicia a execução da Coroutine paralela que gerencia a janela de tempo do ataque.
            StartCoroutine(ExecutarAtaque());
        }

        // Gerencia o início do sistema de dash (F). Previne acionar dash se já estiver em um.
        if (Input.GetKeyDown(KeyCode.F) && estadoAtual != EstadoPlayer.Dash)
        {
            estadoAtual = EstadoPlayer.Dash; // Trava o estado para Dash.
            dashTimer = dashDuration; // Enche o cronômetro para iniciar a contagem regressiva.
            Debug.Log("MOVIMENTO: Dash iniciado.");
        }

        // Controla a duração do estado de dash enquanto ele está ocorrendo.
        if (estadoAtual == EstadoPlayer.Dash)
        {
            // Subtrai do cronômetro o tempo real percorrido desde o último frame (Time.deltaTime).
            dashTimer -= Time.deltaTime;
            
            // Verifica se o tempo de dash acabou.
            if (dashTimer <= 0)
            {
                estadoAtual = EstadoPlayer.Idle; // Devolve o controle base ao jogador.
                Debug.Log("MOVIMENTO: Dash finalizado.");
            }
        }

        // Delegação de métodos: Executa os subsistemas organizados por funções para manter o Update limpo.
        ManejarInteracao();
        ManejarMovimentacaoEolhar();
        ManejarFisicaEpulo();

        // Monitora e loga mudanças na máquina de estados para facilitar debugging de transições de animação.
        if (estadoAtual != estadoAnterior)
        {
            Debug.Log("ESTADO: Transição de [" + estadoAnterior + "] para [" + estadoAtual + "]");
            estadoAnterior = estadoAtual; // Atualiza a flag de controle para o próximo frame.
        }

        // Sincroniza o estado de movimento com o componente de animação (se ele existir na cena).
        if (animador2D != null)
        {
            // Ativa o booleano de correr se o estado atual da máquina for Run.
            animador2D.SetBool("Correndo", estadoAtual == EstadoPlayer.Run);
        }
    }

    // Detecta colisões físicas durante o movimento baseado no CharacterController.
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // Checa se a "seta normal" da superfície colidida aponta para cima. Isso garante que o player colidiu vindo de cima (caindo em cima do objeto).
        if (hit.normal.y > 0.5f)
        {
            // Tenta obter o componente de vida do inimigo no objeto colidido.
            GosmaPH scriptGosma = hit.gameObject.GetComponent<GosmaPH>();
            
            // Se o componente existir, confirmamos que é um inimigo válido.
            if (scriptGosma != null)
            {
                scriptGosma.RecebeAtaque(PHJogador); // Aplica dano ao inimigo.
                QuicarNoInimigo(); // Chama a função que impulsiona o player para cima.
                Destroy(hit.gameObject); // Destrói o inimigo imediatamente (Mecânica estilo Mario).
            }
        }

        // Código futuro/antigo comentado: Tratamento de colisão durante estado de Dash para destruir objetos iterativos.
        /*if (estadoAtual == EstadoPlayer.Dash && hit.collider.CompareTag("Interativo"))
        {
            Debug.Log("COLISÃO: Objeto destruído por impacto.");
            Destroy(hit.gameObject); 
            if (textoInteragir != null) textoInteragir.SetActive(false);
            tinhaObjetoPerto = false;
        }*/
    }

    // Corotina para processar a sequência de ataque e hits sem travar o Update principal do jogo.
    IEnumerator ExecutarAtaque()
    {
        estadoAtual = EstadoPlayer.Attack; // Trava as ações de movimento.
        Debug.Log("AÇÃO: Ataque em execução.");

        // Cria uma esfera de colisão invisível baseada na posição do jogador e sua direção frontal (forward). Retorna tudo que tocar nela.
        Collider[] inimigosAtingidos = Physics.OverlapSphere(transform.position + transform.forward, raioAtaque);

        // Percorre todos os colisores pegos na esfera.
        foreach (Collider col in inimigosAtingidos)
        {
            // Filtra os colisores para afetar apenas objetos com a Tag "Inimigo".
            if (col.CompareTag("Inimigo"))
            {
                hitsNoInimigo++; // Incrementa a contagem de hits em massa.
                Debug.Log("COMBATE: Acerto " + hitsNoInimigo + " de 3.");

                // Regra de negócio: Exige 3 acertos para de fato eliminar a entidade.
                if (hitsNoInimigo >= 3)
                {
                    Debug.Log("COMBATE: Inimigo eliminado.");
                    Destroy(col.gameObject); // Remove o objeto inimigo do mundo.
                    hitsNoInimigo = 0; // Reseta a contagem para o próximo alvo.
                }
            }
        }

        // Pausa APENAS a execução desta coroutine pelo tempo definido na variável, simulando o "peso" da arma/ataque.
        yield return new WaitForSeconds(tempoAtaque);
        
        // Após o tempo de espera, libera a máquina de estados para Idle.
        estadoAtual = EstadoPlayer.Idle;
    }

    // Gerencia a detecção e coleta de itens (interação) pelo jogador.
    void ManejarInteracao()
    {
        // Cria uma esfera de detecção para verificar se o jogador entrou na área de algum item.
        Collider[] objetosPerto = Physics.OverlapSphere(transform.position, raioDeInteracao);
        GameObject objetoInterativo = null;

        // Estrutura de repetição pronta para filtrar o primeiro item interativo detectado na área.
        foreach (Collider obj in objetosPerto)
        {
            /*if (obj.CompareTag("Interativo"))
            {
                objetoInterativo = obj.gameObject;
                break; // Encerra a busca ao encontrar o primeiro.
            }*/
        }

        // Se houver objeto interativo detectado e o jogador não estiver inviabilizado por um dash.
        if (objetoInterativo != null && estadoAtual != EstadoPlayer.Dash)
        {
            // Controle de UI: Se o jogador acabou de entrar na área, notifica e ativa o painel.
            if (!tinhaObjetoPerto)
            {
                tinhaObjetoPerto = true;
                Debug.Log("INTERAÇÃO: Item ao alcance.");
            }
            if (textoInteragir != null) textoInteragir.SetActive(true);

            // Aguarda o input (Tecla E) do jogador para validar a interação.
            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("INTERAÇÃO: Item coletado.");
                Destroy(objetoInterativo); // Simula coleta destruindo do mundo (ideal integrar com script de Inventário depois).
                tinhaObjetoPerto = false; // Reseta a flag já que o objeto sumiu.
            }
        }
        else // Ocorre se não houver objetos próximos válidos.
        {
            // Se havia objeto e agora não há, o jogador saiu da área. Reseta painéis.
            if (tinhaObjetoPerto)
            {
                tinhaObjetoPerto = false;
                Debug.Log("INTERAÇÃO: Longe do item.");
            }
            if (textoInteragir != null) textoInteragir.SetActive(false); // Oculta aviso de interação.
        }
    }

    // Controla a rotação da câmera (Horizontal) e a tradução do movimento WASD no espaço 3D.
    void ManejarMovimentacaoEolhar()
    {
        // Captura o input do eixo X do mouse e o multiplica pelo tempo e sensibilidade para rotação independente de FPS.
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        // Rotaciona fisicamente o GameObject no eixo Y (girar sobre o próprio eixo vertical).
        transform.Rotate(0f, mouseX, 0f);

        // Captura inputs normalizados (de -1 a 1) das teclas W/S e A/D ou controles de joystick.
        float x = Input.GetAxis("Horizontal"); 
        float z = Input.GetAxis("Vertical");   

        // If ternário que decide a velocidade a ser aplicada baseada no estado da máquina.
        float velocidadeAtual = (estadoAtual == EstadoPlayer.Dash) ? dashSpeed : speed;

        // Atualiza estado visual (Máquina) apenas se o jogador estiver com controle no solo.
        if (controller.isGrounded && estadoAtual != EstadoPlayer.Dash) 
        {
            // Se houver input em algum eixo, define como Run, caso contrário, Idle.
            estadoAtual = (x != 0 || z != 0) ? EstadoPlayer.Run : EstadoPlayer.Idle;
        }

        // Calcula o vetor de movimento baseado na direção local do objeto (forward/right) em vez do norte/sul global (Vector3.forward).
        Vector3 move = transform.right * x + transform.forward * z;
        
        // Solicita o movimento do CharacterController com base no vetor, velocidade decidida e tempo do frame.
        controller.Move(move * velocidadeAtual * Time.deltaTime);
    }

    // Gerencia o controle vertical independente (gravidade estática, pulos e forças Y).
    void ManejarFisicaEpulo()
    {
        // Verifica se o personagem está no solo. Usa Raycast redundante de 1.1 metros caso isGrounded do Controller falhe em quinas.
        bool noChao = controller.isGrounded || Physics.Raycast(transform.position, Vector3.down, 1.1f);

        // Lógica de flag para detectar o exato evento de impacto (primeiro frame tocando o chão).
        if (noChao && !estavaNoChao)
        {
            Debug.Log("FÍSICA: Jogador aterrissou no solo.");
            estavaNoChao = true; // Atualiza o estado para não disparar no frame seguinte.
        }
        else if (!noChao && estavaNoChao)
        {
            estavaNoChao = false; // Detecta o exato instante em que o jogador pisa no vazio saindo de uma borda.
        }

        // Estabiliza a velocidade vertical. Se estiver no chão e sendo puxado pra baixo, fixa num pequeno valor para evitar descer flutuando rampas.
        if (noChao && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        // Se estiver no ar caindo por gravidade fora de estado especial, garante que o estado da máquina é Jump (para animação).
        else if (!noChao && estadoAtual != EstadoPlayer.Dash) 
        {
            estadoAtual = EstadoPlayer.Jump; 
        }

        // Aplica força de pulo (injeção de Y positivo) se a barra de espaço for pressionada, dadas as condições seguras.
        if (Input.GetKeyDown(KeyCode.Space) && noChao && estadoAtual != EstadoPlayer.Dash) 
        {
            velocity.y = jumpForce;
            Debug.Log("FÍSICA: Pulo realizado.");
        }

        // Simulação matemática de gravidade manual. Acumula a força negativa gradativamente ao vetor Y.
        velocity.y += gravity * Time.deltaTime; 
        
        // Aplica o movimento vetorial final acumulado ao controlador na Unity.
        controller.Move(velocity * Time.deltaTime);
    }

    // Desenha componentes visuais na tela da Unity (não renderizados no jogo final) para ajudar a equipe de Level Design.
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red; // Define a cor da linha a ser desenhada.
        // Desenha uma esfera virtual que demonstra o alcance físico exato da zona configurada no raioAtaque.
        Gizmos.DrawWireSphere(transform.position + transform.forward, raioAtaque);
    }

    // Método de comunicação externa: permite que outros scripts de upgrade modifiquem a força de ataque base (PH).
    public void SetPHJogador(int valor)
    {
        PHJogador = valor;
        Debug.Log($"STATUS: PH do jogador atualizado para {PHJogador}.");
    }

    // Função pública de auxílio ao OnControllerColliderHit. Executa as diretrizes físicas do rebote.
    public void QuicarNoInimigo()
    {
        velocity.y = jumpForce; // Sobrescreve a queda simulando um pulo automático a partir da cabeça do inimigo.
        estadoAtual = EstadoPlayer.Jump; // Força a máquina para pular em consistência visual.
        Debug.Log("FÍSICA: Jogador quicou no inimigo!");
    }
}
