using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelSounds : MonoBehaviour
{
    [SerializeField] private AudioSource wheelScrollSound;

    [SerializeField] private AudioSource winSound;

    [SerializeField] private AudioSource anticipationSound;
    public void PlayWheelsScrollSound()
    {
        wheelScrollSound.Play();
    }

    public void StopWheelScrollSound()
    {
        wheelScrollSound.Stop();
    }

    public void PlayWinSound()
    {
        winSound.Play();
    }
    
    public void StopWinSound()
    {
        winSound.Stop();
    }

    public void PlayAnticipationSound()
    {
        anticipationSound.Play();
    }
    
    public void StopAnticipationSound()
    {
        anticipationSound.Stop();
    }
}
