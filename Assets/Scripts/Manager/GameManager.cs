using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI")]
    public TextMeshProUGUI testoPunti;
    public GameObject pannelloInizio;
    public GameObject pannelloFine;
    public TextMeshProUGUI testoFine;

    int punti = 0;

    void Awake()
    {
        Instance = this;

        // Se esiste il pannello iniziale, il gioco resta fermo
        // finché il giocatore non preme "Inizia partita".
        Time.timeScale = (pannelloInizio != null) ? 0f : 1f;
    }

    void Start()
    {
        if (testoPunti != null) testoPunti.text = "Punti: 0";

        if (pannelloFine != null) pannelloFine.SetActive(false);

        if (pannelloInizio != null)
        {
            pannelloInizio.SetActive(true);
            MostraCursore(true);    // cursore libero per cliccare il bottone
        }
        else
        {
            MostraCursore(false);   // nessun menu: parte subito come prima
        }
    }

    public void IniziaPartita()
    {
        Time.timeScale = 1f;

        if (pannelloInizio != null) pannelloInizio.SetActive(false);
        MostraCursore(false);

        if (AudioManager.Instance != null) AudioManager.Instance.AvviaMotore();
    }

    public void AggiungiPunti(int p)
    {
        punti += p;
        if (testoPunti != null) testoPunti.text = "Punti: " + punti;
    }

    public void FineGioco()
    {
        if (testoFine != null)
            testoFine.text = "Missione terminata! Punti: " + punti;

        if (pannelloFine != null) pannelloFine.SetActive(true);

        Time.timeScale = 0f;
        MostraCursore(true);

        if (AudioManager.Instance != null) AudioManager.Instance.FermaMotore();
    }

    private void MostraCursore(bool visibile)
    {
        Cursor.lockState = visibile ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = visibile;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
