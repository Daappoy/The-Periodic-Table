using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int golongan;
    public bool metalic;

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
