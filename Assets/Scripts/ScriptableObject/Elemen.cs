using UnityEngine;

[CreateAssetMenu(menuName = "Create New Element")]
public class Elemen : ScriptableObject
{
    public int ID;
    public int elementLevel;
    public int Golongan;
    public string elementName;
    public bool Metalic;
    public Sprite icon;
}
