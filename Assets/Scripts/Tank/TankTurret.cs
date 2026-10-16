using UnityEngine;

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

    Vector2 look;
    float elevate;
    float yaw;
    float pitch;

    void Start()
    {
        yaw = torre.localEulerAngles.y;
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

        // torre: mouse in orizzontale
        yaw += look.x * sensibilitaMouse;
        torre.localRotation = Quaternion.Euler(0f, yaw, 0f);

        // cannone: W alza, S abbassa
        pitch += elevate * velocitaCannone * Time.deltaTime;
        pitch = Mathf.Clamp(pitch, angoloMin, angoloMax);
        cannone.localRotation = Quaternion.Euler(-pitch, 0f, 0f);
    }
}