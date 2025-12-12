using UnityEngine;
using Vuforia;
using System.Collections;
using System.Collections.Generic;

public class ModelTargetAudio : MonoBehaviour
{
    [Header("Audio Settings")]
    public List<AudioSource> audioSources; // Assign beberapa audio di inspector
    public float delayBetween = 2f;        // Delay antar audio

    private ModelTargetBehaviour modelTarget;
    private Coroutine playCoroutine;
    private int currentIndex = 0;          // Index audio saat ini
    private bool isPaused = false;

    void Start()
    {
        modelTarget = GetComponent<ModelTargetBehaviour>();
        modelTarget.OnTargetStatusChanged += OnStatusChanged;

        // Stop semua audio model target di awal
        foreach (var src in audioSources)
            src.Stop();

        // Subscribe ke mute/unmute
        AudioManager.Instance.OnMuteChanged += HandleMuteChanged;
    }


    private void OnStatusChanged(ObserverBehaviour behaviour, TargetStatus status)
    {
        bool tracked = status.Status == Status.TRACKED ||
                    status.Status == Status.EXTENDED_TRACKED;

        if (tracked && !AudioManager.Instance.IsMuted)
        {
            if (playCoroutine == null && !isPaused)
            {
                playCoroutine = StartCoroutine(PlayAudioSequence());
            }
        }
        else if (!tracked)
        {
            // Object hilang → reset semua audio dan index
            ResetAudioSequence();
        }
    }

    private void ResetAudioSequence()
    {
        isPaused = false;

        if (playCoroutine != null)
        {
            StopCoroutine(playCoroutine);
            playCoroutine = null;
        }

        foreach (var src in audioSources)
            if (src.isPlaying)
                src.Stop();

        currentIndex = 0; // Reset index hanya ketika object hilang
    }
    private IEnumerator PlayAudioSequence()
    {
        while (currentIndex < audioSources.Count)
        {
            // Tunggu sampai tidak mute / tidak pause
            yield return new WaitUntil(() => !AudioManager.Instance.IsMuted && !isPaused);

            AudioSource src = audioSources[currentIndex];
            src.Play();

            // Tunggu sampai audio selesai
            yield return new WaitUntil(() => !src.isPlaying);

            // Delay tambahan
            yield return new WaitForSeconds(delayBetween);

            currentIndex++;
        }

        // Sequence selesai → biarkan currentIndex tetap di akhir
        playCoroutine = null;
    }

    private void HandleMuteChanged(bool isMuted)
    {
        if (isMuted)
        {
            PauseAudio(); // Pause tapi jangan reset index
        }
        else
        {
            // Resume sequence jika target masih terdeteksi
            bool tracked = modelTarget.TargetStatus.Status == Status.TRACKED ||
                           modelTarget.TargetStatus.Status == Status.EXTENDED_TRACKED;

            if (tracked && playCoroutine == null)
            {
                isPaused = false;
                playCoroutine = StartCoroutine(PlayAudioSequence());
            }
        }
    }

    private void PauseAudio()
    {
        isPaused = true;

        if (playCoroutine != null)
        {
            StopCoroutine(playCoroutine);
            playCoroutine = null;
        }

        foreach (var src in audioSources)
            if (src.isPlaying)
                src.Stop();

        // **Jangan reset currentIndex** di sini
    }

    private void StopAllAudio(bool resetIndex = false)
    {
        isPaused = false;

        if (playCoroutine != null)
        {
            StopCoroutine(playCoroutine);
            playCoroutine = null;
        }

        foreach (var src in audioSources)
            if (src.isPlaying)
                src.Stop();

        if (resetIndex)
            currentIndex = 0;
    }

    private void OnDestroy()
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.OnMuteChanged -= HandleMuteChanged;
    }
}
