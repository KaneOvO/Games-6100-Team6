using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [System.Serializable]
    public class Sound
    {
        public string name;
        public AudioClip clip;
        public bool loop;
        public float volume = 1f;
        [HideInInspector]
        public AudioSource source;
    }

    public Sound[] sounds;

    public string backgroundMusicName = "BackgroundMusic";  // Name of the background music clip

    private void Awake()
    {
        // Implementing Singleton Pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Initialize each sound with an AudioSource
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.loop = s.loop;
            s.source.volume = s.volume;
        }
    }

    private void Start()
    {
        // Start playing the background music with a fade-in effect
        StartCoroutine(PlayBackgroundMusicWithFadeIn(backgroundMusicName, 2f));  // 2 seconds fade-in duration
    }

    public void Play(string name)
    {
        Sound s = System.Array.Find(sounds, sound => sound.name == name);
        if (s != null && s.source != null)
        {
            s.source.Play();
        }
        else
        {
            Debug.LogWarning("Sound: " + name + " not found!");
        }
    }

    public void Stop(string name)
    {
        Sound s = System.Array.Find(sounds, sound => sound.name == name);
        if (s != null)
        {
            s.source.Stop();
        }
        else
        {
            Debug.LogWarning("Sound: " + name + " not found!");
        }
    }

    public void Pause(string name)
    {
        Sound s = System.Array.Find(sounds, sound => sound.name == name);
        if (s != null)
        {
            s.source.Pause();
        }
        else
        {
            Debug.LogWarning("Sound: " + name + " not found!");
        }
    }

    public void Resume(string name)
    {
        Sound s = System.Array.Find(sounds, sound => sound.name == name);
        if (s != null)
        {
            s.source.UnPause();
        }
        else
        {
            Debug.LogWarning("Sound: " + name + " not found!");
        }
    }

    private IEnumerator PlayBackgroundMusicWithFadeIn(string name, float duration)
    {
        Sound s = System.Array.Find(sounds, sound => sound.name == name);
        if (s != null)
        {
            s.source.volume = 0f;  // Start with volume at 0
            s.source.Play();  // Start playing the music
            float startVolume = s.volume;

            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                s.source.volume = Mathf.Lerp(0f, startVolume, elapsedTime / duration);
                yield return null;
            }

            s.source.volume = startVolume;  // Ensure the final volume is set correctly
        }
        else
        {
            Debug.LogWarning("Background Music: " + name + " not found!");
        }
    }
}


/*
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Assuming the jump sound is named "Jump" in the AudioManager
            AudioManager.Instance.Play("Jump");

        }
    }
}

*/