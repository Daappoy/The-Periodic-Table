using UnityEngine;

public class ElementPanelHandler : MonoBehaviour
{
    public static ElementPanelHandler ElementPanelInstance;
    public GameObject[] ElementColumns;
    public GameObject ElementPanel;


    void Start()
    {
        ElementPanel.SetActive(false);
        ColumnUpdate();
    }

    public void ColumnUpdate()
    {
        for (int i = 0; i < ElementColumns.Length; i++)
        {
            ElementColumns[i].GetComponent<CanvasGroup>().alpha = (i < GameManager.instance.currentLevel) ? 1 : 0.5f;
        }
    }
    
    public void ToggleElementPanel()
    {
        if (ElementPanel.activeSelf)
        {
            ElementPanel.SetActive(false);
        }
        else
        {
            ElementPanel.SetActive(true);
            ColumnUpdate();
        }
    }
}
