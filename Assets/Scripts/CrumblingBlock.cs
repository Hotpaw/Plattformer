using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrumblingBlock : MonoBehaviour
{
    [SerializeField] private int health;
    [SerializeField] private GameObject child;

    private int startHealth;

    private void Start()
    {
        startHealth = health;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        health--;

        if (health <= 0)
            StartCoroutine("Crumbled");
    }

    IEnumerator Crumbled()
    {
        child.SetActive(false);
        yield return new WaitForSeconds(2f);
        health = startHealth;
        child.SetActive(true);
    }
}
