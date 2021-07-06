using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Conducter : Singleton<Conducter>
{
    public float offset = 0.02f;
    public float beatHangTime = 0.16f;
    public float dspSongTime;
    public float inputOffset;
    public float songPos;
    private bool isPlaying = false;
    public List<AudioSource> audioSources;

    protected override void Awake()
    {
        base.Awake();
        isPlaying = false;
        StartCoroutine(StartMusic());
    }


    protected virtual void Update()
    {
        if (isPlaying)
        {
            songPos = (float)(AudioSettings.dspTime - dspSongTime) - offset;
        }
    }

    private IEnumerator StartMusic()
    {
        yield return new WaitForSeconds(1f);
        dspSongTime = (float)AudioSettings.dspTime;
        foreach(AudioSource audioSource in audioSources)
        {
            audioSource.Play();
        }
        isPlaying = true;
    }

    private void OnDisable()
    {
        foreach (AudioSource audioSource in audioSources)
        {
            audioSource.Pause();
        }
    }

}
