# 🎒 Sistema de Coleta e Inventário - Documentação

## 📌 Visão Geral

Este módulo do sistema é responsável por:

* Coletar elementos no mapa
* Armazenar elementos no inventário do jogador
* Gerenciar quantidades (adicionar, remover e verificar)

---

## 🧩 Classe: `Coleta`

### 📖 Responsabilidade

Representa um **objeto coletável no mapa**.

Quando o jogador entra em contato com ele:

* O elemento é adicionado ao inventário
* O objeto é destruído

---

## 🔧 Atributo

```csharp
public Elemento elemento;
```

* Define qual elemento será coletado
* Provavelmente contém propriedades como:

  * `nome`
  * `simbolo`
  * outros atributos

---

## 🔧 Método: `ColetarElementos`

```csharp
public void ColetarElementos(Collider other)
```

### ⚙️ Funcionamento

1. Verifica se o objeto que colidiu tem a tag `"Player"`
2. Tenta pegar o componente `Iventario` do jogador
3. Se existir:

   * Adiciona o elemento ao inventário
   * Exibe log
   * Destroi o objeto coletável
4. Se não existir:

   * Exibe aviso no console

---

## 🧪 Fluxo

```text
Jogador colide com objeto
        ↓
Verifica tag "Player"
        ↓
Busca componente Inventário
        ↓
Adiciona elemento
        ↓
Destrói objeto
```

---

## ⚠️ Observações Importantes

* O método **não está ligado automaticamente à colisão**

👉 Você provavelmente deve chamar ele dentro de:

```csharp
void OnTriggerEnter(Collider other)
{
    ColetarElementos(other);
}
```

---

## 🧩 Classe: `Inventario`

### 📖 Responsabilidade

Gerencia todos os elementos do jogador usando um **dicionário (Dictionary)**.

---

## 🔧 Estrutura de Dados

```csharp
private Dictionary<Elemento, int> elementos;
```

* **Chave (`Elemento`)** → tipo do elemento
* **Valor (`int`)** → quantidade

---

## ⚠️ Problema Crítico

```csharp
Iventario()
{
    elementos = new Dictionary<Elemento, int>();
}
```

❌ Isso **não funciona corretamente no Unity**

### 🚨 Por quê?

* `MonoBehaviour` **não usa construtores padrão**
* Esse código pode nunca ser chamado

### ✅ Correto:

```csharp
void Awake()
{
    elementos = new Dictionary<Elemento, int>();
}
```

---

## 🔧 Método: `AdicionarElemento`

```csharp
public void AdicionarElemento(Elemento elemento, int quantidade)
```

### ⚙️ Funcionamento

* Se o elemento já existe:

  * Soma a quantidade
* Se não:

  * Adiciona ao dicionário
* Exibe log com o total atualizado

---

## 🔧 Método: `VerificarElemento`

```csharp
public bool VerificarElemento(Elemento elemento, int quantidade)
```

### ⚙️ Funcionamento

* Verifica:

  * Se o elemento existe
  * Se há quantidade suficiente

### ✔️ Retorno

* `true` → possui o suficiente
* `false` → não possui

---

## 🔧 Método: `RemoverElemento`

```csharp
public void RemoverElemento(Elemento elemento, int quantidade)
```

### ⚙️ Funcionamento

1. Verifica se o elemento existe
2. Subtrai a quantidade
3. Se chegar a 0 ou menos:

   * Remove do dicionário
4. Caso contrário:

   * Atualiza a quantidade
5. Exibe logs

---

## 🧠 Fluxo Completo

```text
Coleta no mapa
      ↓
Coleta chama Inventário
      ↓
Elemento é adicionado
      ↓
Inventário atualiza quantidade
```

---

## 🚀 Possíveis Melhorias

### 🔹 1. Padronização de nomes

* `Iventario` → `Inventario`
* `adicionarElemento` → `AdicionarElemento`

---

### 🔹 2. Melhorar segurança do Dictionary

Usar `TryGetValue`:

```csharp
if (elementos.TryGetValue(elemento, out int qtd))
{
    elementos[elemento] = qtd + quantidade;
}
```

---

### 🔹 3. Evitar problemas com `Elemento` como chave

Se `Elemento` for uma classe, pode dar problema na comparação.

💡 Melhor usar:

* `string` (nome)
* ou `ID` único

---

### 🔹 4. Integração com UI

* Mostrar inventário na tela
* Atualizar automaticamente ao coletar

---

### 🔹 5. Sistema de eventos

Exemplo:

```csharp
public event Action OnInventarioAtualizado;
```

---

## 🧪 Exemplo de Uso

```csharp
if (inventario.VerificarElemento(agua, 2))
{
    inventario.RemoverElemento(agua, 2);
}
```

---

## 🔗 Integração com Crafting

Esse sistema conecta diretamente com seu crafting:

```text
Inventário → fornece ingredientes
        ↓
CraftingManager → verifica receitas
        ↓
Resultado → exibido
```

---

## 📌 Conclusão

Você já tem um sistema bem sólido com:

✅ Coleta no mapa
✅ Armazenamento eficiente
✅ Controle de quantidade
✅ Base perfeita para crafting

---