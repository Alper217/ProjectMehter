using UnityEngine;

public class TileManager : MonoBehaviour
{
    public GameObject currentHex;

    public void UpdateCurrentHex(GameObject player)
    {
        Ray ray = new Ray(player.transform.position, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit) && hit.collider.CompareTag("HexTile"))
        {
            currentHex = hit.collider.gameObject;
        }
    }
}
