using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using EventHandler = System.EventHandler;
using EventArgs = System.EventArgs;

public class DeliveryManager : MonoBehaviour {

    public event EventHandler OnRecipeSpawned;
    public event EventHandler OnRecipeCompleted;
    public event EventHandler OnRecipeSuccess;
    public event EventHandler OnRecipeFailed;

    public static DeliveryManager Instance {
        get; 
        private set;
    }

    [SerializeField] private RecipeListSO recipeListSO;
    private List<RecipeSO> waitingRecipeSOList;
    private float spawnRecipeTimer;
    private float spawnRecipeTimerMax = 4f;
    private int maxRecipeListCount = 4;
    private int successfulRecipesAmount;

    private void Awake() {

        Instance = this;

        waitingRecipeSOList = new List<RecipeSO>();

        spawnRecipeTimer = spawnRecipeTimerMax;
    }

    private void Update() {
        spawnRecipeTimer -= Time.deltaTime;
        if (spawnRecipeTimer <= 0 ) {
            spawnRecipeTimer = spawnRecipeTimerMax;
            if (GameManager.Instance.IsGamePlaying() && waitingRecipeSOList.Count < maxRecipeListCount) {
                RecipeSO waitingRecipeSO = recipeListSO.recipeSOList[Random.Range(0, recipeListSO.recipeSOList.Count)];
                waitingRecipeSOList.Add(waitingRecipeSO);

                OnRecipeSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public void DeliverRecipe(PlateKitchenObject plateKitchenObject) {
        for (int i = 0; i < waitingRecipeSOList.Count; i++) {
            //Cycling through Waiting Recipes 
            RecipeSO waitingRecipeSO = waitingRecipeSOList[i];

            //Check if waiting Recipe has same amount of ingredients as Plate
            if (waitingRecipeSO.kitchenObjectSO.Count
                != plateKitchenObject.GetKitchenObjectSOList().Count) {
                //Skip this recipe if amount of ingredients are not the same
                continue;
            }

            //Compare recipe ingredients to current Plate ingredients
            bool plateContentsMatchesRecipe = true;
            foreach(KitchenObjectSO recipeKitchenObjectSO in waitingRecipeSO.kitchenObjectSO) {
                //Cycling through All ingredients in Recipe
                bool ingredientFound = false;
                foreach(KitchenObjectSO plateKitchenObjectSO in plateKitchenObject.GetKitchenObjectSOList()) {
                    //Cylcing through All ingredients on Plate
                    if(recipeKitchenObjectSO == plateKitchenObjectSO) {
                        //Ingredient matches
                        ingredientFound = true;
                        break;
                    }
                }
                if (!ingredientFound) {
                    //This Recipe ingredient was not found on plate
                    plateContentsMatchesRecipe = false;
                }
            }

            if (plateContentsMatchesRecipe) {
                //Player delivered correct recipe!
                successfulRecipesAmount++;

                waitingRecipeSOList.RemoveAt(i);

                OnRecipeCompleted?.Invoke(this, EventArgs.Empty);
                OnRecipeSuccess?.Invoke(this, EventArgs.Empty);

                return;
            }

        }

        //No matches found!
        /*Debug.Log("Player did no deliver correct recipe!");*/
        OnRecipeFailed?.Invoke(this, EventArgs.Empty);
    }

    public List<RecipeSO> GetWaitingSOList() {
        return waitingRecipeSOList;
    }

    public int GetSuccessfulRecipesAmount() {
        return successfulRecipesAmount;
    }
}
