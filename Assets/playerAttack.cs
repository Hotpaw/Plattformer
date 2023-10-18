using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class playerAttack : MonoBehaviour
{

    public float radius;
    public Transform attackPoint;
   public void Attack()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackPoint.position, radius);
        foreach(Collider2D collider in colliders)
        {
            if (collider.gameObject.CompareTag("Enemy"))
            {

                Debug.Log("ATTACK");
            }
        }
    }
    void OnDrawGizmosSelected()
    {
        // Display the explosion radius when selected
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(attackPoint.position, radius);
    }

}
