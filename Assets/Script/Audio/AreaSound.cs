using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaSound : MonoBehaviour
{
    [SerializeField] private int areaSoundIndex;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Player entered the area, playing BGM: " + areaSoundIndex);
            AudioManager.instance.PlayBGM(areaSoundIndex);
            AudioManager.instance.SetAreaBgmActive(false);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Player exited the area, stopping BGM: " + areaSoundIndex);
            AudioManager.instance.StopBGMAfterTime(areaSoundIndex);
            AudioManager.instance.SetAreaBgmActive(true);
        }
    }
}
