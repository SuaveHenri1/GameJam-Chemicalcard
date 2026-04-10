using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI; // Importa a biblioteca para usar UI, como Image

// Classe para representar o inventário do jogador, contendo uma lista de elementos.
public class Inventario : MonoBehaviour
{
    public Moleculas[] slots; 
    public Image[] SlotImagem;
    //public int[] SlotQuantidade; 
    //public Text[] SlotQuantidadeText; 


    public List<string> ingredientesCrafting = new List<string>(); 
    public CraftingManager craftingManager; 
    public GameObject conteudoCrafting; 
    public Button botaoCraft; 


    public ResultadoManager resultadoManager;
    public BarraController Barra;

    public string[] Elementos = {
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

    void Start()
    {
        // Inicializa as listas de quantidade e imagens, se necessário
        for (int i=0; i < 118; i++)
        {
            SlotImagem[i].gameObject.SetActive(false); 
        }
        craftingManager = GetComponent<CraftingManager>(); 
        resultadoManager = GetComponent<ResultadoManager>();
        Barra = GetComponent<BarraController>();
        if (botaoCraft != null)
        {
            botaoCraft.onClick.AddListener(EnviarIngredientesCrafting); 
        }
    }

    // Método para adicionar um elemento ao inventário
    public void AdicionarElemento(Moleculas elemento)
    {
        int elementoIndex = System.Array.IndexOf(Elementos, elemento.nome); // Encontra o índice do elemento na tabela periódica
        if (elementoIndex == -1)
        {            
            Debug.LogWarning($"Elemento {elemento.nome} não encontrado na tabela periódica.");
            return; // Sai do método se o elemento não for encontrado
        }

        slots[elementoIndex] = elemento; // Armazena o elemento no slot correspondente

        // SlotQuantidade[elementoIndex] += quantidade; // Incrementa a quantidade do elemento no slot correspondente

        // SlotQuantidadeText[elementoIndex].text = SlotQuantidade[elementoIndex].ToString(); // Atualiza o texto de quantidade (opcional)
        // SlotQuantidadeText[elementoIndex].gameObject.SetActive(true); // Mostra o texto de quantidade


        SlotImagem[elementoIndex].sprite = elemento.imagem; // Atualiza a imagem do slot (opcional)
        Button btn = SlotImagem[elementoIndex].GetComponent<Button>();
        if (btn == null){
            SlotImagem[elementoIndex].gameObject.SetActive(true); // Mostra a imagem do slot
            btn = SlotImagem[elementoIndex].gameObject.AddComponent<Button>();
        }
        btn.transition = Selectable.Transition.ColorTint; // Feedback visual
        btn.targetGraphic = SlotImagem[elementoIndex]; // A imagem que vai mudar de cor
        string simbolo = elemento.nome; // Captura o nome do elemento para usar no listener
        btn.onClick.RemoveAllListeners(); // Remove listeners anteriores para evitar múltiplas chamadas
        btn.onClick.AddListener(() => OnClickElemento(simbolo, elementoIndex)); // Adiciona um listener que chama o método de clique passando o nome do elemento e o índice
    }

    void OnClickElemento(string simbolo, int index)
    {
        Debug.Log($"Clicando no elemento {simbolo}");
        // Criar uma nova imagem para representar o ingrediente selecionado
        GameObject novaImagem = new GameObject($"Ingrediente_{simbolo}"); // Nome do objeto para organização
        novaImagem.transform.SetParent(conteudoCrafting.transform); // Define o pai da nova imagem para o painel de ingredientes
        Image img = novaImagem.AddComponent<Image>(); // Adiciona o componente Image para mostrar a imagem do ingrediente
        img.sprite = SlotImagem[index].sprite; 
        img.rectTransform.localScale = Vector3.one; 
        Button btn = novaImagem.AddComponent<Button>(); 
        btn.transition = Selectable.Transition.ColorTint; 
        btn.targetGraphic = img; 
        btn.onClick.AddListener(() => {
            ingredientesCrafting.Remove(simbolo); 
            Destroy(novaImagem);
        }); 

        ingredientesCrafting.Add(simbolo);
    }
    //*
    void EnviarIngredientesCrafting()
    {
        string resultado = craftingManager.Craft(ingredientesCrafting); // Chama o método de crafting passando os ingredientes selecionados
        if (resultado != null)
        {
            Cartas cartas =  resultadoManager.ExibirResultado(resultado); // Mostra o resultado do crafting na interface
            Debug.Log($"Crafting bem-sucedido! Resultado: {resultado}");
            Barra.AddCarta(cartas); // Adiciona a carta resultante à barra de cartas
        }
        else
        {
            Debug.Log("Crafting falhou. Ingredientes não correspondem a nenhuma receita.");
        }

        conteudoCrafting.transform.DetachChildren();
        ingredientesCrafting.Clear();
    }
    //*/  
}