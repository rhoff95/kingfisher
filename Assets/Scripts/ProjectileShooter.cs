using UnityEngine;

public class ProjectileShooter : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Team team;
    public float projectileSpeed;
    public int damage;

    public void Fire(Vector3 position, float direction)
    {
        var go =Instantiate(projectilePrefab, position, Quaternion.identity);
        var projectile = go.GetComponent<Projectile>();

        projectile.SetDirection(direction);
        projectile.SetSpeed(projectileSpeed);
        projectile.SetTeam(team);
        projectile.SetDamage(damage);
        
        Destroy(go, 5f);
    }
}
