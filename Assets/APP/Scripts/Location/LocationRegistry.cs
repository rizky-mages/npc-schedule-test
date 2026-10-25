using System.Collections.Generic;
using UnityEngine;

public class LocationRegistry : MonoBehaviour
{
    [System.Serializable]
    public struct LocationBinding
    {
        public LocationData locationAsset;
        public Transform sceneWaypoint;
    }

    [SerializeField] private List<LocationBinding> locationBindings;
    [SerializeField] private LocationBinding defaultLocationBinding; // Fallback location if a binding is missing
    
    private Dictionary<LocationData, Transform> waypointLookup = new Dictionary<LocationData, Transform>();

    private void Awake()
    {
        for (int i = 0; i < locationBindings.Count; i++)
        {
            LocationBinding binding = locationBindings[i];
            if (binding.locationAsset != null && binding.sceneWaypoint != null)
                waypointLookup[binding.locationAsset] = binding.sceneWaypoint;
        }
    }

    public Transform GetWaypoint(LocationData location)
    {
        return waypointLookup.TryGetValue(location, out Transform waypoint) ? waypoint : defaultLocationBinding.sceneWaypoint;
    }
}