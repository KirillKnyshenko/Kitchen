using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    private Player player;
    private float foorstepTimer;
    private float foorstepTimerMax = 0.1f;

    private void Awake() {
        player = GetComponent<Player>();
    }

    private void Update() {
        foorstepTimer -= Time.deltaTime;

        if (foorstepTimer < 0f)
        {
            foorstepTimer = foorstepTimerMax;

            if (player.IsWalking())
            {
                float volume = 1f;
                SoundManager.Instance.PlayFootstep(player.transform.position, volume);
            }
        }
    }
}
