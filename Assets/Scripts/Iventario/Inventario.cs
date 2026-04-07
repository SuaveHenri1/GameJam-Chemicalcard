using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI; // Importa a biblioteca para usar UI, como Image

// Classe para representar o inventário do jogador, contendo uma lista de elementos.
public class Inventario : MonoBehaviour
{
    public Moleculas[] slots; // Lista para armazenar os elementos e suas quantidades
    public Image[] SlotImagem; // Lista para armazenar as imagens dos elementos (opcional, para interface)
    public int[] SlotQuantidade; // Lista para armazenar as SlotQuantidade correspondentes aos elementos
    public Text[] SlotQuantidadeText; // Lista para armazenar os textos de quantidade (opcional, para interface)


    public List<string> ingredientesCrafting = new List<string>(); // Lista para armazenar os ingredientes selecionados para crafting
    public CraftingManager craftingManager; // Referência ao CraftingManager para acessar as receitas
    public GameObject conteudoCrafting; // O painel/pai que contém todas as imagens
    public Button botaoCraft; // Botão para enviar os ingredientes para o crafting


    public ResultadoManager resultadoManager; // Referência ao objeto de interface do crafting (se
    public Cartas[] slotsCartas; // Lista para armazenar as cartas e suas quantidades
    public Image[] SlotImagemCartas; // Lista para armazenar as imagens das cartas (opcional, para interface)
    public int[] SlotQuantidadeCartas; // Lista para armazenar as quantidades correspondentes às cartas
    public Text[] SlotQuantidadeTextCartas; // Lista para armazenar os textos de quantidade das cartas (opcional, para interface)

    void Start()
    {
        // Inicializa as listas de quantidade e imagens, se necessário
        for (int i=0; i < 118; i++)
        {
            SlotQuantidadeText[i].gameObject.SetActive(false); // Esconde o texto de quantidade inicialmente
            SlotImagem[i].gameObject.SetActive(false); // Esconde a imagem do slot inicialmente
        }
        craftingManager = GetComponent<CraftingManager>(); // Encontra o CraftingManager na cena
        resultadoManager = GetComponent<ResultadoManager>(); // Encontra o ResultadoManager na cena
        if (botaoCraft != null)
        {
            botaoCraft.onClick.AddListener(EnviarIngredientesCrafting); // Adiciona um listener para o botão de crafting
        }
    }

    // Método para adicionar um elemento ao inventário
    public void AdicionarElemento(Moleculas elemento, int quantidade)
    {
        string[] Elementos = {
            "H", "He",
            "Li", "Be", "B", "C", "N", "O", "F", "Ne",
            "Na", "Mg", "Al", "Si", "P", "S", "Cl", "Ar",
            "K", "Ca", "Sc", "Ti", "V", "Cr", "Mn", "Fe", "Co", "Ni", "Cu", "Zn", "Ga", "Ge", "As", "Se", "Br", "Kr",
            "Rb", "Sr", "Y", "Zr", "Nb", "Mo", "Tc", "Ru", "Rh", "Pd", "Ag", "Cd", "In", "Sn", "Sb", "Te", "I", "Xe",
            "Cs", "Ba", "La", "Ce", "Pr", "Nd", "Pm", "Sm", "Eu", "Gd", "Tb", "Dy", "Ho", "Er", "Tm", "Yb", "Lu",
            "Hf", "Ta", "W", "Re", "Os", "Ir", "Pt", "Au", "Hg", "Tl", "Pb", "Bi", "Po", "At", "Rn",
            "Fr", "Ra", "Ac", "Th", "Pa", "U", "Np", "Pu", "Am", "Cm", "Bk", "Cf", "Es", "Fm", "Md", "No", "Lr",
            "Rf", "Db", "Sg", "Bh", "Hs", "Mt", "Ds", "Rg", "Cn", "Nh", "Fl", "Mc", "Lv", "Ts", "Og"
        };

        int elementoIndex = System.Array.IndexOf(Elementos, elemento.nome); // Encontra o índice do elemento na tabela periódica
        if (elementoIndex == -1)
        {            
            Debug.LogWarning($"Elemento {elemento.nome} não encontrado na tabela periódica.");
            return; // Sai do método se o elemento não for encontrado
        }

        slots[elementoIndex] = elemento; // Armazena o elemento no slot correspondente

        SlotQuantidade[elementoIndex] += quantidade; // Incrementa a quantidade do elemento no slot correspondente

        SlotQuantidadeText[elementoIndex].text = SlotQuantidade[elementoIndex].ToString(); // Atualiza o texto de quantidade (opcional)
        SlotQuantidadeText[elementoIndex].gameObject.SetActive(true); // Mostra o texto de quantidade

        SlotImagem[elementoIndex].sprite = elemento.imagem; // Atualiza a imagem do slot (opcional)
        SlotImagem[elementoIndex].gameObject.SetActive(true); // Mostra a imagem do slot
        Button btn = SlotImagem[elementoIndex].GetComponent<Button>();
        if (btn == null)        {
            btn = SlotImagem[elementoIndex].gameObject.AddComponent<Button>();
        }
        btn.transition = Selectable.Transition.ColorTint; // Feedback visual
        btn.targetGraphic = SlotImagem[elementoIndex]; // A imagem que vai mudar de cor
        string simbolo = elemento.nome; // Captura o nome do elemento para usar no listener
        btn.onClick.RemoveAllListeners(); // Remove listeners anteriores para evitar múltiplas chamadas
        btn.onClick.AddListener(() => OnClickElemento(simbolo, elementoIndex)); // Adiciona um listener que chama o método de clique passando o nome do elemento e o índice

        Debug.Log($"Adicionado {quantidade} de {elemento.nome} no slot {elementoIndex}. Total agora: {SlotQuantidade[elementoIndex]}");
    }

