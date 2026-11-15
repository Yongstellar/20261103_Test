using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;


public class SelectManager : MonoBehaviour
{
    [SerializeField] private Camera targetCamera;

    [SerializeField] private LayerMask propMask;

    [SerializeField] private RectTransform menu;

    [SerializeField] private Vector2 menuGap = new Vector2(40f, 0f);

    private Canvas canvas;

    private PlaceableProp selected;

    private void Awake()
    {
        if (targetCamera == null)
            targetCamera = Camera.main;
        //尋找上層的Canvas
        canvas = menu.GetComponentInParent<Canvas>().rootCanvas;
    }

    private void Start()
    {
        menu.pivot = new Vector2(0f, 0.5f);
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
            //跟隨Canvas間距變化
            Vector3 gap = (Vector3)menuGap * canvas.scaleFactor;
            menu.position = screenPos + gap;
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