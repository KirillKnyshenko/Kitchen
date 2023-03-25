using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounter : BaseCounter, IHasProgress
{
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;
    public class OnStateChangedEventArgs : EventArgs {
        public State state;
    }

    public enum State {
        Idle,
        Frying,
        Fried,
        Burned,
    }

    private State state;
    [SerializeField] private FryingRecipeSO[] fryingRecipeSOArray;
    [SerializeField] private BurningRecipeSO[] burningRecipeSOArray;
    private float fryingTimer;

    private FryingRecipeSO fryingRecipeSO;

    private float burningTimer;

    private BurningRecipeSO burningRecipeSO;

    private void Start() {
        state = State.Idle;
    }

    private void Update() {
        if (HasKitchenObject())
        {
            switch (state)
            {
                case State.Idle:

                break;
                case State.Frying:
                    fryingTimer += Time.deltaTime;

                    if (fryingTimer >= fryingRecipeSO.timeFrying)
                    {
                        // Fried

                        GetKitchenObject().DestroySelf();

                        KitchenObject.SpawnKitchenObject(fryingRecipeSO.output, this);

                        state = State.Fried;

                        burningRecipeSO = GetBurningRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());
                        burningTimer = 0;

                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs{
                            state = state
                        });
                    }

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                        progressNormalized = (float)fryingTimer / fryingRecipeSO.timeFrying
                    });
                break;
                case State.Fried:
                    burningTimer += Time.deltaTime;

                    if (burningTimer >= burningRecipeSO.timeBurning)
                    {
                        // Burned

                        GetKitchenObject().DestroySelf();

                        KitchenObject.SpawnKitchenObject(burningRecipeSO.output, this);

                        state = State.Burned;
                        
                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs{
                            state = state
                        });
                    }

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                        progressNormalized = (float)burningTimer / burningRecipeSO.timeBurning
                    });
                break;
                case State.Burned:

                break;
            }

        }
    }

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            // There is no KitchenObject here
            if (player.HasKitchenObject())
            {
                // Player is carrying something
                if (HasOutputForInput(player.GetKitchenObject().GetKitchenObjectSO()))
                {
                    // Player is carrying something that can be Fry
                    player.GetKitchenObject().SetKitchenObjectHolder(this);

                    fryingRecipeSO = GetFryingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());

                    fryingTimer = 0;
                    state = State.Frying;

                    OnStateChanged?.Invoke(this, new OnStateChangedEventArgs{
                        state = state
                    });
                }
            } 
            else
            {
                // Player is not carrying anything
            }
        } 
        else
        {
            // There is a KitchenObject here
            if (player.HasKitchenObject())
            {
                // Player is carrying something

                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    // Player is holding a plate

                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        GetKitchenObject().DestroySelf();

                        state = State.Idle;

                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs{
                            state = state
                        });

                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                            progressNormalized = 0
                        });
                    }
                }
            } 
            else
            {
                // Player is not carrying anything
                GetKitchenObject().SetKitchenObjectHolder(player);

                state = State.Idle;

                OnStateChanged?.Invoke(this, new OnStateChangedEventArgs{
                    state = state
                });

                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                    progressNormalized = 0
                });
            }
        }
    }

    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO) {
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOWithInput(inputKitchenObjectSO);

        if (fryingRecipeSO.input != null)
        {
            return fryingRecipeSO.output;
        }

        Debug.LogError("KitchenObject that can't be fryed");
        return null;
    }

    private bool HasOutputForInput(KitchenObjectSO inputKitchenObjectSO) {
        if (GetFryingRecipeSOWithInput(inputKitchenObjectSO))
        {
            return true;
        }
        return false;
    }

    private FryingRecipeSO GetFryingRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO) {
        foreach (FryingRecipeSO fryingRecipeSO in fryingRecipeSOArray)
        {
            if (fryingRecipeSO.input == inputKitchenObjectSO)
            {
                return fryingRecipeSO;
            }
        }
        return null;
    }

    private BurningRecipeSO GetBurningRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO) {
        foreach (BurningRecipeSO burningRecipeSO in burningRecipeSOArray)
        {
            if (burningRecipeSO.input == inputKitchenObjectSO)
            {
                return burningRecipeSO;
            }
        }
        return null;
    }
}
