using System.Collections.Generic;
using UnityEngine;

public class NPCManager : MonoBehaviour
{
    [Header("Spawning Configuration")]
    [SerializeField] private GameObject npcPrefab; 
    [SerializeField] private Transform npcParentContainer; 
    [SerializeField] private List<NPCData> npcsToSpawn; 

    [Header("Core Services")]
    [SerializeField] private TimeManager timeManager;
    [SerializeField] private LocationRegistry locationRegistry;
    [SerializeField] private Camera uiEventCamera;
    private void Start()
    {
        SpawnAllNPCs();
    }

    private void SpawnAllNPCs()
    {
        for (int i = 0; i < npcsToSpawn.Count; i++)
        {
            NPCData npcData = npcsToSpawn[i];
            GameObject spawnedNPC = Instantiate(npcPrefab, npcParentContainer);
            spawnedNPC.name = $"NPC_{npcData.NpcName}";

            if (spawnedNPC.TryGetComponent<NPCScheduleController>(out var scheduleController))
            {
                scheduleController.Initialize(npcData, locationRegistry, uiEventCamera);
            }
        }
    }
}