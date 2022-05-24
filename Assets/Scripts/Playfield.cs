using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
public class Playfield : MonoBehaviour
{
    public static Playfield instance;

    [Header("Field sizes")]
    [SerializeField]private int ContainerSizeX;
    [SerializeField]private int ContainerSizeY;
    [SerializeField]private int ContainerSizeZ;

    [Header("Tetrominoes")]
    [SerializeField]private GameObject[] blockList;
    [SerializeField]private GameObject[] ghostList;
    private Vector3 spawnPoint;

    [Header("Playfield visualisation")]
    [SerializeField]private GameObject bottomPlane;
    [SerializeField]private GameObject North, South, West, East;

    private int randomTetrominoes;
    public Transform[,,] Container;

    private Vector3 first_cell;
    public float size_cell = 0.1f;

    void Awake() 
    {
        instance = this; 
    }
    private void Start()
    {
        Container = new Transform[ContainerSizeX, ContainerSizeY, ContainerSizeZ];
        // // Debug.Log(transform.position.ToString());
        
        // // Debug.Log("first_cellv" + first_cell);
        if (ARManager.instance.gameStart) {
            NextTetromino();// next element random
            SpawnNewBlock();
        }
    }

    public void NextTetromino()
    {
        randomTetrominoes = Random.Range(0, blockList.Length);
        // TODO set model for preview
        // Previewer.instance.ShowPreview(randomIndex);
    }

    public void SpawnNewBlock()
    {
        // Vector3 spawnPoint = new Vector3(transform.position.x + size_cell/2 + swawnHelper[randomTetrominoes].x, 
        //                                 transform.position.y + (float)ContainerSizeY/10 / 2 + size_cell/2 + swawnHelper[randomTetrominoes].y, 
        //                                 transform.position.z + size_cell/2 + swawnHelper[randomTetrominoes].z);
        Vector3 spawnPoint = new Vector3(transform.position.x + size_cell/2 , 
                                        transform.position.y + (float)ContainerSizeY/10/2 - size_cell/2, 
                                        transform.position.z + size_cell/2);
        // ARManager.instance.txt.text += "SpawnNewBlock";
        // // Debug.Log(spawnPoint);
        Debug.Log("start new block");
        GameObject newBlock = Instantiate(blockList[randomTetrominoes], spawnPoint, Quaternion.identity) as GameObject;
        // TODO create class Ghost and set the ghost
        GameObject newGhost = Instantiate(ghostList[randomTetrominoes], spawnPoint, Quaternion.identity) as GameObject;
        Debug.Log("randomTetrominoes " + randomTetrominoes);
        // // Debug.Log("newBlock" + newBlock.name);
        newGhost.GetComponent<Ghost>().setParent(newBlock);

        NextTetromino();
    }

    // TODO maybe make optimal
    private bool CheckFullLayer(int y)
    {
        for (int x = 0; x < ContainerSizeX; x++) {
            for (int z = 0; z < ContainerSizeZ; z++) {
                if(Container[x,y,z] == null){
                    return false;
                }
            }
        }
        return true;
    }

    public void DeleteLayer()
    {
        int layersCleared = 0;
        for (int y = ContainerSizeY - 1; y >= 0; y--) {
            //Check full Layer
            if (CheckFullLayer(y)) {
                layersCleared++;
                //Delete some blocks
                DeleteLayerAt(y);
                MoveAllLayerDown(y);
                //Move all Down By 1
            }
        }
        if (layersCleared > 0) {
            GameManager.instance.LayersCleared(layersCleared);
        }
    }

    void DeleteLayerAt(int y)
    {
        for (int x = 0; x < ContainerSizeX; x++) {
            for (int z = 0; z < ContainerSizeZ; z++) {
                Destroy(Container[x, y, z].gameObject);
                // ? music add
                // FindObjectOfType<AudioManager>().Play("ClearLayer");
                Container[x, y, z] = null;
            }
        }
    }
    void MoveAllLayerDown(int y)
    {
        for (int i = y; i < ContainerSizeY; i++) {
            MoveOneLayerDown(i);
        }
    }

    void MoveOneLayerDown(int y)
    {
        for (int x = 0; x < ContainerSizeX; x++) {
            for (int z = 0; z < ContainerSizeZ; z++) {
                if (Container[x, y, z] != null) {
                    Container[x, y - 1, z] = Container[x, y, z];
                    Container[x, y, z] = null;
                    Container[x, y - 1, z].position +=  new Vector3(0, -size_cell, 0);
                }
            }
        }
    }

