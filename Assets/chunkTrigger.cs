using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chunkTrigger : MonoBehaviour
{
    TileManager tileManager;
    bool updated = false;
    private void Start()
    {
        tileManager = FindAnyObjectByType<TileManager>();

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!updated)
            {
                Debug.Log("CHUNK TRIGGER" + gameObject.name);

                updated = true;
                FindAnyObjectByType<TileManager>().UpdateMultiplier(); 
                tileManager.SpawnChunk(transform.parent);
                gameObject.SetActive(false);
            }
        }

    }
}
