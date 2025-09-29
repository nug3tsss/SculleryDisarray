using UnityEngine;


[CreateAssetMenu()]
public class FryingRecipeScriptableObject : ScriptableObject
{

    public KitchenObjectScriptableObject input;
    public KitchenObjectScriptableObject output;
    public int fryingTimerMax;

}
