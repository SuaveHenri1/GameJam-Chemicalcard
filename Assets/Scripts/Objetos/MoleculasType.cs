using UnityEngine;

public class MoleculasType : MonoBehaviour
{
    public Moleculas moleculasType;
    
    void Start()
    {
        // Garante configurações corretas automaticamente
        GarantirConfiguracoes();
    }
    
    void GarantirConfiguracoes()
    {
        // Garante Collider como Trigger
        Collider col = GetComponent<Collider>();
        if (col == null)
        {
            col = gameObject.AddComponent<BoxCollider>();
        }
        col.isTrigger = true;
        
        // Garante Rigidbody
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }
        rb.isKinematic = true;
        rb.useGravity = false;
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Inventario inventario = other.GetComponent<Inventario>();
            if (inventario != null)
            {
                inventario.AdicionarElemento(moleculasType, moleculasType.quantidade);
                Debug.Log($"Coletado: {moleculasType.nome}");
                Destroy(gameObject);
            }
        }
    }
}