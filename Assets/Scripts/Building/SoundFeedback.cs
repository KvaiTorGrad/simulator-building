using System.Collections.Generic;
using UnityEngine;

public class SoundFeedback : MonoBehaviour
{
    [SerializeField] private AudioClip _clickSound, _placeSound, _removeSound, _wrongPlacementSound;
    private AudioSource _audio;
    private void Awake()
    {
        _audio = GetComponent<AudioSource>();
    }
    public void PlaySound(SoundType soundType)
    {
        switch (soundType)
        {
            case SoundType.Click:
                _audio.PlayOneShot(_clickSound);
                break;
            case SoundType.Place:
                _audio.PlayOneShot(_placeSound);
                break;
            case SoundType.Remove:
                _audio.PlayOneShot(_removeSound);
                break;
            case SoundType.WrongPlacement:
                _audio.PlayOneShot(_wrongPlacementSound);
                break;
        }
    }
}
public enum SoundType
{
    Click,
    Place,
    Remove,
    WrongPlacement
}
