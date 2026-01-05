using UnityEngine;

public class displayElement : MonoBehaviour
{
    public Elemen element;


    public void InitializeElement(Elemen elementData)
    {
        element = elementData;
        // Apply data ke UI components (image, text, dll)
    }
}
