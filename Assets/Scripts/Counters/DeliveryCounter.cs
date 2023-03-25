using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryCounter : BaseCounter
{
    public static DeliveryCounter Instance;

    private void Awake() {
        Instance = this;
    }
    
    public override void Interact(Player player) {
        if (player.HasKitchenObject())
        {
            if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
            {
                // Only accept plates
                player.GetKitchenObject().DestroySelf();

                DeliveryManager.Instance.DeliveryRecipe(plateKitchenObject);
            }
        }
    }
}
