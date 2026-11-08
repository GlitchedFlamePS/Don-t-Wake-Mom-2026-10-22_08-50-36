using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerVariables : MonoBehaviour
{
    public float soundValue = 0f;
    public float minSoundValue = 0f;
    public float maxSoundValue = 100f;


    public float timeSinceLastSound = 0f;
    public float angerValue = 1f;


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
        soundValue += sound;
        if(soundValue >= maxSoundValue)
        {
           
            Invoke("KillPlayer", 4f);
        }
    }

    public void DecreaseSound(float sound)
    {

    }
}
