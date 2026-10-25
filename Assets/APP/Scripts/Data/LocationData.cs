using UnityEngine;

[CreateAssetMenu(fileName = "NewLocation", menuName = "NPC System/Location")]
public class LocationData : ScriptableObject
{
    [SerializeField] private string locationName;
    [TextArea] [SerializeField] private string description;

    public string LocationName => locationName;
}