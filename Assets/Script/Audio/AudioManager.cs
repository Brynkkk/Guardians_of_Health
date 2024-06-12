using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] private AudioSource[] sfx;
    [SerializeField] private AudioSource[] bgm;
    [SerializeField] private float soundDistance;

    public bool playBgm;
    private int bgmIndex;
    private bool areaBgmActive = false;

    private Coroutine stopBgmCoroutine;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject); // Ensure persistence across scenes
        }
    }

    private void Update()
    {
        if (!playBgm || areaBgmActive)
        {
            StopAllBGM();
        }
        else
        {
            if (!bgm[bgmIndex].isPlaying)
            {
                PlayBGM(bgmIndex);
            }
        }
    }

    public void PlaySFX(int _sfxIndex, Transform _source)
    {
        if (sfx[_sfxIndex].isPlaying)
        {
            return;
        }

        if (_source != null && Vector2.Distance(PlayerManager.instance.player.transform.position, _source.position) > soundDistance)
        {
            return;
        }

        if (_sfxIndex < sfx.Length)
        {
            sfx[_sfxIndex].pitch = Random.Range(.85f, 1.1f);
            sfx[_sfxIndex].Play();
        }
    }

    public void StopSFX(int stopIndex) => sfx[stopIndex].Stop();

    public void StopSFXAfterTime(int _index) => StartCoroutine(DecreaseVolume(sfx[_index]));

    public void StopBGMAfterTime(int _index)
    {
        if (stopBgmCoroutine != null)
        {
            StopCoroutine(stopBgmCoroutine);
        }
        stopBgmCoroutine = StartCoroutine(DecreaseVolume(bgm[_index]));
    }

    private IEnumerator DecreaseVolume(AudioSource _audio)
    {
        float defaultVolume = _audio.volume;

        while (_audio.volume > .1f)
        {
            _audio.volume -= _audio.volume * .2f;
            yield return new WaitForSeconds(.25f);

            if (_audio.volume <= .1f)
            {
                _audio.Stop();
                _audio.volume = defaultVolume;
                break;
            }
        }
    }

    public void PlayBGM(int _bgmIndex)
    {
        if (_bgmIndex < bgm.Length)
        {
            bgmIndex = _bgmIndex;

            StopAllBGM();
            bgm[bgmIndex].Play();
        }
        else
        {
            Debug.LogError("BGM index out of range: " + _bgmIndex);
        }
    }

    public void PlayRandomBGM()
    {
        if (bgm.Length > 0)
        {
            bgmIndex = Random.Range(0, bgm.Length);
            PlayBGM(bgmIndex);
        }
        else
        {
            Debug.LogError("No BGM sources available.");
        }
    }

    public void StopAllBGM()
    {
        foreach (var source in bgm)
        {
            source.Stop();
        }
    }

    public void SetAreaBgmActive(bool isActive)
    {
        areaBgmActive = isActive;
    }
}
