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
        // ARManager.instance.txt.text += "parent   " + " " + _parent.name;
    }

    void PositionGhost()
    {
        // Debug.Log("ghost" + transform.position);
        // ARManager.instance.txt.text += "  ghost" + " " + transform.position;
        transform.position = parentTetromino.transform.position;
        // ARManager.instance.txt.text += "  parent" + " " + transform.position;
        // Debug.Log("ghost" + transform.position);
        transform.rotation = parentTetromino.transform.rotation;
    }

    IEnumerator RepositionBlock()
    {
        while (parentTetromino.enabled) {
            // ARManager.instance.txt.text += "isActiveAndEnabled" + " " + transform.position;
            PositionGhost();
            MoveDown();
            yield return new WaitForSeconds(0.1f);
        }
        // ARManager.instance.txt.text += "Destroy" + " " + transform.position;
        Debug.Log("Destroy");
        Destroy(gameObject);
        yield return null;
    }

    void MoveDown()
    {
        while (CheckValidMove())
        {
            // ARManager.instance.txt.text += "Down ";
            transform.position += Down;
        }
        if (!CheckValidMove())
        {
            // ARManager.instance.txt.text += "up ";
            transform.position += Up;
        }
    }

    bool CheckValidMove()
    {
        foreach (Transform child in transform)
        {
            if (!Playfield.instance.CheckInsideContainer(child.position)) {
                // ARManager.instance.txt.text += "outside" + " " + child.position;
                // Debug.Log("outside" + child.position.ToString()+ "   " + transform.name+ "  t " + transform.position);
                return false;
            }
        }

        foreach (Transform child in transform) {
            Transform t = Playfield.instance.GetTransformOnContainerPos(child.position);
            // Debug.Log("!!!checkghost" + t.position + "   " + t.name + "  " + transform.name);
            if (t != null && t.parent == parent.transform) {
                // ARManager.instance.txt.text += "parrent" + " " + child.position;
                // Debug.Log("ContainerTrue" + child.position.ToString() + "   " + t.name + "  t " + t.position);
                return true;
            }

            if (t != null && t.parent != transform) {
                // ARManager.instance.txt.text += "not parent" + " " + child.position;
                // Debug.Log("ContainerFale" + child.position.ToString()+ "   " + t.name+ "  t " + t.position);
                return false;
            }
        }
        // Debug.Log("checkghost" + transform.position + "   " + transform.name);
        return true;
    }
}
