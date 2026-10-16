using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float velocita = 40f;
    public int danno = 10;
    public float durata = 3f;

    void OnEnable()
    {
        CancelInvoke();
        Invoke(nameof(Disattiva), durata);   // dopo X secondi torna nel pool
    }

    void Update()
    {
        transform.Translate(Vector3.forward * velocita * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger) return;
        if (other.GetComponentInParent<TankMover>() != null) return;   // ignora il carro

        Target t = other.GetComponentInParent<Target>();
        if (t != null) t.TakeDamage(danno);

        Disattiva();
    }

    void Disattiva()
    {
        gameObject.SetActive(false);
    }
}