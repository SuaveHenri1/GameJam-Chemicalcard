using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Importa a biblioteca para usar UI, como Image e Text


public class BarraController : MonoBehaviour
{
    [Header("Referências da Barra")]
    [SerializeField] private Cartas[] slotsCartas;
    [SerializeField] private Image[] SlotImagemCartas;
    [SerializeField] private int[] SlotQuantidadeCartas;
    [SerializeField] private Text[] SlotQuantidadeTextCartas;

    [Header("Feedback Visual")]
    [SerializeField] private Color corSelecionada = Color.yellow;  // Cor quando selecionada
    [SerializeField] private Color corNormal = Color.white;        // Cor normal
    
    [Header("Configuração")]
    [SerializeField] private bool loop = true;  // Volta ao início quando chegar no fim
    private PlayerSimples playerSimples; // Referência ao inventário para acessar as cartas
    
    // Controle interno
    private int cartaSelecionada = 0;
    private float ultimoClique = 0f;
    private Image imagemAtual;


    void Start()
    {
        // Inicializa as listas de quantidade e imagens, se necessário
        slotsCartas = new Cartas[SlotImagemCartas.Length]; // Inicializa o array de cartas com o mesmo tamanho das imagens
        SlotQuantidadeCartas = new int[SlotImagemCartas.Length]; // Inicializa o array de quantidades com o mesmo tamanho das imagens
        playerSimples = GetComponent<PlayerSimples>(); // Obtém a referência ao PlayerSimples para acessar o PH do jogador

        foreach (Text txt in SlotQuantidadeTextCartas)
        {
            txt.gameObject.SetActive(false); // Esconde os textos de quantidade no início
        }
    }
    void Update()
    {
        if (SlotImagemCartas == null || SlotImagemCartas.Length == 0) return;
        
        // ========== SCROLL DO MOUSE ==========
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        
        if (scroll != 0)
        {
            if (scroll > 0)
            {
                // Scroll para cima: carta anterior
                NavegarCarta(-1);
            }
            else if (scroll < 0)
            {
                // Scroll para baixo: próxima carta
                NavegarCarta(1);
            }
        }

        // ========== TECLAS NUMÉRICAS (1-9) ==========
        for (int i = 0; i < SlotImagemCartas.Length && i < 9; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                SelecionarCarta(i);
            }
        }
        
        // ========== BOTÃO ESQUERDO ==========
        if (Input.GetMouseButtonDown(0))  // 0 = botão esquerdo
        {
            AtivarCarta();
        }
    }
    
    // Navega entre as cartas
    void NavegarCarta(int direcao)
    {
        int novaCarta = cartaSelecionada + direcao;
        
        // Verifica limites
        if (novaCarta < 0)
        {
            if (loop)
                novaCarta = SlotImagemCartas.Length - 1;  // Volta para última
            else
                return;  // Não faz nada se não tiver loop
        }
        else if (novaCarta >= SlotImagemCartas.Length)
        {
            if (loop)
                novaCarta = 0;  // Volta para primeira
            else
                return;
        }
        
        SelecionarCarta(novaCarta);
    }
    
    // Seleciona uma carta específica
    void SelecionarCarta(int indice)
    {
        // Remove seleção da carta anterior
        if (SlotImagemCartas[cartaSelecionada] != null)
        {
            SlotImagemCartas[cartaSelecionada].color = corNormal;
        }
        
        // Atualiza índice
        cartaSelecionada = indice;
        
        // Aplica seleção na nova carta
        if (SlotImagemCartas[cartaSelecionada] != null)
        {
            SlotImagemCartas[cartaSelecionada].color = corSelecionada;
            imagemAtual = SlotImagemCartas[cartaSelecionada];
            
            // Debug
            Debug.Log($"Carta selecionada: {cartaSelecionada}");
        }
    }
    
    // Ativa a carta atualmente selecionada
    void AtivarCarta()
    {
        // Verifica se tem carta válida
        if (slotsCartas[cartaSelecionada] != null)
        {
            SlotQuantidadeCartas[cartaSelecionada]--;
            SlotQuantidadeTextCartas[cartaSelecionada].text = SlotQuantidadeCartas[cartaSelecionada].ToString();
            playerSimples.SetPHJogador(slotsCartas[cartaSelecionada].PH);
            if (SlotQuantidadeCartas[cartaSelecionada] <= 0)
            {
                // Remove a carta do slot
                slotsCartas[cartaSelecionada] = null;
                SlotImagemCartas[cartaSelecionada].sprite = null;
                SlotQuantidadeTextCartas[cartaSelecionada].gameObject.SetActive(false);
            }
            return;
        }
        
    }

    public void AddCarta(Cartas carta)
    {
        for(int i=0; i < slotsCartas.Length; i++)
        {
            if(slotsCartas[i] == null || slotsCartas[i].nome == carta.nome)
            {
                slotsCartas[i] = carta;
                SlotImagemCartas[i].sprite = carta.imagem;
                // SlotImagemCartas[i].gameObject.SetActive(true);
                SlotQuantidadeCartas[i] += carta.quantidade;
                SlotQuantidadeTextCartas[i].text = SlotQuantidadeCartas[i].ToString();
                SlotQuantidadeTextCartas[i].gameObject.SetActive(true);
                Debug.Log($"Carta {carta.nome} adicionada à barra no slot {i}.");
                break;
            }
        }
    }

}