# 📦 Sistema de Crafting - Documentação

## 📌 Visão Geral

Este sistema implementa uma lógica simples de **crafting (combinação de elementos)** em Unity, onde:

* O jogador combina ingredientes
* O sistema verifica se existe uma receita válida
* Um resultado é retornado e exibido

---

## 🧩 Classe: `ResultadoManager`

### 📖 Responsabilidade

Responsável por **exibir o resultado do crafting** (atualmente via `Debug.Log`).

### 🔧 Método principal

```csharp
public void ExibirResultado(string resultado)
```

### ⚙️ Funcionamento

* Recebe uma `string` com o nome do resultado
* Usa um `switch` para identificar o que foi criado
* Exibe uma mensagem no console

### 📌 Casos implementados

| Resultado | Saída no Console           |
| --------- | -------------------------- |
| Agua      | "Você criou água!"         |
| Sal       | "Você criou sal!"          |
| Explosao  | "Você criou explosão!"     |
| Outro     | "Combinação desconhecida." |

### 💡 Observações

* Ideal para testes
* Pode ser evoluído para UI (Text, TMP, animações, efeitos, etc.)

---

## 🧩 Classe: `CraftingManager`

### 📖 Responsabilidade

Responsável por **verificar receitas e determinar o resultado do crafting**.

---

## 🔧 Atributo

```csharp
public ReceitaLista receitaLista;
```

* Contém todas as receitas disponíveis no jogo
* Espera-se que `ReceitaLista` tenha algo como:

```csharp
public List<Receita> receitas;
```

---

## 🔧 Método: `Craft`

```csharp
public string Craft(List<string> ingredientes)
```

### ⚙️ Funcionamento

1. Percorre todas as receitas disponíveis
2. Para cada receita:

   * Verifica se pode ser craftada (`PodeCraftar`)
3. Se encontrar:

   * Retorna o resultado da receita
4. Se não encontrar:

   * Retorna `null`

---

## 🔧 Método: `PodeCraftar`

```csharp
public bool PodeCraftar(List<string> ingredientes, List<string> receitas)
```

### ⚙️ Lógica

1. Verifica se a quantidade de itens é igual
2. Ordena ambas as listas (`Sort()`)
3. Compara item por item
4. Retorna:

   * `true` → combinação válida
   * `false` → combinação inválida

---

## ⚠️ Atenção (Importante)

### 🔥 Problema potencial

```csharp
ingredientes.Sort();
receitas.Sort();
```

Isso **modifica as listas originais**, o que pode causar bugs se você reutilizar elas depois.

### ✅ Melhor abordagem

Criar cópias:

```csharp
var ingredientesOrdenados = new List<string>(ingredientes);
var receitasOrdenadas = new List<string>(receitas);

ingredientesOrdenados.Sort();
receitasOrdenadas.Sort();
```

---

## 🧪 Exemplo de Uso

```csharp
List<string> ingredientes = new List<string> { "H", "O" };

string resultado = craftingManager.Craft(ingredientes);

if (resultado != null)
{
    resultadoManager.ExibirResultado(resultado);
}
else
{
    Debug.Log("Nada foi criado.");
}
```

---

## 🧠 Resumo do Fluxo

```text
Jogador escolhe ingredientes
        ↓
CraftingManager.Craft()
        ↓
Verifica receitas
        ↓
Retorna resultado (ou null)
        ↓
ResultadoManager exibe mensagem
```

---

## 🚀 Possíveis Melhorias

* 🔹 Usar JSON para receitas (como você comentou antes 👀)
* 🔹 Transformar `string` em `enum` (mais seguro)
* 🔹 Criar sistema de UI ao invés de Debug.Log
* 🔹 Permitir receitas com ordem irrelevante (já está parcialmente implementado)
* 🔹 Adicionar quantidade (ex: 2x H + 1x O)

---