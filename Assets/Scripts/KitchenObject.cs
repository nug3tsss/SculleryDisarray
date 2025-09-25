using UnityEngine;

public class KitchenObject : MonoBehaviour
{

    [SerializeField] KitchenObjectScriptableObject kitchenObjectSO;

    public KitchenObjectScriptableObject GetKitchenObjectSO()
    {
        return kitchenObjectSO;
    }

}
