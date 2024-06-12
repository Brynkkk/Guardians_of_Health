using UnityEngine;

public class SpawnOnDestroy : MonoBehaviour
{
    // Prefabs to spawn on destruction
    public GameObject prefab1;
    public GameObject prefab2;

    // This method is called when the attached GameObject is destroyed
    private void OnDestroy()
    {
        // Instantiate two prefabs at the GameObject's position and rotation
        if (prefab1 != null)
        {
            Instantiate(prefab1, transform.position, transform.rotation);
        }

        if (prefab2 != null)
        {
            Instantiate(prefab2, transform.position, transform.rotation);
        }
    }
}
