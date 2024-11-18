using UnityEngine;

public class PlayerSelection : MonoBehaviour
{
    public GameObject playerObject;
    public Material selectedMaterial;

    private bool isSelected = false;

    private GameManager gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>(); // GameManager'ý bulur
    }

    void Update()
    {
        if (gameManager == null) return;

        // Sadece sýrasý gelen oyuncu hareket edebilir
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
        // Oyuncu seçiliyse ve sýrasý geldiyse hareket etme
        // Bu iþlem sadece sýrasý geldiðinde yapýlabilir
        // Burada oyuncu hareket edebilir veya saldýrý yapabilir
    }

    private void ToggleSelection()
    {
        isSelected = !isSelected;
        PlayerHighlighter.Instance.HighlightPlayer(playerObject, selectedMaterial, isSelected);
    }
}
