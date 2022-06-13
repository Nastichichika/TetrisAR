using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    GameObject parent;
    Tetromino parentTetromino;


    private Vector3 Down = new Vector3(0, -0.1f, 0);
    private Vector3 Up = new Vector3(0, 0.1f, 0);

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(RepositionBlock());
    }

    public void setParent(GameObject _parent)
    {
        parent = _parent;
        parentTetromino = parent.GetComponent<Tetromino>();
    }

    void PositionGhost()
    {
        transform.position = parentTetromino.transform.position;
        transform.rotation = parentTetromino.transform.rotation;
    }

    IEnumerator RepositionBlock()
    {
        while (parentTetromino.enabled) {
            PositionGhost();
            MoveDown();
            yield return new WaitForSeconds(0.1f);
        }
        Debug.Log("Destroy");
        Destroy(gameObject);
        yield return null;
    }

    void MoveDown()
    {
        while (CheckValidMove())
        {
            transform.position += Down;
        }
        if (!CheckValidMove())
        {
            transform.position += Up;
        }
    }

    bool CheckValidMove()
    {
        foreach (Transform child in transform)
        {
            if (!Playfield.instance.CheckInsideContainer(child.position)) {
                return false;
            }
        }

        foreach (Transform child in transform) {
            Transform t = Playfield.instance.GetTransformOnContainerPos(child.position);
    
            if (t != null && t.parent == parent.transform) {
                return true;
            }

            if (t != null && t.parent != transform) {
                return false;
            }
        }
        return true;
    }
}
