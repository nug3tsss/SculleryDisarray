using UnityEngine;

[CreateAssetMenu()]
public class BurningRecipeScriptableObject : ScriptableObject
{

    public KitchenObjectScriptableObject input;
    public KitchenObjectScriptableObject output;
    public int burningTimerMax;

}
