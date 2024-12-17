using TMPro;
using UnityEngine;
using UnityEngine.UI;  

public class ItemCollector : MonoBehaviour
{
    public TextMeshProUGUI scoreText; 

    public int itemsCollected = 0; 
    public int totalItems = 5; 

    void Start()
    {
        scoreText.text = $"{itemsCollected} / {totalItems}";
        UpdateScoreText();
    }

   
    public void CollectItem()
    {
        itemsCollected++;
        

        UpdateScoreText();
    }

    
    void UpdateScoreText()
    {
        scoreText.text = $"{itemsCollected} / {totalItems}";
    }
}
