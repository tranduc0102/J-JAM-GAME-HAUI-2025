using System.Collections;
using System.Collections.Generic;
using pooling;
using UnityEngine;

public class SpawnLevel : MonoBehaviour
{
    [SerializeField] private List<Transform> levels;
    private Transform currentLevel;

    public void SpawmLevel(int index)
    {
        if(index > 8) return;
        DestroyMap();
        currentLevel = Instantiate(levels[index], transform.position, Quaternion.identity, transform);
    }

    public void DestroyMap()
    {
        if (currentLevel != null)
        {
            Destroy(currentLevel.gameObject);
            currentLevel = null;
        }
    }
}
