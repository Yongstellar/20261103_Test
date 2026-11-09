using UnityEngine;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PropDropdown : MonoBehaviour
{
    [Header("UI")]

    [SerializeField] private Button toggleButton;  //收合按鈕

    [SerializeField] private TMP_Text titleLabel;

    [SerializeField] private RectTransform listPanel;  //清單面板

    [SerializeField] private PropListItem itemPrefab;  //清單項目

    [Header("Data")]

    [SerializeField] private List<Proplist> props = new List<Proplist>();

    [Header("Debug")]

    [SerializeField] private bool logEvents = true;

    //訂閱事件
    public event Action<Proplist, PointerEventData> DragBegan;
    public event Action<Proplist, PointerEventData> Dragging;
    public event Action<Proplist, PointerEventData> DragEnded;

    
    public bool IsExpanded => listPanel != null && listPanel.gameObject.activeSelf;

    private void Awake()
    {
        BuildItems();
        SetExpanded(false); 
        toggleButton.onClick.AddListener(Toggle);
    }
    private void BuildItems()
    {
        foreach (var data in props)
        {
            if (data == null) continue; 

            PropListItem item = Instantiate(itemPrefab, listPanel);
            item.name = "Item_" + data.DisplayName;
            item.Setup(data);

            item.DragBegan += OnItemDragBegan;
            item.Dragging += OnItemDragging;
            item.DragEnded += OnItemDragEnded;
        }
    }

    public void Toggle()
    {
        SetExpanded(!IsExpanded);
    }

    public void Collapse()
    {
        SetExpanded(false);
    }

    private void SetExpanded(bool expanded)
    {
        listPanel.gameObject.SetActive(expanded);

        if (titleLabel != null)
            titleLabel.text = expanded ? "Props [-]" : "Props [+]";
    }

    private void OnItemDragBegan(Proplist data, PointerEventData e)
    {
        if (logEvents) Debug.Log($"[Dropdown] Drag began: {data.DisplayName}");
        DragBegan?.Invoke(data, e);
    }

    private void OnItemDragging(Proplist data, PointerEventData e)
    {
        Dragging?.Invoke(data, e);
    }

    private void OnItemDragEnded(Proplist data, PointerEventData e)
    {
        if (logEvents) 
            Debug.Log($"[Dropdown] Drag ended: {data.DisplayName} at {e.position}");
        DragEnded?.Invoke(data, e);
    }
}
