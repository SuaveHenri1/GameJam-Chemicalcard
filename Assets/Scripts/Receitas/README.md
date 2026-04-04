# 🧪 Sistema de Receitas (JSON) - Documentação

## 📌 Visão Geral

Este módulo define e carrega o sistema de **receitas de crafting** a partir de um arquivo JSON.

Ele permite:

* Definir receitas fora do código
* Facilitar manutenção e expansão
* Separar dados da lógica do jogo

---

## 🧩 Classe: `Receita`

### 📖 Responsabilidade

Representa **uma única receita de crafting**.

---

## 🔧 Estrutura

```csharp
[System.Serializable]
public class Receita
{
    public List<string> ingredientes;
    public string resultado;
}
```

### 📌 Campos

| Campo        | Tipo         | Descrição                      |
| ------------ | ------------ | ------------------------------ |
| ingredientes | List<string> | Lista de elementos necessários |
| resultado    | string       | Resultado da combinação        |

---

## 🧪 Exemplo

```json
{
  "ingredientes": ["H", "O"],
  "resultado": "Agua"
}
```

---

## 🧩 Classe: `ReceitaLista`

### 📖 Responsabilidade

Wrapper necessário para o `JsonUtility` do Unity.

---

## 🔧 Estrutura

```csharp
[System.Serializable]
public class ReceitaLista
{
    public List<Receita> receitas;
}
```

### ⚠️ Por que isso é necessário?

O `JsonUtility`:

* ❌ NÃO consegue ler listas diretamente no root do JSON
* ✅ Precisa de um objeto "container"

---

## 🧪 Exemplo de JSON Completo

Arquivo: `Resources/receitas.json`

```json
{
  "receitas": [
    {
      "ingredientes": ["H", "O"],
      "resultado": "Agua"
    },
    {
      "ingredientes": ["Na", "Cl"],
      "resultado": "Sal"
    },
    {
      "ingredientes": ["Na", "H2O"],
      "resultado": "Explosao"
    }
  ]
}
```

---

## 🧩 Classe: `ReceitaLoad`

### 📖 Responsabilidade

Carregar as receitas do JSON e disponibilizar para o sistema.

---

## 🔧 Atributo

```csharp
public List<Receita> receitas;
```

* Armazena todas as receitas carregadas

---

## 🔧 Método: `Start`

```csharp
void Start()
```

### ⚙️ Funcionamento

1. Carrega o arquivo JSON da pasta `Resources`
2. Verifica se foi encontrado
3. Converte o JSON em objeto C#
4. Armazena na lista `receitas`
5. Exibe log com a quantidade

---

## 🔧 Carregamento do JSON

```csharp
TextAsset jsonText = Resources.Load<TextAsset>("receitas");
```

### 📌 Importante

* O arquivo deve estar em:

```text
Assets/Resources/receitas.json
```

* NÃO colocar `.json` no nome ao carregar

---

## 🔧 Desserialização

```csharp
ReceitaLista receitaLista = JsonUtility.FromJson<ReceitaLista>(jsonText.text);
```

* Converte JSON → objeto C#
* Usa a classe wrapper (`ReceitaLista`)

---

## ⚠️ Tratamento de Erros

```csharp
if (jsonText != null)
```

### ✔️ Sucesso:

* Carrega normalmente

### ❌ Falha:

* Exibe erro no console

---

## 🧠 Fluxo do Sistema

```text
Arquivo JSON (Resources)
        ↓
ReceitaLoad.Start()
        ↓
JsonUtility.FromJson()
        ↓
Lista de receitas carregada
        ↓
Usada pelo CraftingManager
```

---

## 🔗 Integração com Crafting

```text
ReceitaLoad → fornece receitas
        ↓
CraftingManager → usa receitas
        ↓
Resultado → enviado para ResultadoManager
```