    void OnClickElemento(string simbolo, int index)
    {
        if (SlotQuantidade[index] > 0)
        {
            // Criar uma nova imagem para representar o ingrediente selecionado
            GameObject novaImagem = new GameObject($"Ingrediente_{simbolo}"); // Nome do objeto para organização
            novaImagem.transform.SetParent(conteudoCrafting.transform); // Define o pai da nova imagem para o painel de ingredientes
            Image img = novaImagem.AddComponent<Image>(); // Adiciona o componente Image para mostrar a imagem do ingrediente
            img.sprite = SlotImagem[index].sprite; // Define a sprite da nova imagem como a do slot correspondente
            img.rectTransform.localScale = Vector3.one; // Garante que a escala seja 1 para evitar distorções
            Button btn = novaImagem.AddComponent<Button>(); // Adiciona um componente Button para permitir a remoção do ingrediente
            btn.transition = Selectable.Transition.ColorTint; // Feedback visual para o botão
            btn.targetGraphic = img; // A imagem que vai mudar de cor ao clicar
            btn.onClick.AddListener(() => {
                ingredientesCrafting.Remove(simbolo); // Remove o símbolo do ingrediente da lista de crafting
                Destroy(novaImagem); // Destroi a imagem do ingrediente selecionado
                SlotQuantidade[index]++; // Incrementa a quantidade do elemento de volta no slot correspondente
                SlotQuantidadeText[index].text = SlotQuantidade[index].ToString(); // Atualiza o texto de quantidade
                if (SlotQuantidade[index] > 0)                {
                    SlotQuantidadeText[index].gameObject.SetActive(true); // Mostra o texto de quantidade se houver mais do elemento
                    SlotImagem[index].gameObject.SetActive(true); // Mostra a imagem do slot se houver mais do elemento
                }
                Debug.Log($"Ingrediente {simbolo} removido dos ingredientes de crafting. Quantidade restante: {SlotQuantidade[index]}");
            }); // Adiciona um listener para remover o ingrediente ao clicar na imagem do ingrediente selecionado

            ingredientesCrafting.Add(simbolo); // Adiciona o símbolo do elemento à lista de ingredientes para crafting
            SlotQuantidade[index]--; // Decrementa a quantidade do elemento no slot correspondente
            SlotQuantidadeText[index].text = SlotQuantidade[index].ToString(); // Atualiza o texto de quantidade
            Debug.Log($"Elemento {simbolo} adicionado aos ingredientes de crafting. Quantidade restante: {SlotQuantidade[index]}");
            if (SlotQuantidade[index] <= 0)
            {
                SlotQuantidadeText[index].gameObject.SetActive(false); // Esconde o texto de quantidade
                SlotImagem[index].gameObject.SetActive(false); // Esconde a imagem do slot
                Debug.Log($"Elemento {simbolo} esgotado no slot {index}.");
            }
        }
        else
        {
            Debug.Log($"Elemento {simbolo} sem quantidade disponível para clicar.");
        }
    }

    void EnviarIngredientesCrafting()
    {
        string resultado = craftingManager.Craft(ingredientesCrafting); // Chama o método de crafting passando os ingredientes selecionados
        if (resultado != null)
        {
            for (int i=0; i < SlotImagemCartas.Length; i++)
            {
                if(slotsCartas[i] != null && slotsCartas[i].nome == resultado)
                {
                    SlotQuantidadeCartas[i]++; // Incrementa a quantidade da carta correspondente ao resultado
                    SlotQuantidadeTextCartas[i].text = SlotQuantidadeCartas[i].ToString(); // Atualiza o texto de quantidade da carta
                    SlotQuantidadeTextCartas[i].gameObject.SetActive(true); // Mostra o texto de quantidade da carta
                    Debug.Log($"Carta {resultado} já existe. Quantidade agora: {SlotQuantidadeCartas[i]}");
                    break; // Sai do método para evitar adicionar uma nova carta
                }else if (slotsCartas[i] == null)
                {
                    // Encontra o índice da carta correspondente ao resultado
                    Cartas cartaResultado = resultadoManager.ExibirResultado(resultado); // Obtém a carta correspondente ao resultado do crafting
                    if (cartaResultado != null)
                    {
                        slotsCartas[i] = cartaResultado; // Armazena a carta no slot correspondente
                        SlotImagemCartas[i].sprite = cartaResultado.imagem; // Atualiza a imagem do slot de cartas
                        SlotImagemCartas[i].gameObject.SetActive(true); // Mostra a imagem do slot de cartas
                        SlotQuantidadeCartas[i] = 1; // Define a quantidade inicial da carta como 1
                        SlotQuantidadeTextCartas[i].text = "1"; // Atualiza o texto de quantidade da carta
                        SlotQuantidadeTextCartas[i].gameObject.SetActive(true); // Mostra o texto de quantidade da carta
                        Debug.Log($"Carta {resultado} adicionada ao inventário. Quantidade: 1");
                        break; // Sai do método após adicionar a nova carta
                    }
                    else
                    {
                        Debug.LogError($"Carta para o resultado {resultado} não encontrada nas receitas.");
                    }
                }
            }
            conteudoCrafting.transform.DetachChildren(); // Remove as imagens dos ingredientes do painel de crafting
            ingredientesCrafting.Clear(); // Limpa a lista de ingredientes após tentar craftar
        }
        else
        {
            Debug.Log("Crafting falhou. Ingredientes não correspondem a nenhuma receita.");
        }
    }
}