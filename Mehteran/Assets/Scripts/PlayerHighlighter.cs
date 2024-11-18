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
        // T�m vurgulamalar temizlenebilir, �rne�in t�m hexler normal hale getirilir
    }
}
