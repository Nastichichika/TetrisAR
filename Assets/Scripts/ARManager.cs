using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARManager : MonoBehaviour
{
    [Header("Plane marker")]
    [SerializeField]public GameObject PlaneMarkerPrefab;
    private GameObject marker;
    public static ARManager instance;
    
    [Header("Tetris playfield")]
    [SerializeField]public GameObject ObjectToSpawn;

    private ARRaycastManager ARRaycastManagerScript;

    public bool gameStart = false;

    void Awake()
    {
        instance = this;
        Input.compass.enabled = true;
        Input.location.Start();
        PlaneMarkerPrefab.SetActive(false);
    }

    void Start()
    {
        ARRaycastManagerScript = FindObjectOfType<ARRaycastManager>();
    }

    void Update()
    {
        if (!gameStart && !PauseMenu.isPauseCustom) {
            ShowMarkerAndSetObject();
        }
    }
    void ShowMarkerAndSetObject() {
        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        ARRaycastManagerScript.Raycast(new Vector2(Screen.width /2, Screen.height/2), hits, TrackableType.Planes);

        if (hits.Count > 0) {
            Debug.Log("hits" + hits[0].pose.position);
            PlaneMarkerPrefab.transform.position = new Vector3(hits[0].pose.position.x, hits[0].pose.position.y - 0.5f, hits[0].pose.position.z);
            
            Debug.Log(PlaneMarkerPrefab.transform.position);
            PlaneMarkerPrefab.SetActive(true);
        }
        if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began) {
            PlaneMarkerPrefab.SetActive(false);
            var instance = Instantiate(ObjectToSpawn, hits[0].pose.position, ObjectToSpawn.transform.rotation);
            
            RotateMoveHandler.instance.compass = Input.compass.trueHeading;
            if (instance.GetComponent<ARAnchor>() == null)
            {
                instance.AddComponent<ARAnchor>();
            }
            gameStart = true;
        }
    }
}