    private float CreateFirstCoordinate(float center, int diff) 
    {
        return (center - diff * size_cell + size_cell/2);
    }
    public Vector3Int ChangeCoordinate(Vector3 vec)
    {
        first_cell = new Vector3( CreateFirstCoordinate(transform.position.x, (int)ContainerSizeX/2), 
                                CreateFirstCoordinate(transform.position.y, (int)ContainerSizeY/2),
                                CreateFirstCoordinate(transform.position.z, (int)ContainerSizeZ/2));

        return new Vector3Int(Mathf.RoundToInt((vec.x - first_cell.x) / size_cell) ,
                              Mathf.RoundToInt((vec.y - first_cell.y) / size_cell) ,
                              Mathf.RoundToInt((vec.z - first_cell.z) / size_cell ));
    }

    public Transform GetTransformOnContainerPos(Vector3 pos)
    {
        // ARManager.instance.txt.text += "old " + pos;
        // // Debug.Log("position_REAL" + pos);
        Vector3Int new_pos = ChangeCoordinate(pos);
        // // Debug.Log("position_ROund" + new_pos);
        // ARManager.instance.txt.text += "new  " + pos;
        // ARManager.instance.txt.text += "ChangeCoordinate(pos);" + pos;
        if (pos.y > ContainerSizeY - 1) {
            return null;
        }
        else {
            // string tre = "";
            // foreach (var item in Container)
            // {
            //     tre += item.transform.name + "  ";
            // }
            // for (int x = 0; x < ContainerSizeX; x++) 
            // for (int z = 0; z < ContainerSizeZ; z++) 
            //     for (int y = 0; y < ContainerSizeY; y++) 
            //         if (Container[x,y,z] != null) 
            //             tre += Container[x,y,z].transform.name + "(" + x + "," + y + "," + z + " ;;";
            
            // // Debug.Log("Container" + tre);
            return Container[new_pos.x, new_pos.y, new_pos.z];;
        }
    }
    public bool CheckInsideContainer(Vector3 pos)
    {
        float dif = (float)ContainerSizeZ/10 / 2;
        float x_start = transform.position.x;
        float y_start = transform.position.y;
        float z_start = transform.position.z;

        //TODO Abs
        return (pos.x <= x_start + dif && pos.x >= x_start - dif &&
                pos.z <= z_start + dif && pos.z >= z_start - dif &&
                pos.y >= (y_start - (float)ContainerSizeY/10 / 2)); 
    }

    public void UpdateContainer(Tetromino block)
    {
        for (int x = 0; x < ContainerSizeX; x++) 
            for (int z = 0; z < ContainerSizeZ; z++) 
                for (int y = 0; y < ContainerSizeY; y++) 
                    if (Container[x,y,z] != null) 
                        if (Container[x, y, z].parent == block.transform) {
                            Container[x, y, z] = null;
                        }

        foreach (Transform child in block.transform) {
            Vector3Int pos = ChangeCoordinate(child.position);
            if (pos.y < ContainerSizeY) {
                Container[pos.x, pos.y, pos.z] = child;
            }
        }
    }
     public void UpdateContainer(GameObject block)
    {
        for (int x = 0; x < ContainerSizeX; x++) 
            for (int z = 0; z < ContainerSizeZ; z++) 
                for (int y = 0; y < ContainerSizeY; y++) 
                    if (Container[x,y,z] != null) 
                        if (Container[x, y, z].parent == block.transform) {
                            Container[x, y, z] = null;
                        }

        foreach (Transform child in block.transform) {
            Vector3Int pos = ChangeCoordinate(child.position);
            if (pos.y < ContainerSizeY) {
                Container[pos.x, pos.y, pos.z] = child;
            }
        }
    }
    void OnDrawGizmos()
    {
       
        bottomPlane.transform.position = new Vector3(transform.position.x,
                                                        transform.position.y - (float)ContainerSizeY/10 / 2,
                                                        transform.position.z);
        if (North != null) {
            North.transform.position = new Vector3(transform.position.x,
                                                   transform.position.y,
                                                   transform.position.z - (float)ContainerSizeZ/10 / 2);
        }

        if (South != null) {
            South.transform.position = new Vector3(transform.position.x,
                                                   transform.position.y,
                                                   transform.position.z + (float)ContainerSizeZ/10 / 2);
        }

        if (East != null) {
            East.transform.position = new Vector3(transform.position.x + (float)ContainerSizeX/10 / 2,
                                                    transform.position.y,
                                                    transform.position.z );
        }

        if (West != null) {
            West.transform.position = new Vector3(transform.position.x - (float)ContainerSizeX/10 / 2,
                                                    transform.position.y,
                                                    transform.position.z );
        }

    }

}
