using UnityEngine;
using UnityEngine.UI;

public class TankTurret : MonoBehaviour
{
    public Transform torre;      // ruota con il mouse
    public Transform cannone;    // pivot del cannone, figlio della torre

    [Header("Torre")]
    public float sensibilitaMouse = 0.1f;

    [Header("Cannone")]
    public float velocitaCannone = 40f;
    public float angoloMin = -10f;
    public float angoloMax = 30f;
    public Image cursor;


    Vector2 look;
    float elevate;
    float rotazioneTorretta;
    float inclinazioneTorretta;
    Quaternion rotazioneBaseTorre;

    void Start()
    {
        // salvo l'orientamento originale del modello 
        rotazioneBaseTorre = torre.localRotation;
    }

    void OnEnable()
    {
        InputManager.Instance.OnLook += SalvaLook;
        InputManager.Instance.OnElevate += SalvaElevate;
    }

    void OnDisable()
    {
        if (InputManager.Instance == null) return;
        InputManager.Instance.OnLook -= SalvaLook;
        InputManager.Instance.OnElevate -= SalvaElevate;
    }

    void SalvaLook(Vector2 v) => look = v;
    void SalvaElevate(float v) => elevate = v;

    void Update()
    {
        if (Time.timeScale == 0f) return;

        // torre: ruota attorno all'asse Z locale (verticale), mantenendo l'inclinazione del modello
        rotazioneTorretta += look.x * sensibilitaMouse;
        torre.localRotation = rotazioneBaseTorre * Quaternion.Euler(0f, 0f, rotazioneTorretta);

        // cannone: W alza, S abbassa
        inclinazioneTorretta += elevate * velocitaCannone * Time.deltaTime;
        inclinazioneTorretta = Mathf.Clamp(inclinazioneTorretta, angoloMin, angoloMax);
        cannone.localRotation = Quaternion.Euler(-inclinazioneTorretta, 0f, 0f);

        if (cursor != null)
            cursor.rectTransform.anchoredPosition = new Vector2(0f, -inclinazioneTorretta);
    }
}