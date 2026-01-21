using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 5f;
    public Rigidbody2D rb;
    public Elemen element;

    void Start()
    {
        rb.linearVelocity = transform.up * speed;
        
        // Debug: Verify element data
        if (element != null)
        {
            Debug.Log("Bullet created with element: " + element.elementName + ", Golongan: " + element.Golongan + ", Metalic: " + element.Metalic);
        }
        else
        {
            Debug.LogError("Bullet has no element data!");
        }
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        Debug.Log("Bullet hit: " + hitInfo.name);
        
        Enemy enemy = hitInfo.GetComponent<Enemy>();
        if (enemy != null)
        {
            // Debug.Log("Enemy found - Golongan: " + enemy.golongan + ", Metalic: " + enemy.metalic);
            // Debug.Log("Bullet element - Golongan: " + element.Golongan + ", Metalic: " + element.Metalic);

            if (gameManager.Instance.currentLevel >= 4)
            {
                // Check if bullet element matches enemy properties
                if (element != null && element.Golongan == enemy.golongan && element.Metalic == enemy.metalic)
                {
                    Debug.Log("Match! Both objects should be destroyed.");
                    enemy.DestroySelf();
                    gameManager.Instance.EnemyDefeated(); // Tambahkan score/progress
                }
                else
                {
                    Debug.Log("No match. Only bullet destroyed.");
                }
            }
            else
            {
                if(element != null && element.Golongan == enemy.golongan)
                {
                    Debug.Log("Match! Both objects should be destroyed.");
                    enemy.DestroySelf();
                    gameManager.Instance.EnemyDefeated(); // Tambahkan score/progress
                }
                else
                {
                    Debug.Log("No match. Only bullet destroyed.");
                }
            }
        }
        else
        {
            Debug.Log("Hit object is not an enemy.");
        }
        
        Destroy(gameObject); // Bullet selalu hancur setelah hit apapun
    }

    void Update()
    {
        if (transform.position.y > 10)
        {
            Destroy(gameObject);
        }
    }
}
