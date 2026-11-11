using UnityEngine;
using UnityEngine.EventSystems;


public class PlacementManager : MonoBehaviour
{
    [SerializeField] private PropDropdown dropdown;

    [SerializeField] private Camera targetCamera;

    [SerializeField] private LayerMask groundMask;

    private GameObject ghost;  //Null狀態

    private void Awake()
    {
        if (targetCamera == null)
            targetCamera = Camera.main;
    }

    private void OnEnable()
    {
        dropdown.DragEnded += OnDragEnded;
    }
    private void OnDisable()
    {
        dropdown.DragEnded -= OnDragEnded;
    }
    private void OnDragEnded(Proplist data, PointerEventData e)
    {
        if (data == null || data.Prefab == null) return;

        Ray ray = targetCamera.ScreenPointToRay(e.position);

        if (Physics.Raycast(ray, out RaycastHit hit, 1000f, groundMask))
            Instantiate(data.Prefab, hit.point, Quaternion.identity);
    }
}