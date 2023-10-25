using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class spikes : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            string currentScene = SceneManager.GetActiveScene().name;
            FindObjectOfType<SceneChanger>().TransitionToNewScene(currentScene);
            FindObjectOfType<Movement>().enabled = false;
            
            
        }
    }
}
