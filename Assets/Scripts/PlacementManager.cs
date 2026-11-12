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
        dropdown.DragBegan += OnDragBegan;
        dropdown.Dragging += OnDragging;
        dropdown.DragEnded += OnDragEnded;
    }
    private void OnDisable()
    {
        dropdown.DragBegan -= OnDragBegan;
        dropdown.Dragging -= OnDragging;
        dropdown.DragEnded -= OnDragEnded;
    }
    private void OnDragBegan(Proplist data, PointerEventData e)
    {
        if (data == null || data.Prefab == null) return;

        ghost = Instantiate(data.Prefab);
        ghost.SetActive(false);
    }
    private void OnDragging(Proplist data, PointerEventData e)
    {
        if (ghost == null) return;

        if (TryGetGroundPoint(e, out Vector3 point))
        {
            ghost.transform.position = point;
            ghost.SetActive(true);
        }
        else
        {
            ghost.SetActive(false);
        }
    }
    private void OnDragEnded(Proplist data, PointerEventData e)
    {
        if (ghost != null)
            Destroy(ghost);

        if (!TryGetGroundPoint(e, out Vector3 point)) return;

        GameObject obj = Instantiate(data.Prefab, point, Quaternion.identity);

        PlaceableProp prop = obj.GetComponent<PlaceableProp>();
        if (prop == null) prop = obj.AddComponent<PlaceableProp>();
        prop.Initialize(data);
    }

    private bool TryGetGroundPoint(PointerEventData e, out Vector3 point)
    {
        point = Vector3.zero;

        if (EventSystem.current.IsPointerOverGameObject())
            return false;

        Ray ray = targetCamera.ScreenPointToRay(e.position);

        if (Physics.Raycast(ray, out RaycastHit hit, 1000f, groundMask))
        {
            point = hit.point;
            return true;
        }
        return false;
    }
}