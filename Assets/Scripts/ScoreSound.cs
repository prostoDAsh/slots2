using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreSound : MonoBehaviour
{
    [SerializeField] private AudioSource moneySound;

    public void PlayMoneySound()
    {
        moneySound.Play();
    }
}
