using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
public class RotateMoveHandler : MonoBehaviour
{
    public static RotateMoveHandler instance;
    private Vector2 tapPosition;
    private Vector2 swipeDelta;
    private bool isSwipingMove;

    private bool isSwipingRotate;

    GameObject activeBlock;
    public Tetromino activeTetris;

    [SerializeField] private Camera ARCamera;

    private Vector3 Down = new Vector3(0, -0.1f, 0);
    private Vector3 Up = new Vector3(0, 0.1f, 0);

    private Vector3 Right = new Vector3(0.1f, 0, 0);
    private Vector3 Left = new Vector3(-0.1f, 0, 0);
    private Vector3 Forward = new Vector3(0, 0, 0.1f);
    private Vector3 Back = new Vector3(0, 0, -0.1f);
    private int side = -1;
    public float compass;

    private int stateZone = 8;
    private Dictionary<int, Vector3[]> moves = new Dictionary<int, Vector3[]>();
    private void InitializeMove() {
        moves.Add(0, new[] {Right, Left, Forward, Back});
        moves.Add(1, new[] {Forward, Back, Right, Left});
        moves.Add(2, new[] {Left, Right, Back, Forward});
        moves.Add(3, new[] {Back, Forward, Left, Right});
    }

    private float touchDuration = 0;
    private Touch touch;
       
    private void Awake()
    {
        instance = this; 
    }
    void Start()
    {
        InitializeMove();
    }
    void Update()
    {
        if (activeBlock == null || !ARManager.instance.gameStart)
            return;
        
        // if (Input.touchCount > 0 && Input.GetTouch(0).tapCount == 1) {
        //     HandleSide();
        //     activeTetris.setInput(moves[side][2]);
        // }

        // else if (Input.touchCount > 0 && Input.GetTouch(0).tapCount == 2) {
        //     HandleSide();
        //     activeTetris.setInput(moves[side][3]);
        // }
        // if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        //  {
        //      // Debug.Log('tap count' + );
        //      tapCount += 1;
        //      StartCoroutine(Countdown()); 
        //      HandleSide();
        //      activeTetris.setInput(moves[side][2]);   
        //  }
       
        //  if (tapCount == 2)
        //  { 
        //      tapCount = 0;
        //      StopCoroutine(Countdown());
        //      HandleSide();   
        //      activeTetris.setInput(moves[side][3]);
        //  }

        float touchDuration = 0;
        Touch touch;
        
        if(Input.touchCount > 0){ //if there is any touch
            HandleSide();
            touchDuration += Time.deltaTime;
            touch = Input.GetTouch(0);

            if (Input.touchCount == 2) {
                // Debug.Log("2 swipe = " + Input.GetTouch(0).phase);
                // // Debug.Log(Input.GetTouch(0).phase);
                if (Input.GetTouch(0).phase == TouchPhase.Began) {
                    tapPosition = Input.GetTouch(0).position;
                    isSwipingRotate = true;
                }
                else if (Input.GetTouch(0).phase == TouchPhase.Canceled ||
                        Input.GetTouch(0).phase == TouchPhase.Ended) {
                    ResetSwipe();
                }
                CheckSwipeRotate();
            }
            else if (Input.touchCount == 1) {
                // Debug.Log("1 swipe = " + Input.GetTouch(0).phase);
                // // Debug.Log(Input.GetTouch(0).phase);
                if (Input.GetTouch(0).phase == TouchPhase.Began) {
                    tapPosition = Input.GetTouch(0).position;
                    isSwipingMove = true;
                }
                else if (Input.GetTouch(0).phase == TouchPhase.Canceled ||
                        Input.GetTouch(0).phase == TouchPhase.Ended) {
                    ResetSwipe();
                    if(touchDuration < 0.2f) {
                        // Debug.Log("tap = " + Input.GetTouch(0).phase + " time = " +  touchDuration);
                        StartCoroutine("singleOrDouble", touch);
                    }
                }
                CheckSwipeMove();
            }   
        }
        else
            touchDuration = 0.0f;
        
    }
    public void SetActiveBlock(GameObject block, Tetromino tetris)
    {
        activeBlock = block;
        activeTetris = tetris;
    }
    private void CheckSwipeMove() {
        if (isSwipingMove) {
            swipeDelta = Input.GetTouch(0).position - tapPosition;
        }
        // Debug.Log("swipeDelta.magnitude = " + swipeDelta.magnitude);
        if (swipeDelta.magnitude > 5) {
            if (Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y)) {
                activeTetris.setInput((swipeDelta.x > 0 ? moves[side][0] : moves[side][1]));
            }
            else if (swipeDelta.y < 0){
                SetHighSpeed();
            }
            ResetSwipe();
        }
    }

    private void HandleSide() {
        float degree = Input.compass.magneticHeading;
        int oldSide = side;
        if (degree < (compass + 45)%360 && degree > (compass - 45)%360) {
            side = 0;
            // Debug.Log("side " + side + " compass " + compass + " degree " + degree);
        }
        else if (degree > (compass + 45)%360 && degree < (compass + 125)%360) {
            side = 3;
            // Debug.Log("side " + side + " compass " + compass + " degree " + degree);
        }
        else if (degree < (compass - 45)%360 && degree > (compass - 125)%360) {
            side = 1;
            // Debug.Log("side " + side + " compass " + compass + " degree " + degree);
        }
        else {
            side = 2;
            // Debug.Log("side " + side + " compass " + compass + " degree " + degree);
        }
        if (oldSide != side && (compass - degree) % 360 <= 9 && oldSide > 0) {
            side = oldSide;
        }
    }
    private void CheckSwipeRotate() {
        // Debug.Log("rotate");
        if (isSwipingRotate) {
            swipeDelta = Input.GetTouch(0).position - tapPosition;
        }
        if (swipeDelta.magnitude > 3) {
            if (Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y)) {
                activeTetris.setRotationInput((swipeDelta.y > 0 ? new Vector3(0, 90, 0) : new Vector3(0, -90, 0)));
                //ARManager.instance.txt.text = "2delrax" + swipeDelta;
            }
            else {
                activeTetris.setRotationInput((swipeDelta.x > 0 ? new Vector3(0, 0, 90) : new Vector3(0, 0, -90)));
                //ARManager.instance.txt.text = "2delray" + swipeDelta;
            }
            
            ResetSwipe();
        }
    }
    private void ResetSwipe() {
        isSwipingMove = false;
        swipeDelta = Vector2.zero;
        tapPosition = Vector2.zero;
        isSwipingRotate = false;
    }

    IEnumerator singleOrDouble(Touch touch){
        yield return new WaitForSeconds(0.3f);
        if(touch.tapCount == 1) {
            activeTetris.setInput(moves[side][2]);   
            // Debug.Log ("Single");
        }
        else if(touch.tapCount == 2){
            //this coroutine has been called twice. We should stop the next one here otherwise we get two double tap
            StopCoroutine("singleOrDouble");
            // Debug.Log ("Double");
            activeTetris.setInput(moves[side][3]);
        }
    }

    public void SetHighSpeed()
    {
        activeTetris.SetSpeed();
    }
}