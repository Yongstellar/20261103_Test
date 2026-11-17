using UnityEngine;
using UnityEngine.EventSystems;

public class PlacementManager : MonoBehaviour
{
    [SerializeField] private PropDropdown dropdown;

    [SerializeField] private Camera targetCamera;

    [SerializeField] private LayerMask groundMask;

    [SerializeField] private float hoverHeight = 1.5f;  //懸浮高度

    private GameObject ghost;  //Null狀態

    [Header("Guide")]
    
    [SerializeField] private LineRenderer guideLine;


    private void Awake()
    {
        if (targetCamera == null)
            targetCamera = Camera.main;
    }
    private void Start()
    {
        SetGuideVisible(false);
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
            Vector3 hoverPos = point + Vector3.up * hoverHeight;
            ghost.transform.position = hoverPos;
            ghost.SetActive(true);

            guideLine.SetPosition(0, hoverPos);
            guideLine.SetPosition(1, point);

            SetGuideVisible(true);
        }
        else
        {
            ghost.SetActive(false);
            SetGuideVisible(false);
        }
    }
    private void OnDragEnded(Proplist data, PointerEventData e)
    {
        if (ghost != null)
            Destroy(ghost);

        SetGuideVisible(false);

        if (!TryGetGroundPoint(e, out Vector3 point)) return;

        GameObject obj = Instantiate(data.Prefab, point, Quaternion.identity);

        PlaceableProp prop = obj.GetComponent<PlaceableProp>();
        if (prop == null) prop = obj.AddComponent<PlaceableProp>();
        prop.Initialize(data);
    }
    private void SetGuideVisible(bool visible)
    {
        if (guideLine != null) guideLine.gameObject.SetActive(visible);
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