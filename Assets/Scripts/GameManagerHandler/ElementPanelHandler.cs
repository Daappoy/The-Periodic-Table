using UnityEngine;

public class ElementPanelHandler : MonoBehaviour
{
    public static ElementPanelHandler ElementPanelInstance;
    public GameObject[] elementColumns;
    public GameObject elementPanel;


    void Start()
    {
        elementPanel.SetActive(false);
        ColumnUpdate();
    }

    private void ColumnUpdate()
    {
        for (int i = 0; i < elementColumns.Length; i++)
        {
            elementColumns[i].GetComponent<CanvasGroup>().alpha = (i < gameManager.Instance.currentLevel) ? 1 : 0.33f;
        }
    }
    
    public void ToggleElementPanel()
    {
        if (elementPanel.activeSelf)
        {
            elementPanel.SetActive(false);
        }
        else
        {
            elementPanel.SetActive(true);
            ColumnUpdate();
        }
    }
}
