using UnityEngine;

public class TankMover : MonoBehaviour
{
    public Transform puntoB;
    public float velocita = 5f;
    public float velocitaRotazione = 90f;
    public float distanzaArrivo = 0.5f;

    [Header("Terreno")]
    public bool seguiTerreno = true;
    public float altezzaDalTerreno = 0f;

    Vector3 puntoA;
    bool andata = true;
    bool finito = false;

    void Start()
    {
        puntoA = transform.position;

        // avvia il loop del motore (una sola volta, non un PlayOneShot per frame)
        if (AudioManager.Instance != null)
            AudioManager.Instance.AvviaMotore();
    }

    void Update()
    {
        if (finito) return;

        Vector3 meta = andata ? puntoB.position : puntoA;
        Vector3 dir = meta - transform.position;
        dir.y = 0;

        // arrivato
        if (dir.magnitude < distanzaArrivo)
        {
            if (andata)
            {
                andata = false;                       // raggiunto B: torna indietro
            }
            else
            {
                finito = true;                        // tornato in A: fine gioco
                if (AudioManager.Instance != null) AudioManager.Instance.FermaMotore();
                GameManager.Instance.FineGioco();
            }
            return;
        }

        // ruota verso la meta
        Quaternion rotazioneMeta = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotazioneMeta, velocitaRotazione * Time.deltaTime);

        // avanza solo se è abbastanza allineato
        bool inMovimento = false;
        if (Vector3.Angle(transform.forward, dir) < 10f)
        {
            transform.position += transform.forward * velocita * Time.deltaTime;
            inMovimento = true;
        }

        // il motore resta in loop e cambia volume/pitch in base al movimento
        if (AudioManager.Instance != null)
            AudioManager.Instance.AggiornaMotore(inMovimento ? 1f : 0f);
    }
}