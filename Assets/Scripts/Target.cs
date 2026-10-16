using UnityEngine;

public class Target : MonoBehaviour
{
    public int vitaMax = 30;
    public int punti = 10;

    [HideInInspector] public int indicePunto;
    int vita;

    void OnEnable()
    {
        vita = vitaMax;   // ogni volta che torna in gioco riparte con la vita piena
    }

    public void TakeDamage(int danno)
    {
        vita -= danno;
        if (vita <= 0)
        {
            GameManager.Instance.AggiungiPunti(punti);
            TargetSpawner.Instance.ObiettivoDistrutto(indicePunto);
            gameObject.SetActive(false);   // torna nel pool
        }
    }
}