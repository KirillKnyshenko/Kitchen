using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DeliveryManagerSingleUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI recipeNameText;
    [SerializeField] private Transform iconContainer;
    [SerializeField] private Transform iconTemplate;

    private void Awake() {
        iconTemplate.gameObject.SetActive(false);
    }

    public void SetRecipe(RecipeSO recipeSO) {
        foreach (Transform child in iconContainer)
        {
            if (child == iconTemplate)
            {
                continue;
            }

            Destroy(child.gameObject);
        }

        recipeNameText.text = recipeSO.recipeName;

        foreach (KitchenObjectSO kitchenObjectSO in recipeSO.KitchenObjectSOList)
        {
            Transform iconTransform = Instantiate(iconTemplate, iconContainer);
            
            iconTransform.GetComponent<Image>().sprite = kitchenObjectSO.sprite;

            iconTransform.gameObject.SetActive(true);
        }
    }
}
