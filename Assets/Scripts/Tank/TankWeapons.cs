using UnityEngine;
using UnityEngine.UI;

public class TankWeapons : MonoBehaviour
{
    [Header("Mitragliatrice")]
    public PoolManager poolMitragliatrice;
    public Transform puntoMitragliatrice;
    public float cadenzaMitragliatrice = 0.1f;

    [Header("Cannone")]
    public PoolManager poolCannone;
    public Transform puntoCannone;
    public float ricaricaCannone = 30f;
    public Image barraCannone;           // opzionale, Image Type = Filled

    bool mitraPremuta = false;
    float prossimoMitra = 0f;
    float prossimoCannone = 0f;

    void OnEnable()
    {
        InputManager.Instance.OnFireMitra += ImpostaMitra;
        InputManager.Instance.OnFireCannon += SparaCannone;
    }

    void OnDisable()
    {
        if (InputManager.Instance == null) return;
        InputManager.Instance.OnFireMitra -= ImpostaMitra;
        InputManager.Instance.OnFireCannon -= SparaCannone;
    }

    void ImpostaMitra(bool premuta) => mitraPremuta = premuta;

    void Update()
    {
        if (Time.timeScale == 0f) return;

        // finché il tasto è tenuto premuto spara con la cadenza impostata
        if (mitraPremuta && Time.time >= prossimoMitra)
        {
            prossimoMitra = Time.time + cadenzaMitragliatrice;
            Spara(poolMitragliatrice, puntoMitragliatrice);
        }

        // barra ricarica: 1 = pronto
        if (barraCannone != null)
        {
            float rimasto = Mathf.Max(0f, prossimoCannone - Time.time);
            barraCannone.fillAmount = 1f - rimasto / ricaricaCannone;
        }
    }

    void SparaCannone()
    {
        if (Time.timeScale == 0f) return;
        if (Time.time < prossimoCannone) return;   // ancora in ricarica

        prossimoCannone = Time.time + ricaricaCannone;
        Spara(poolCannone, puntoCannone);
    }

    void Spara(PoolManager pool, Transform punto)
    {
        GameObject proiettile = pool.Prendi();
        proiettile.transform.SetPositionAndRotation(punto.position, punto.rotation);
        proiettile.SetActive(true);
    }
}