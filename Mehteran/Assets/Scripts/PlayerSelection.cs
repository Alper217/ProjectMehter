using UnityEngine;

public class PlayerSelection : MonoBehaviour
{
    public GameObject playerObject;
    public Material selectedMaterial;

    private bool isSelected = false;

    private GameManager gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>(); // GameManager'� bulur
    }

    void Update()
    {
        if (gameManager == null) return;

        // Sadece s�ras� gelen oyuncu hareket edebilir
        if ((gameManager.isPlayer1Turn && playerObject == gameManager.player1) ||
            (!gameManager.isPlayer1Turn && playerObject == gameManager.player2))
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    if (hit.collider.gameObject == playerObject)
                    {
                        ToggleSelection();
                    }
                }
            }
        }
    }

    public void HandlePlayerAction()
    {
        if (!isSelected) return;
        // Oyuncu se�iliyse ve s�ras� geldiyse hareket etme
        // Bu i�lem sadece s�ras� geldi�inde yap�labilir
        // Burada oyuncu hareket edebilir veya sald�r� yapabilir
    }

    private void ToggleSelection()
    {
        isSelected = !isSelected;
        PlayerHighlighter.Instance.HighlightPlayer(playerObject, selectedMaterial, isSelected);
    }
}
