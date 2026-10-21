using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public TextMeshProUGUI testoPunti;
    public GameObject pannelloFine;
    public TextMeshProUGUI testoFine;

    private int punti = 0;

    void Awake()
    {
        Instance = this;
        Time.timeScale = 1f;
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        testoPunti.text = "Punti: 0";
    }

    public void AggiungiPunti(int p)
    {
        punti += p;
        testoPunti.text = "Punti: " + punti;
    }

    public void FineGioco()
    {
        testoFine.text = "Missione terminata! Punti: " + punti;
        pannelloFine.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}