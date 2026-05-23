using UnityEngine;

public class ProjectileShooter : MonoBehaviour
{
    public GameObject projectilePrefab;

    public void Fire(Vector3 position, Vector3 direction)
    {
        var go =Instantiate(projectilePrefab, position, Quaternion.identity);
        Destroy(go, 5f);
    }
}
