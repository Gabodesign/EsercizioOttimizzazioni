using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSpawner : MonoBehaviour
{
    public static TargetSpawner Instance;
    public PoolManager poolObiettivi;
    public Transform[] puntiSpawn;
    public int obiettiviContemporanei = 4;
    public float tempoRespawn = 10f;

    private bool[] occupato;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        occupato = new bool[puntiSpawn.Length];
        int numeroObiettivi = Mathf.Min(obiettiviContemporanei, puntiSpawn.Length);
        for (int i = 0; i < numeroObiettivi; i++)
            Spawna(-1);
    }

    void Spawna(int escludi)
    {
        // punti liberi, escluso quello dove l'obiettivo è appena stato distrutto
        List<int> obiettiviLiberi = new List<int>();
        for (int i = 0; i < puntiSpawn.Length; i++)
        {
            if (!occupato[i] && i != escludi) obiettiviLiberi.Add(i);
        }

        // se non ce ne sono altri, riusa quello appena liberato
        if (obiettiviLiberi.Count == 0)
        {
            if (escludi >= 0 && !occupato[escludi]) obiettiviLiberi.Add(escludi);
            else return;
        }

        int scelto = obiettiviLiberi[Random.Range(0, obiettiviLiberi.Count)];
        occupato[scelto] = true;

        GameObject obj = poolObiettivi.Prendi();
        obj.transform.SetPositionAndRotation(puntiSpawn[scelto].position, puntiSpawn[scelto].rotation);
        obj.GetComponent<Target>().indicePunto = scelto;
        obj.SetActive(true);
    }

    public void ObiettivoDistrutto(int indice)
    {
        occupato[indice] = false;
        StartCoroutine(Respawn(indice));
    }

    private IEnumerator Respawn(int indiceVecchio)
    {
        yield return new WaitForSeconds(tempoRespawn);
        Spawna(indiceVecchio);
    }
}