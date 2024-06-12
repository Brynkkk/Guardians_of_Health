using UnityEngine;
using TMPro; 

public class PotionCountDisplay : MonoBehaviour
{
    [Header("TextMeshPro Component")]
    public TMP_Text potionCountText;

    private Player player;

    void Awake()
    {
        player = FindObjectOfType<Player>(); 
    }

    void Update()
    {
        if (player != null)
        {
            potionCountText.text = player.potionCount.ToString();
        }
    }
}

