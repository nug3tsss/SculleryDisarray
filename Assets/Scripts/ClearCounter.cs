using UnityEngine;

public class ClearCounter : MonoBehaviour
{

    [SerializeField] private KitchenObjectScriptableObject kitchenObjectSO;
    [SerializeField] private Transform counterTopPoint;

    public void Interact()
    {
        Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.prefab, counterTopPoint);
        kitchenObjectTransform.localPosition = Vector3.zero;

        Debug.Log(kitchenObjectTransform.GetComponent<KitchenObject>().GetKitchenObjectSO().objectName);

        Debug.Log("Interact!");
    }

}
