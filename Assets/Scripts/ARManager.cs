using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARManager : MonoBehaviour
{
    [Header("Plane marker")]
    [SerializeField]private GameObject PlaneMarkerPrefab;

    public static ARManager instance;
    
    [Header("Tetris playfield")]
    [SerializeField]public GameObject ObjectToSpawn;

    private ARRaycastManager ARRaycastManagerScript;

    public bool gameStart = false;

    public Text txt;

    void Awake()
    {
        instance = this;
        Input.compass.enabled = true;
        Input.location.Start();
    }

    void Start()
    {
        ARRaycastManagerScript = FindObjectOfType<ARRaycastManager>();
        PlaneMarkerPrefab.SetActive(false);
    }

    void Update()
    {
        if (!gameStart) {
            ShowMarkerAndSetObject();
        }
    }
    void ShowMarkerAndSetObject() {
        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        ARRaycastManagerScript.Raycast(new Vector2(Screen.width /2, Screen.height/2), hits, TrackableType.Planes);

        if (hits.Count > 0) {
            // txt.text = hits.Count + "qwer";
            PlaneMarkerPrefab.transform.position = hits[0].pose.position;
            PlaneMarkerPrefab.SetActive(true);
        }
        if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began) {
            // txt.text += " magneticHeading   " + Input.compass.magneticHeading;
            
            var instance = Instantiate(ObjectToSpawn, hits[0].pose.position, Quaternion.identity);
            // Debug.Log("Input.compass.trueHeading" + " " + Input.compass.magneticHeading);
            // Debug.Log(Input.compass.trueHeading);
            RotateMoveHandler.instance.compass = Input.compass.trueHeading;
            // Add an ARAnchor component if it doesn't have one already.
            if (instance.GetComponent<ARAnchor>() == null)
            {
                instance.AddComponent<ARAnchor>();
            }

            // Debug.Log(Input.touches[0].position.ToString());
            gameStart = true;
            // txt.text = Input.touches[0].position + " ";
        }
    }
}
