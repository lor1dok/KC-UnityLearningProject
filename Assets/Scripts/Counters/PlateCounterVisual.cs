using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateCounterVisual : MonoBehaviour
{
    [SerializeField] private PlateCounter plateCounter;
    [SerializeField] private Transform counterTopPoint;
    [SerializeField] private Transform plateVisualPrefab;

    private List<GameObject> plateVisualGameObjectList;

    private void Awake() {
        plateVisualGameObjectList = new List<GameObject>();
    }

    private void Start() {
        plateCounter.OnPlateSpawn += PlateCounter_OnPlateSpawn;
        plateCounter.OnPlateRemove += PlateCounter_OnPlateRemove;

    }

    private void PlateCounter_OnPlateSpawn(object sender, System.EventArgs e) {
        Transform plateVisualTransform = Instantiate(plateVisualPrefab, counterTopPoint);

        float plateOffsetY = .1f;
        plateVisualTransform.localPosition = new Vector3(0, 
            plateOffsetY * plateVisualGameObjectList.Count, 0);

        plateVisualGameObjectList.Add(plateVisualTransform.gameObject);
    }

    private void PlateCounter_OnPlateRemove(object sender, System.EventArgs e) {
        RemoveLastPlate();
    }

    private void RemoveLastPlate() {
        int lastPlateNumber = plateVisualGameObjectList.Count - 1;
        GameObject lastPlateGameObject = plateVisualGameObjectList[lastPlateNumber];
        plateVisualGameObjectList.Remove(lastPlateGameObject);
        Destroy(lastPlateGameObject);
    }
}
