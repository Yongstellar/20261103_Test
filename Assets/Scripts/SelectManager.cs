using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;


public class SelectManager : MonoBehaviour
{
    [SerializeField] private Camera targetCamera;

    [SerializeField] private LayerMask propMask;

    [SerializeField] private RectTransform menu;

    [SerializeField] private Vector2 menuOffset = new Vector2(120f, 0f);

    private PlaceableProp selected;

    private void Awake()
    {
        if (targetCamera == null)
            targetCamera = Camera.main;
    }

    private void Start()
    {
        menu.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
            HandleClick();
        if (selected == null && menu.gameObject.activeSelf)
            menu.gameObject.SetActive(false);
    }

    private void LateUpdate()
    {
        if (selected != null)
        {
            Vector3 screenPos = targetCamera.WorldToScreenPoint(selected.transform.position);
            menu.position = screenPos + (Vector3)menuOffset;
        }
    }

    private void HandleClick()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        Vector2 mousePos = Mouse.current.position.ReadValue();
        Ray ray = targetCamera.ScreenPointToRay(mousePos);

        if (Physics.Raycast(ray, out RaycastHit hit, 1000f, propMask))
        {
            PlaceableProp prop = hit.collider.GetComponentInParent<PlaceableProp>();
            if (prop != null)
            {
                Select(prop);
                return;
            }
        }
        Deselect();
    }

    private void Select(PlaceableProp prop)
    {
        selected = prop;
        menu.gameObject.SetActive(true);
    }

    private void Deselect()
    {
        selected = null;
        menu.gameObject.SetActive(false);
    }
    public void DeleteSelected()
    {
        if (selected != null)
            selected.Remove();

        Deselect();
    }
}