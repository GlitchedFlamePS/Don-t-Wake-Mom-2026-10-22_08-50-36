using System.Collections;
using UnityEngine;

public class MomController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] PlayerVariables player;
    [SerializeField] Animator animator;      // optional

    [Header("Tuning")]
    [SerializeField] float wakeDelay = 2f;
    [SerializeField] float stepsPerSoundUnit = 0.1f;
    [SerializeField] float stepLength = 0.6f;
    [SerializeField] float stepDuration = 0.5f;
    [SerializeField] float catchRadius = 1.2f;

    Transform head;
    float stepRemainder;
    int pendingSteps;
    bool awake, walking, caught;

    void Start()
    {
        if (player == null)
        {
            Debug.LogError("MomController: assign the Player (PlayerVariables) in the Inspector.", this);
            enabled = false;
            return;
        }

        if (Camera.main == null)
        {
            Debug.LogError("MomController: no camera tagged MainCamera.", this);
            enabled = false;
            return;
        }

        head = Camera.main.transform;
        SetAnim("Sleeping", true);

        player.OnMaxSoundReached += WakeUp;
        player.OnSoundAfterMax += HandleNoise;

        Debug.Log("Mom is tracking: " + Camera.main.name, Camera.main);
    }

    void OnDestroy()
    {
        if (player == null) return;
        player.OnMaxSoundReached -= WakeUp;
        player.OnSoundAfterMax -= HandleNoise;
    }

    void Update()
    {
        if (!awake || caught) return;

        Vector3 momPos = transform.position;
        Vector3 playerPos = head.position;
        momPos.y = playerPos.y = 0f;   // compare floor distance only

        if (Vector3.Distance(momPos, playerPos) <= catchRadius)
        {
            caught = true;
            player.KillPlayer();
        }
    }

    // Safe replacement for animator?.SetBool(...)
    void SetAnim(string param, bool value)
    {
        if (animator != null) animator.SetBool(param, value);
    }

    void WakeUp() => StartCoroutine(WakeRoutine());

    IEnumerator WakeRoutine()
    {
        Debug.Log("Mom: threshold reached, waking up...");
        yield return new WaitForSeconds(wakeDelay);
        SetAnim("Sleeping", false);
        awake = true;
        Debug.Log("Mom: awake");
        if (pendingSteps > 0 && !walking) StartCoroutine(WalkRoutine());
    }

    void HandleNoise(float soundAmount)
    {
        if (caught) return;

        stepRemainder += soundAmount * stepsPerSoundUnit;
        int whole = Mathf.FloorToInt(stepRemainder);
        stepRemainder -= whole;
        pendingSteps += whole;
        Debug.Log($"Mom: noise {soundAmount} -> +{whole} steps (pending {pendingSteps})");

        if (awake && !walking && pendingSteps > 0)
            StartCoroutine(WalkRoutine());
    }

    IEnumerator WalkRoutine()
    {
        walking = true;
        SetAnim("Walking", true);

        while (pendingSteps > 0 && !caught)
        {
            pendingSteps--;

            Vector3 start = transform.position;
            Vector3 toPlayer = head.position - start;
            toPlayer.y = 0f;

            Vector3 end = start + toPlayer.normalized * Mathf.Min(stepLength, toPlayer.magnitude);

            if (toPlayer.sqrMagnitude > 0.001f)
                transform.rotation = Quaternion.LookRotation(toPlayer);

            for (float t = 0f; t < stepDuration; t += Time.deltaTime)
            {
                transform.position = Vector3.Lerp(start, end, t / stepDuration);
                yield return null;
            }
            transform.position = end;
        }

        SetAnim("Walking", false);
        walking = false;
    }
}