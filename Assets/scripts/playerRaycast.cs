using UnityEngine;
using UnityEngine.Rendering;

public class playerRaycast : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private float _raycastDistance = 1f;

    [Header("Inventory")]
    [SerializeField] private InventoryManager _inventoryManager;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            RaycastHit hit;
            if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out hit, _raycastDistance, _layerMask))
            {
                if (hit.collider.TryGetComponent(out Item _item))
                {
                    _inventoryManager.AddItem(_item.item, _item.amount);
                    Destroy(_item.gameObject);
                }
            }
        }

    }

    private void OnDrawGizmos()
    {
        Debug.DrawRay(_camera.transform.position, _camera.transform.forward, Color.green, _raycastDistance);
    }


}
