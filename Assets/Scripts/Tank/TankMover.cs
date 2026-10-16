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
                GameManager.Instance.FineGioco();
            }
            return;
        }

        // ruota verso la meta
        Quaternion rotazioneMeta = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotazioneMeta, velocitaRotazione * Time.deltaTime);

        // avanza solo se è abbastanza allineato
        if (Vector3.Angle(transform.forward, dir) < 10f)
        {
            transform.position += transform.forward * velocita * Time.deltaTime;
        }

        // resta appoggiato al terreno
        if (seguiTerreno && Terrain.activeTerrain != null)
        {
            Terrain t = Terrain.activeTerrain;
            Vector3 pos = transform.position;
            pos.y = t.SampleHeight(pos) + t.transform.position.y + altezzaDalTerreno;
            transform.position = pos;
        }
    }
}