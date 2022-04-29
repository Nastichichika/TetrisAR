using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARManager : MonoBehaviour
{
    // [Header("Plane marker")]
    // [SerializeField]private GameObject PlaneMarkerPrefab;
    
    [Header("Tetris playfield")]
    [SerializeField]public GameObject ObjectToSpawn;

    // [Header("ARCamera")]
    // [SerializeField]private Camera ARCamera;

    // private Vector2 TouchPosition;
    private ARRaycastManager ARRaycastManagerScript;
    List<ARRaycastHit> hits = new List<ARRaycastHit>();

    private GameObject SelectedObject;

    public bool gameStart = false;
    void Start()
    {
        ARRaycastManagerScript = FindObjectOfType<ARRaycastManager>();
        ObjectToSpawn.SetActive(false);
        //PlaneMarkerPrefab.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(!gameStart) {
            ShowMarkerAndSetObject();
        }
    }
    void ShowMarkerAndSetObject() {
        ARRaycastManagerScript.Raycast(new Vector2(Screen.width /2, Screen.height/2), hits, TrackableType.Planes);

        // //showMarker
        // if(hits.Count > 0) {
        //      PlaneMarkerPrefab.transform.position = hits[0].pose.position;
        //      PlaneMarkerPrefab.SetActive(true);
        // }
        //setObject
         if(Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began) {
            Instantiate(ObjectToSpawn, hits[0].pose.position, ObjectToSpawn.transform.rotation);
            gameStart = true;
        }
    }
}
