using UnityEngine;
using UnityEngine.UI;
public class RotateMoveHandler : MonoBehaviour
{
    public static RotateMoveHandler instance;
    private Vector2 tapPosition;
    private Vector2 swipeDelta;
    private bool isSwipingMove;

    private bool isSwipingRotate;

    GameObject activeBlock;
    Tetromino activeTetris;

    private Vector3 Down = new Vector3(0, -0.1f, 0);
    private Vector3 Up = new Vector3(0, 0.1f, 0);

    private Vector3 Right = new Vector3(0.1f, 0, 0);
    private Vector3 Left = new Vector3(-0.1f, 0, 0);
    private Vector3 Forward = new Vector3(0, 0, 0.1f);
    private Vector3 Back = new Vector3(0, 0, -0.1f);

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {

    }
    void Update()
    {
        if (activeBlock == null)
            return;
        if (Input.touchCount > 0 && Input.GetTouch(0).tapCount == 2) {
            SetHighSpeed();
        }
        // if (Input.touchCount == 2) {
        //     if (Input.GetTouch(0).phase == TouchPhase.Began) {
        //         tapPosition = Input.GetTouch(0).position;
        //         isSwipingRotate = true;
        //     }
        //     else if (Input.GetTouch(0).phase == TouchPhase.Canceled ||
        //             Input.GetTouch(0).phase == TouchPhase.Ended) {
        //         ResetSwipe();
        //     }
        //     CheckSwipeRotate();
        // }
        else if (Input.touchCount == 1) {
            if (Input.GetTouch(0).phase == TouchPhase.Began) {
                tapPosition = Input.GetTouch(0).position;
                isSwipingMove = true;
            }
            else if (Input.GetTouch(0).phase == TouchPhase.Canceled ||
                    Input.GetTouch(0).phase == TouchPhase.Ended) {
                ResetSwipe();
            }
            CheckSwipeMove();
        }   
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
        if (swipeDelta.magnitude > 10) {
            if (Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y)) {
                activeTetris.setInput((swipeDelta.x > 0 ? Right : Left));
               // = "delrax" + tapPosition + " " + Input.GetTouch(0).position;
            }
            else {
                activeTetris.setInput((swipeDelta.y > 0 ? Forward : Back));
               // ARManager.instance.txt.text = "delray" + tapPosition + " " + Input.GetTouch(0).position;
            }
            
            ResetSwipe();
        }   
    }
    private void CheckSwipeRotate() {
        if (isSwipingMove) {
            swipeDelta = Input.GetTouch(0).position - tapPosition;
        }
        if (swipeDelta.magnitude > 10) {
            if (Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y)) {
                activeTetris.setInput((swipeDelta.x > 0 ? Vector3.right : Vector3.left));
                //ARManager.instance.txt.text = "2delrax" + swipeDelta;
            }
            else {
                activeTetris.setInput((swipeDelta.y > 0 ? Vector3.up : Vector3.down));
                //ARManager.instance.txt.text = "2delray" + swipeDelta;
            }
            
            ResetSwipe();
        }
    }
    private void ResetSwipe() {
        isSwipingMove = false;
        swipeDelta = Vector2.zero;
        tapPosition = Vector2.zero;
    }

    public void RotateBlock(string rotation)
    {
        if (activeBlock != null)
        {
            //X Rotation
            if(rotation == "posX")
            {
                activeTetris.setRotationInput(new Vector3(90, 0, 0));
            }
            if (rotation == "negX")
            {
                activeTetris.setRotationInput(new Vector3(-90, 0, 0));
            }

            //Y Rotation
            if (rotation == "posY")
            {
                activeTetris.setRotationInput(new Vector3(0, 90, 0));
            }
            if (rotation == "negY")
            {
                activeTetris.setRotationInput(new Vector3(0, -90, 0));
            }

            //Z Rotation
            if (rotation == "posZ")
            {
                activeTetris.setRotationInput(new Vector3(0, 0, 90));
            }
            if (rotation == "negZ")
            {
                activeTetris.setRotationInput(new Vector3(0, 0, -90));
            }
        }
    }

    public void SetHighSpeed()
    {
        activeTetris.SetSpeed();
    }
}