using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float velocita = 40f;
    public int danno = 10;
    public float durata = 3f;

    private void OnEnable()
    {
        CancelInvoke();
        Invoke(nameof(Disattiva), durata);   // dopo X secondi torna nel pool
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * velocita * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger) return;
        if (other.GetComponentInParent<TankMover>() != null) return;   // ignora il carro

        Target target = other.GetComponentInParent<Target>();
        if (target != null) target.TakeDamage(danno);

        Disattiva();
    }

    private void Disattiva()
    {
        gameObject.SetActive(false);
    }
}