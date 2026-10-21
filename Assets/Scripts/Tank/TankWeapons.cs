using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TankWeapons : MonoBehaviour
{
    [Header("Mitragliatrice")]
    public PoolManager poolMitragliatrice;
    public Transform puntoMitragliatrice;    // prima canna
    public Transform puntoMitragliatrice2;   // seconda canna (si alterna alla prima)
    public float cadenzaMitragliatrice = 0.1f;
    [Range(0f, 1f)] public float volumeMitra = 0.4f;

    [Header("Cannone")]
    public PoolManager poolCannone;
    public Transform puntoCannone;
    public float ricaricaCannone = 30f;
    [Range(0f, 1f)] public float volumeCannone = 1f;

    [Header("Rinculo cannone")]
    public Transform cannone;                    // pivot del cannone (figlio della torre)
    public float distanzaRinculo = 0.25f;
    public float durataRinculo = 0.08f;
    public float ritornoRinculo = 0.3f;

    public Image barraCannone;           // opzionale, Image Type = Filled

    bool mitraPremuta = false;
    bool usaSecondaCanna = false;
    float prossimoMitra = 0f;
    float prossimoCannone = 0f;
    Vector3 posizioneBaseCannone;
    Coroutine routineRinculo;

    void Start()
    {
        if (cannone != null)
            posizioneBaseCannone = cannone.localPosition;
    }


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

        // alternando le due canne della mitragliatrice a ogni colpo
        if (mitraPremuta && Time.time >= prossimoMitra)
        {
            prossimoMitra = Time.time + cadenzaMitragliatrice;

            Transform canna = usaSecondaCanna && puntoMitragliatrice2 != null ? puntoMitragliatrice2 : puntoMitragliatrice;

            Spara(poolMitragliatrice, canna);
            if (AudioManager.Instance != null)
                AudioManager.Instance.Play(AudioManager.Instance.shootMitra, volumeMitra);
            usaSecondaCanna = !usaSecondaCanna;
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

        if (AudioManager.Instance != null)
            AudioManager.Instance.Play(AudioManager.Instance.shootCannon, volumeCannone);

       
        if (cannone != null)
        {
            if (routineRinculo != null) StopCoroutine(routineRinculo);
            routineRinculo = StartCoroutine(Rinculo());
        }
    }

    public void Spara(PoolManager pool, Transform punto)
    {
        GameObject proiettile = pool.Prendi();
        proiettile.transform.SetPositionAndRotation(punto.position, punto.rotation);
        proiettile.SetActive(true);
    }

    public IEnumerator Rinculo()
    {
        Vector3 basePos = posizioneBaseCannone;

        float tempo = 0f;
        while (tempo < 1f)
        {
            tempo += Time.deltaTime / durataRinculo;

            Vector3 indietro = cannone.parent.InverseTransformDirection(-puntoCannone.forward).normalized;
            cannone.localPosition = basePos + indietro * (distanzaRinculo * Mathf.SmoothStep(0f, 1f, tempo));

            yield return null;
        }

        //ritorno morbido alla posizione originale
        Vector3 posizioneRinculo = cannone.localPosition;
        tempo = 0f;
        while (tempo < 1f)
        {
            tempo += Time.deltaTime / ritornoRinculo;
            cannone.localPosition = Vector3.Lerp(posizioneRinculo, basePos, Mathf.SmoothStep(0f, 1f, tempo));

            yield return null;
        }

        cannone.localPosition = basePos;
        routineRinculo = null;
    }
}