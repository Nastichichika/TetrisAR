using UnityEngine;
public class Tetromino : MonoBehaviour
{

    float prevTime;
    float fallTime = 1.4f;

    private Vector3 Down = new Vector3(0, -0.1f, 0);
    private Vector3 Up = new Vector3(0, 0.1f, 0);
    
    void Start()
    {
        RotateMoveHandler.instance.SetActiveBlock(gameObject, this);
        fallTime = GameManager.instance.ReadFallSpeed();
        if (!CheckValidMove())
        {
        //    ARManager.instance.txt.text = "game over";
            GameManager.instance.SetGameIsOver();
        }
    }
    void Update()
    {
        if(Time.time - prevTime > fallTime)
        {
            transform.position += Down;

            
            if (!CheckValidMove())
            {
                transform.position += Up;
                //DELETE LAYER IF POSSIBLE
                Playfield.instance.DeleteLayer();
                enabled = false;
                //CREATE A NEW TETRIS BLOCK

                if (!GameManager.instance.ReadGameIsOver()) {
                    Playfield.instance.SpawnNewBlock();

                }
            }
            else
            {
                //UPDATE THE GRID
                Playfield.instance.UpdateGrid(this);
            }
            prevTime = Time.time;
        }
    }

    public void setInput(Vector3 direction)
    {
        transform.position += direction;
        if (!CheckValidMove()) {
            transform.position -= direction;
        }
        else {
            Playfield.instance.UpdateGrid(this);
        }
    }

    public void setRotationInput(Vector3 rotation)
    {
        transform.Rotate(rotation, Space.World);
        if (!CheckValidMove())
        {
            transform.Rotate(-rotation, Space.World);
        }
        else
        {
            Playfield.instance.UpdateGrid(this);
        }
    }

    bool CheckValidMove()
    {
        foreach(Transform child in transform)
        {
            //ARManager.instance.txt.text += "transform" + transform;
            Vector3 pos = child.position;
            if(!Playfield.instance.CheckInsideGrid(pos)) {
                Debug.Log("outside" + child.position.ToString());
                ARManager.instance.txt.text += "outside" + pos;
                return false;
            }
        }

        foreach(Transform child in transform)
        {
            Vector3 pos = child.position;
            Transform t = Playfield.instance.GetTransformOnGridPos(pos);
            if (t != null && t.parent != transform)
            {
                ARManager.instance.txt.text = "falde" + " ";
                return false;
            }
        }
        return true;
    }

    public void SetSpeed()
    {
        fallTime = 0.1f;
    }
}
