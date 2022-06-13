using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
public class RotateMoveHandler : MonoBehaviour
{
    public static RotateMoveHandler instance;
    private Vector2 tapPositionFirst;
    private Vector2 tapPosition;
    private Vector2 swipeDelta;
    private bool isSwipingMove;

    private bool isSwipingRotate;

    GameObject activeBlock;
    public Tetromino activeTetris;

    private Vector3 Down = new Vector3(0, -0.1f, 0);
    private Vector3 Up = new Vector3(0, 0.1f, 0);

    private Vector3 Right = new Vector3(0.1f, 0, 0);
    private Vector3 Left = new Vector3(-0.1f, 0, 0);
    private Vector3 Forward = new Vector3(0, 0, 0.1f);
    private Vector3 Back = new Vector3(0, 0, -0.1f);
    private int side = -1;
    public float compass;

    private int stateZone = 10;
    private Dictionary<int, Vector3[]> moves = new Dictionary<int, Vector3[]>();
    private void InitializeMove() {
        moves.Add(0, new[] {Right, Left, Forward, Back});
        moves.Add(1, new[] {Forward, Back, Right, Left});
        moves.Add(2, new[] {Left, Right, Back, Forward});
        moves.Add(3, new[] {Back, Forward, Left, Right});
    }
       
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

        if (activeBlock == null || !ARManager.instance.gameStart || PauseMenu.isPauseCustom)
            return;
        
        if(Input.touchCount > 0){ //if there is any touch
            HandleSide();
            Touch touch = Input.GetTouch(0);
            Debug.Log("Input.touchCount " + Input.touchCount);
            Debug.Log("touch " + touch.position);
            Debug.Log("touch.phase " + touch.phase);
            if ((Input.touchCount == 2 || isSwipingRotate || (isSwipingMove && Input.touchCount == 2 && (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved)))) {
                switch (touch.phase)
                {   
                    case TouchPhase.Began:
                    // Record initial touch position.
                    tapPositionFirst = touch.position;
                    tapPosition = touch.position;
                    isSwipingRotate = true;
                    break;

                    case TouchPhase.Ended:
                        tapPosition = touch.position;
                        CheckSwipeRotate();
                        Debug.Log("touch Rotete ENded " + touch.position);
                        ResetSwipe();
                        break;  
                }
                
            }
            else if (Input.touchCount == 1 || isSwipingMove) {
                switch (touch.phase)
                {   
                    case TouchPhase.Began:
                    // Record initial touch position.
                        tapPositionFirst = touch.position;
                        tapPosition = touch.position;
                        isSwipingMove = true;
                        break;

                    case TouchPhase.Ended:
                        tapPosition = touch.position;
                        CheckSwipeMove();
                        Debug.Log("touch Move ENded " + touch.position);
                        ResetSwipe();
                        break;  
                }
                
            }   
        }
        
    }
    public void SetActiveBlock(GameObject block, Tetromino tetris)
    {
        activeBlock = block;
        activeTetris = tetris;
    }
    private void CheckSwipeMove() {
        if (isSwipingMove) {
            Debug.Log("tapPositionFirst " + tapPositionFirst + " tapPosition " + tapPosition);
            swipeDelta = tapPosition - tapPositionFirst;
            Debug.Log("swipeDelta " + swipeDelta);
        }
        else return;
        
        Debug.Log("tapPositionFirst " + tapPositionFirst + " tapPosition " + tapPosition);
        Debug.Log("isSwipingMove = " + isSwipingMove);
        Debug.Log("Move swipeDelta.magnitude " + swipeDelta);
        if (Mathf.Abs(swipeDelta.magnitude) > 100) {
            if (Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y)) {
                if( swipeDelta.x > 0) {
                    Debug.Log("Move by X > 0   " + swipeDelta.magnitude);
                }
                else {
                    Debug.Log("Move by X << 0   " + swipeDelta.magnitude);
                }
                Debug.Log("Move by X " + swipeDelta.magnitude);
                activeTetris.setInput((swipeDelta.x > 0 ? moves[side][0] : moves[side][1]));
            }
            else if (swipeDelta.y < 0){
                Debug.Log("Speed up " + swipeDelta.magnitude);
                SetHighSpeed();
            }
            
        } else if(swipeDelta.magnitude == 0) {
             activeTetris.setInput(moves[side][3]); 
        }
        // ResetSwipe();
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
        if (oldSide != side && (compass - degree) % 360 <= stateZone && oldSide > 0) {
            side = oldSide;
        }
    }
    private void CheckSwipeRotate() {
        Debug.Log("rotate");
        if (isSwipingRotate) {
            Debug.Log("tapPositionFirst " + tapPositionFirst + " tapPosition " + tapPosition);
            swipeDelta = tapPosition - tapPositionFirst;
            Debug.Log("swipeDelta " + swipeDelta);
        }
        else return;
        Debug.Log("tapPositionFirst " + tapPositionFirst + " tapPosition " + tapPosition);
        Debug.Log("isSwipingRotate = " + isSwipingRotate);
        Debug.Log("ROTATE swipeDelta.magnitude " + swipeDelta);
        if (Mathf.Abs(swipeDelta.magnitude) > 100) {
            
            if (Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y)) {
                if( swipeDelta.y > 0) {
                    Debug.Log("Rotate by Y > 0   " + swipeDelta.magnitude);
                }
                else {
                    Debug.Log("Rotate by Y << 0   " + swipeDelta.magnitude);
                }
                activeTetris.setRotationInput((swipeDelta.y > 0 ? new Vector3(0, 90, 0) : new Vector3(0, -90, 0)));
                //ARManager.instance.txt.text = "2delrax" + swipeDelta;
            }
            else {
                Debug.Log("ROTATE swipeDelta.x " + swipeDelta.x);
                if (Mathf.Abs(swipeDelta.x) < 100) {
                    activeTetris.setRotationInput((swipeDelta.x > 0 ? new Vector3(90, 0, 0) : new Vector3(-90, 0, 0)));
                    if( swipeDelta.x > 0) {
                        Debug.Log("Rotate by X > 0   " + swipeDelta.magnitude);
                    }
                    else {
                        Debug.Log("Rotate by X << 0   " + swipeDelta.magnitude);
                    }
                }
                else {
                    if( swipeDelta.y > 0) {
                    Debug.Log("Rotate by Z > 0   " + swipeDelta.magnitude);
                    }
                    else {
                        Debug.Log("Rotate by Z << 0   " + swipeDelta.magnitude);
                    }
                    activeTetris.setRotationInput((swipeDelta.x > 0 ? new Vector3(0, 0, 90) : new Vector3(0, 0, -90)));
                } 
            }
        } else if(swipeDelta.magnitude == 0){
             activeTetris.setInput(moves[side][2]);
        }
    }
    private void ResetSwipe() {
        isSwipingMove = false;
        swipeDelta = Vector2.zero;
        tapPosition = Vector2.zero;
        isSwipingRotate = false;
    }

    public void SetHighSpeed()
    {
        activeTetris.SetSpeed();
    }
}