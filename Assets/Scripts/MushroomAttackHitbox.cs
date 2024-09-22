using System.Collections;
using UnityEngine;

public class MushroomAttackHitbox : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Call TakeDamage without any argument since it's a normal attack
            other.GetComponent<PlayerHealth>().TakeDamage();
        }
    }
}