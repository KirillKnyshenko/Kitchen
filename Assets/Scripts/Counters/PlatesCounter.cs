using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounter : BaseCounter
{
    public event EventHandler OnPlateSpawned;
    public event EventHandler OnPlateRemoved;
    private float spawnPlatesTimer;
    private float spawnPlatesTimerMax = 4f;
    private int platesSpawnedAmount;
    private int platesSpawnedAmountMax = 4;

    [SerializeField] private KitchenObjectSO plateKitchenObjectSO;

    private void Update() {
        spawnPlatesTimer += Time.deltaTime;

        if (spawnPlatesTimer >= spawnPlatesTimerMax)
        {
            spawnPlatesTimer = 0f;

            if (GameManager.Instance.IsGamePlaying() && platesSpawnedAmount < platesSpawnedAmountMax)
            {
                platesSpawnedAmount++;
                OnPlateSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public override void Interact(Player player) {
        if (!player.HasKitchenObject())
        {
            // Player is empty handed

            if (platesSpawnedAmount > 0)
            {
                // There's at least one plate here

                platesSpawnedAmount--;

                KitchenObject.SpawnKitchenObject(plateKitchenObjectSO, player);

                OnPlateRemoved?.Invoke(this, EventArgs.Empty);
            }
        }

    }
}