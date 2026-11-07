using UnityEngine;

[CreateAssetMenu(fileName = "NewProp", menuName = "PropPlacement/Prop list")]

public class Proplist : ScriptableObject
{
    
    [SerializeField] private string displayName = "New Prop";  // 清單名稱
    [SerializeField] private GameObject prefab;  // 道具
    [SerializeField] private Sprite icon;

    public string DisplayName => displayName;
    public GameObject Prefab => prefab;
    public Sprite Icon => icon;
}
