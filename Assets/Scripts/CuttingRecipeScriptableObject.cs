using UnityEngine;


[CreateAssetMenu()]
public class CuttingRecipeScriptableObject : ScriptableObject
{

    public KitchenObjectScriptableObject input;
    public KitchenObjectScriptableObject output;
    public int cuttingProgressMax;

}
