using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chunkTrigger : MonoBehaviour
{
    TileManager tileManager;

    private void Start()
    {
        tileManager = FindAnyObjectByType<TileManager>();
     
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            tileManager.SpawnChunk(transform.parent);
            gameObject.SetActive(false);
        }
    }
}
