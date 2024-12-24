using UnityEngine;

public class EnemyCheck : MonoBehaviour
{
    public float detectionRadius = 1f;

    private void CheckForEnemies()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius);
        foreach (var collider in colliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                collider.gameObject.SetActive(false); 
                Debug.Log($"{collider.gameObject.name} etkisiz hale getirildi.");
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.gameObject.SetActive(false); // Düþmaný devre dýþý býrak
            Debug.Log($"{other.gameObject.name} etkisiz hale getirildi (OnTriggerEnter).");
        }
    }

    private void Update()
    {
        CheckForEnemies(); // Sürekli olarak etraftaki düþmanlarý kontrol et
    }
}
