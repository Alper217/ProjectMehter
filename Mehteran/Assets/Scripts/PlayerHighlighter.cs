using UnityEngine;

public class PlayerHighlighter : MonoBehaviour
{
    public static PlayerHighlighter Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void HighlightPlayer(GameObject player, Material selectedMaterial, bool isSelected)
    {
        Renderer renderer = player.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material = isSelected ? selectedMaterial : null;
        }
    }

    public void ClearHighlights()
    {
        // Tüm vurgulamalar temizlenebilir, örneðin tüm hexler normal hale getirilir
    }
}
