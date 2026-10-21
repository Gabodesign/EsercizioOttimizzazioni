using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Sorgente effetti (one-shot)")]
    public AudioSource sorgente;

    [Header("Motore (loop)")]
    public AudioSource sorgenteMotore;   // opzionale: se vuota viene creata in automatico

    [Header("Suoni Effetti")]
    public AudioClip engineTank;
    public AudioClip shootCannon;
    public AudioClip shootMitra;
    public AudioClip explosion;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        if (sorgente == null) sorgente = GetComponent<AudioSource>();
        if (sorgente == null) sorgente = gameObject.AddComponent<AudioSource>();

        if (sorgenteMotore == null && engineTank != null)
        {
            sorgenteMotore = gameObject.AddComponent<AudioSource>();
            sorgenteMotore.playOnAwake = false;
            sorgenteMotore.loop = true;
            sorgenteMotore.spatialBlend = 0f;
            sorgenteMotore.volume = 0.3f;
        }
    }

    // suono 2D (player, sparo, esplosione)
    public void Play(AudioClip clip, float volume = 1f)
    {
        if (clip == null || sorgente == null) return;
        sorgente.PlayOneShot(clip, volume);
    }

    // avvia il loop del motore (chiamato da TankMover.Start)
    public void AvviaMotore()
    {
        if (sorgenteMotore == null || engineTank == null) return;
        if (sorgenteMotore.isPlaying) return;

        sorgenteMotore.clip = engineTank;
        sorgenteMotore.loop = true;
        sorgenteMotore.Play();
    }

    // velocitaNormalizzata: 0 = fermo, 1 = velocità massima
    public void AggiornaMotore(float velocitaNormalizzata)
    {
        if (sorgenteMotore == null || !sorgenteMotore.isPlaying) return;

        float v = Mathf.Clamp01(velocitaNormalizzata);
        sorgenteMotore.volume = Mathf.Lerp(0.15f, 0.45f, v);
        sorgenteMotore.pitch = Mathf.Lerp(0.85f, 1.15f, v);
    }

    public void FermaMotore()
    {
        if (sorgenteMotore != null) sorgenteMotore.Stop();
    }

    // suono 3D in un punto (esplosioni lontane, ecc.)
    public void PlayAt(AudioClip[] clips, Vector3 posizione, float volume = 1f)
    {
        if (clips == null || clips.Length == 0) return;
        AudioClip clip = clips[Random.Range(0, clips.Length)];
        if (clip != null) AudioSource.PlayClipAtPoint(clip, posizione, volume);
    }
}
