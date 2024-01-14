using UnityEngine.Audio;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    //List of every audio queue we have in the game.
    internal enum AudioQueue { 
        BG_MUSIC, BUTTON_CLICK, 
        GOOP_SHOOT,  NERF_SHOOT,  ICE_SHOOT,  FISH_SHOOT,  PLUNGER_SHOOT,  MEGA_SHOOT,
        GOOP_RELOAD, NERF_RELOAD, ICE_RELOAD, FISH_RELOAD, PLUNGER_RELOAD, MEGA_RELOAD,
        PLAYER_FALL, PLAYER_HURT
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
            go = Instantiate(go, transform);
            go.AddComponent<AudioSource>();
            sources[i] = go.GetComponent<AudioSource>();
        }
    }


    /// <summary>
    /// Attempts to play a sound based on the given Audio queue
    /// </summary>
    internal void PlaySound(AudioQueue queue)
    {
        for (int i=0; i<numberAudioPlayers; i++)
        {
            int usedSource = (i + currentSource) % numberAudioPlayers;
            if (sources[usedSource].isPlaying) continue;

            //PLay sound here
            Sound s = getSound(queue);
            if (s.clip == null) break;

            sources[usedSource].clip = s.clip;
            sources[usedSource].volume = s.volume;
            sources[usedSource].pitch = s.pitch;

            break;
        }
    }



    private Sound getSound(AudioQueue queue)
    {
        return getSound(queue, Random.Range(0, soundDirectory[(int)queue].sounds.Count));
    }

    private Sound getSound(AudioQueue queue, int soundChosen)
    {
        return GameSounds[soundDirectory[(int)queue].sounds[soundChosen]];
    }



}





/// <summary>
/// This class contains a list of integers that refer to where in the GameSounds Array the sound is
/// </summary>
internal class SoundList
{
    internal List<int> sounds;

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
