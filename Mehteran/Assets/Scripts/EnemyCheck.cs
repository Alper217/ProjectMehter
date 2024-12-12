using UnityEngine;

public class EnemyCheck : MonoBehaviour
{
    GameObject detector;
    private GameObject currentHex;
    void UpdateCurrentHex()
    {
        Ray ray = new Ray(detector.transform.position, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit) && hit.collider.CompareTag("Enemy"))
        {
            Debug.Log("Burada Düþman var");
        }
    }
}
