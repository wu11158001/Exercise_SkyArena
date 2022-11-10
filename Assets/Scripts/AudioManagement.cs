using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManagement : MonoBehaviour
{
    static AudioManagement audioManagement;
    public static AudioManagement Instance => audioManagement;

    [Tooltip("ThisAudioSource")] public AudioSource audioSource;

    private void Awake()
    {
        if(audioManagement != null)
        {
            Destroy(this);
            return;
        }
        audioManagement = this;
        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        audioSource.volume = 0.25f;
        audioSource.playOnAwake = true;
        audioSource.loop = true;
        audioSource.clip = AssetManagement.Instance.OnSearchAssets<AudioClip>("BackgroundMusic", AssetManagement.Instance.backgroundMusic);
        audioSource.Play();
    }
}
