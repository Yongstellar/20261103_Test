using UnityEngine;

public class PlaceableProp : MonoBehaviour
{
    
    public Proplist Data { get; private set; }  //道具資料


    public void Initialize(Proplist data)
    {
        Data = data;
    }

    public void Remove()
    {
        Destroy(gameObject);
    }
}
