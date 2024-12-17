using UnityEngine;

public class CollectibleItem : MonoBehaviour
{
    
    public string itemName = "Mana";   
    public int scoreValue = 1;         

    
    public ItemCollector itemCollector;

    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) 
        {
            
            Debug.Log(itemName + " picked up!");
            itemCollector=other.GetComponent<ItemCollector>();
            
            if (itemCollector != null)
            {
                itemCollector.CollectItem(); 
            }

            
            Destroy(gameObject);
        }
    }
}
