using UnityEngine;

public class AttackHitBox : MonoBehaviour
{
    [SerializeField] private bool isDashAttack = false; // Set this to true if this attack is a dash attack

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the collider is a MushroomEnemy
        MushroomEnemy enemy = collision.GetComponent<MushroomEnemy>();
        if (enemy != null)
        {
            // Call TakeHit with isDashAttack argument
            enemy.TakeHit(isDashAttack);
        }
        //Check if the collider is a GoblinEnemy
        GoblinEnemy enemyg = collision.GetComponent<GoblinEnemy>();
        if (enemyg != null)
        {
            // Call TakeHit with isDashAttack argument
            enemyg.TakeHit(isDashAttack);
        }
        //Check if the collider is a SkeletonEnemy
        SkeletonEnemy enemys = collision.GetComponent<SkeletonEnemy>();
        if (enemys != null)
        {
            // Call TakeHit with isDashAttack argument
            enemys.TakeHit(isDashAttack);
        }
        //Check if the collider is a BatEnemy
        BatEnemy enemyb = collision.GetComponent<BatEnemy>();
        if (enemyb != null)
        {
            // Call TakeHit with isDashAttack argument
            enemyb.TakeHit(isDashAttack);
        }

    }
}
