using UnityEngine;

public class Target : MonoBehaviour
{
    public int vitaMax = 30;
    public int punti = 10;

    [HideInInspector] public int indicePunto;
    int vita;
    bool morto;

    void OnEnable()
    {
        vita = vitaMax;   // ogni volta che torna in gioco riparte con la vita piena
        morto = false;
    }

    public void TakeDamage(int danno)
    {
        if (morto) return;   // evita punti/esplosioni doppi se arrivano più colpi insieme

        vita -= danno;
        if (vita <= 0)
        {
            morto = true;
            GameManager.Instance.AggiungiPunti(punti);
            TargetSpawner.Instance.ObiettivoDistrutto(indicePunto);
            if (AudioManager.Instance != null)
                AudioManager.Instance.Play(AudioManager.Instance.explosion);
            gameObject.SetActive(false);   // torna nel pool
        }
    }
}