using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class EffectManager : MonoBehaviour
{
    public static EffectManager Instance;

    [Header("Effect Prefabs")]
    public GameObject GateEffectPrefab;
    public GameObject ButtonEffectPrefab;

    [Header("Audio Settings")]
    public AudioSource AudioSource;

    [Header("Effect Containers")]
    public Transform EffectContainer;

    [Header("Pooling Settings")]
    public int GateEffectPoolSize = 10;
    public int ButtonEffectPoolSize = 75;

    private Queue<GameObject> _gateEffectPool = new Queue<GameObject>();
    private Queue<GameObject> _buttonEffectPool = new Queue<GameObject>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            if (EffectContainer == null)
            {
                EffectContainer = new GameObject("EffectContainer").transform;
                DontDestroyOnLoad(EffectContainer.gameObject);
            }
            InitializePools();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializePools()
    {
        if (GateEffectPrefab != null)
        {
            for (int i = 0; i < GateEffectPoolSize; i++)
            {
                GameObject effect = Instantiate(GateEffectPrefab, EffectContainer);
                effect.SetActive(false);
                _gateEffectPool.Enqueue(effect);
            }
        }

        if (ButtonEffectPrefab != null)
        {
            for (int i = 0; i < ButtonEffectPoolSize; i++)
            {
                GameObject effect = Instantiate(ButtonEffectPrefab, EffectContainer);
                effect.SetActive(false);
                _buttonEffectPool.Enqueue(effect);
            }
        }
    }

    public void PlayGateEffect(Vector3 position)
    {
        PlayEffect(position, _gateEffectPool, GateEffectPrefab);
    }

    public void PlayButtonEffect(Vector3 position)
    {
        PlayEffect(position, _buttonEffectPool, ButtonEffectPrefab);
    }

    private void PlayEffect(Vector3 position, Queue<GameObject> pool, GameObject prefab)
    {
        if (prefab == null) return;

        GameObject effect = pool.Count > 0 ? pool.Dequeue() : Instantiate(prefab, EffectContainer);
        effect.transform.position = position;
        effect.SetActive(true);

        StartCoroutine(DeactivateEffect(effect, pool));
    }

    public void PlaySound(AudioClip clip)
    {
        if (AudioSource != null && clip != null)
        {
            AudioSource.PlayOneShot(clip);
        }
    }

    private IEnumerator DeactivateEffect(GameObject effect, Queue<GameObject> pool)
    {
        yield return new WaitForSeconds(1f);
        effect.SetActive(false);
        pool.Enqueue(effect);
    }
}
