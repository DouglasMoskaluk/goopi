using UnityEngine.Audio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    //List of every audio queue we have in the game.
    internal enum AudioQueue { 
        BG_MUSIC, BUTTON_CLICK, 
        GOOP_SHOOT,  NERF_SHOOT,  ICE_SHOOT,  FISH_SHOOT,  PLUNGER_SHOOT,  MEGA_SHOOT,
        GOOP_RELOAD, NERF_RELOAD, ICE_RELOAD, FISH_RELOAD, PLUNGER_RELOAD, MEGA_RELOAD,
        PLAYER_FALL, PLAYER_HURT,
        LENGTH
    }

    internal static AudioManager instance;

    [SerializeField]
    private int numberAudioPlayers = 5;

    [SerializeField]
    private Sound[] GameSounds;

    private SoundList[] soundDirectory;

    private int currentSource = 0;
    private AudioSource[] sources;




    private void Awake()
    {
        if (instance = null) instance = this;
        sources = new AudioSource[numberAudioPlayers];
        for (int i=0; i<numberAudioPlayers; i++)
        {
            GameObject go = new GameObject();
            //go = Instantiate(go, transform);
            go.transform.parent = transform;
            go.AddComponent<AudioSource>();
            sources[i] = go.GetComponent<AudioSource>();
        }

        //Create directory
        soundDirectory = new SoundList[(int)AudioQueue.LENGTH];
        for (int i=0; i<soundDirectory.Length; i++)
        {
            soundDirectory[i] = new SoundList();
        }
        for (int i=0; i<GameSounds.Length; i++)
        {
            soundDirectory[(int)GameSounds[i].queue].AddSound(i);
        }
    }


    /// <summary>
    /// Attempts to play a sound based on the given Audio queue
    /// </summary>
    internal void PlaySound(AudioQueue queue)
    {
        bool soundPlayed = false;
        for (int i=0; i<numberAudioPlayers; i++)
        {
            int usedSource = (i + currentSource) % numberAudioPlayers;
            if (sources[usedSource].isPlaying)
            {
                continue;
            }
            currentSource = usedSource;
            soundPlayed = true;
            //PLay sound here
            Sound s = getSound(queue);
            if (s.clip == null)
            {
                Debug.LogWarning("Attempting to play sound " + queue + " but there are no sounds for it.");
                break;
            }

            sources[usedSource].clip = s.clip;
            sources[usedSource].volume = s.volume;
            sources[usedSource].pitch = s.pitch;

            sources[usedSource].Play();
            break;
        }
        if (!soundPlayed)
        {
            Debug.LogWarning("Attemped to play a sound, but all AudioSources were in use.");
        }
    }


    private void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.Z))
        {
            PlaySound(AudioQueue.BUTTON_CLICK);
        }*/
    }


    private Sound getSound(AudioQueue queue)
    {
        return getSound(queue, Random.Range(0, soundDirectory[(int)queue].sounds.Count));
    }

    private Sound getSound(AudioQueue queue, int soundChosen)
    {
        if (soundDirectory[(int)queue].sounds.Count == 0) return new Sound();
        return GameSounds[soundDirectory[(int)queue].sounds[soundChosen]];
    }



}





/// <summary>
/// This class contains a list of integers that refer to where in the GameSounds Array the sound is
/// </summary>
internal class SoundList
{
    internal List<int> sounds;

    internal SoundList()
    {
        sounds = new List<int>();
    }

    internal void AddSound(int s)
    {
        sounds.Add(s);
    }
}



/// <summary>
/// This contains a single sound, audioQueue and all.
/// </summary>
[System.Serializable]
internal struct Sound
{
    [SerializeField]
    internal AudioManager.AudioQueue queue;
    [SerializeField]
    internal AudioClip clip;
    [SerializeField]
    [Range(0, 1)]
    internal float volume;
    [SerializeField]
    [Range(0.1f, 3)]
    internal float pitch;
}
