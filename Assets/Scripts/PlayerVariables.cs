using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerVariables : MonoBehaviour
{
    public float soundValue = 0f;
    public float minSoundValue = 0f;
    public float maxSoundValue = 100f;


    public float timeSinceLastSound = 0f;
    public float angerValue = 1f;

    public event Action OnMaxSoundReached;      // Mum wakes
    public event Action<float> OnSoundAfterMax; // Mum walks
    bool maxReached;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceLastSound += (angerValue * Time.deltaTime);
    }

    public void KillPlayer()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void IncreaseSound(float sound)
    {
        if (!maxReached)
        {
            soundValue = Mathf.Min(soundValue + sound, maxSoundValue);
            if (soundValue >= maxSoundValue)
            {
                maxReached = true;
                OnMaxSoundReached?.Invoke();
            }
        }
        else
        {
            OnSoundAfterMax?.Invoke(sound);
        }
    }

    public void DecreaseSound(float sound)
    {

    }
}
