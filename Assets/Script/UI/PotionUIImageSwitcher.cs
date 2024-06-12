using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PotionUIImageSwitcher : MonoBehaviour
{
    public Sprite imageA; // Image when potion count is above 0
    public Sprite imageB; // Image when potion count is 0

    private Image uiImage;
    private Player player;

    void Awake()
    {
        uiImage = GetComponent<Image>();
        player = FindObjectOfType<Player>(); 
    }

    void Update()
    {
        if (player != null)
        {
            if (player.potionCount > 0)
            {
                uiImage.sprite = imageA;
            }
            else
            {
                uiImage.sprite = imageB;
            }
        }
    }
}
