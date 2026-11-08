using JetBrains.Annotations;
using UnityEngine;

public class SoundProducerScript : MonoBehaviour
{
    public float soundValue = 0f;
    public bool hasBeenTriggered = false;
    public float timeSinceTriggered = 0f;
    public float resetTime = 10f;

    public PlayerVariables player;

    private AudioSource audioSource;

    public AudioClip noise;
    public float noiseVolume = 1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
       if(hasBeenTriggered == true)
        {
            if (player != null) {
                timeSinceTriggered += player.angerValue * Time.deltaTime;

            }
            else
            {
                timeSinceTriggered += 1 * Time.deltaTime;
            }

            if (timeSinceTriggered > resetTime)
            {
                hasBeenTriggered = false;
            }
        } 
    }

    public void OnTriggerEnter(Collider other)
    {
        if (hasBeenTriggered == false)
        {
            if (other.gameObject.TryGetComponent<PlayerVariables>(out PlayerVariables playerRef))
            {
                playSound(playerRef);
            }
        }
       
}

    public void playSound(PlayerVariables playerObj)
    {
        player = playerObj;
        audioSource.PlayOneShot(noise, noiseVolume);
        player.IncreaseSound(soundValue);
        hasBeenTriggered = true;
        print("Should play sound!");
        print(player.soundValue);
    }
}
