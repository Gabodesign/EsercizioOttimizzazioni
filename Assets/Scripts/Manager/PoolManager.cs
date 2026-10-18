using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{

    public GameObject prefab;
    public int dimensioneIniziale = 20;
    List<GameObject> pool = new List<GameObject>();

    void Awake()
    {
        for (int i = 0; i < dimensioneIniziale; i++)
            Crea();
    }

    GameObject Crea()
    {
        GameObject obj = Instantiate(prefab, transform);
        obj.SetActive(false);
        pool.Add(obj);
        return obj;
    }

    // restituisce un oggetto spento; se sono tutti attivi il pool si espande
    public GameObject Prendi()
    {
        foreach (GameObject obj in pool)
        {
            if (!obj.activeInHierarchy) return obj;
        }

        Debug.Log("Pool espanso: " + prefab.name + " (ora " + (pool.Count + 1) + ")");
        return Crea();
    }
}