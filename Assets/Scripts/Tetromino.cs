using UnityEngine;
public class Tetromino : MonoBehaviour
{

    float prevTime;
    float fallTime = 1f;

    private Vector3 Down = new Vector3(0, -0.1f, 0);
    private Vector3 Up = new Vector3(0, 0.1f, 0);
    
    void Start()
    {
        RotateMoveHandler.instance.SetActiveBlock(gameObject, this);
        
        fallTime = GameManager.instance.ReadFallSpeed();
        if (!CheckValidMove()) {
            Debug.Log("SetGameIsOver");
            GameManager.instance.SetGameIsOver();
        }
    }
    void Update()
    {
        if(Time.time - prevTime > fallTime) {
            transform.position += Down;

            if (!CheckValidMove()) {
                transform.position += Up;

                Playfield.instance.DeleteLayer();
                enabled = false;

                if (!GameManager.instance.ReadGameIsOver()) {
                    GameManager.instance.setScore(36);
                    Playfield.instance.SpawnNewBlock();
                }
            }
            else {
                Playfield.instance.UpdateContainer(this);
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
            Playfield.instance.UpdateContainer(this);
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
            Playfield.instance.UpdateContainer(this);
        }
    }

    bool CheckValidMove()
    {
        foreach(Transform child in transform){
            //ARManager.instance.txt.text += "transform" + transform;
            Vector3 pos = child.position;

            if(!Playfield.instance.CheckInsideContainer(pos)) {
                // Debug.Log("outside NOT VALID" + child.position.ToString() + transform.name);
                // ARManager.instance.txt.text += "outside" + pos;
                // // Debug.Log(NPOTSupport)
                return false;
            }
        }

        foreach(Transform child in transform) {
            Vector3 pos = child.position;
            Transform t = Playfield.instance.GetTransformOnContainerPos(pos);
            if (t != null && t.parent != transform) {
                // Debug.Log("here someone NOT VALID" + child.position.ToString() + t.name + transform.name);
                // ARManager.instance.txt.text += "falde" + " " + pos;
                return false;
            }
        }
        // Debug.Log("VALID" + transform.position + "   " + transform.name);
        return true;
    }

    public void SetSpeed() 
    {
        fallTime = 0.1f;
    }
}
