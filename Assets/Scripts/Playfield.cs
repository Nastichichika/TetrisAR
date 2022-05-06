using UnityEngine;
using UnityEngine.UI;
public class Playfield : MonoBehaviour
{
    public static Playfield instance;

    [Header("Field sizes")]
    [SerializeField]private int gridSizeX;
    [SerializeField]private int gridSizeY;
    [SerializeField]private int gridSizeZ;

    [Header("Tetrominoes")]
    public GameObject[] blockList;
    public GameObject[] ghostList;
    public Vector3 spawnPoint;

    [Header("Playfield visualisation")]
    public GameObject bottomPlane;
    public GameObject North, South, West, East;

    private int randomTetrominoes;
    public Transform[,,] TheGrid;

    private Vector3 first_cell;
    private float size_cell = 0.1f;

    void Awake()
    {
        instance = this; 
    }

    private float CreateFirstCoordinate(float center, int diff) {
        return (center - diff * size_cell + size_cell/2);
    }
    private void Start()
    {
        TheGrid = new Transform[gridSizeX, gridSizeY, gridSizeZ];
        Debug.Log(transform.position.ToString());
        first_cell = new Vector3( CreateFirstCoordinate(transform.position.x, 3), 
                                CreateFirstCoordinate(transform.position.y, 5),
                                CreateFirstCoordinate(transform.position.z, 3));
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
        // Vector3 spawnPoint = new Vector3(((int)(transform.position.x + (float)gridSizeX/ 2))/100,
        //                                  ((int)transform.position.y + gridSizeY) /100,
        //                                  ((int)(transform.position.z * 100+ (float)gridSizeZ * 100/ 2)))/100;
        // Vector3 spawnPoint = new Vector3((int)(transform.position.x + (float)gridSizeX / 2),
        //                                  (int)transform.position.y + gridSizeY,
        //                                  (int)(transform.position.z + (float)gridSizeZ / 2));
        Vector3 spawnPoint = new Vector3(transform.position.x + 0.05f, 
                                        transform.position.y + (float)gridSizeY/10 / 2 + 0.05f, 
                                        transform.position.z + 0.05f);
        ARManager.instance.txt.text = "SpawnNewBlock";
        Debug.Log(spawnPoint);
        // //ARManager.instance.txt.text = "wet";
        // ARManager.instance.txt.text += "point" + (int)(transform.position.x * 100 + (float)gridSizeX * 100/ 2) + " " + ((int)transform.position.y * 100+ gridSizeY* 100) + " " + (transform.position.z * 100+ (float)gridSizeZ * 100/ 2);
        // ARManager.instance.txt.text += "point" + transform.position.x + " " + transform.position.y + " " + transform.position.z + spawnPoint;
        // ARManager.instance.txt.text += "point" + x_start + " " + y_start + " " + z_start;
        // spawnPoint = new Vector3(x_start + 0.025f,
        //                                  y_start + 0.8f,
        //                                  z_start + 0.025f);
        // Spawn The Block
        GameObject newBlock = Instantiate(blockList[randomTetrominoes], spawnPoint, Quaternion.identity) as GameObject;
        // ARManager.instance.txt.text += "spawnPoint" + ((int)(transform.position.x * 100 + (float)gridSizeX * 100/ 2))/100 + " " +
        // ((int)transform.position.y * 100+ gridSizeY* 100) /100  + " " + ((int)(transform.position.z * 100+ (float)gridSizeZ * 100/ 2))/100 ;
        // TODO create class Ghost and set the ghost
        // GameObject newGhost = Instantiate(ghostList[randomTetrominoes], spawnPoint, Quaternion.identity) as GameObject;
        // newGhost.GetComponent<Ghost>().setParent(newBlock);

        NextTetromino();
    }

    // TODO maybe make optimal
    private bool CheckFullLayer(int y)
    {
        for (int x = 0; x < gridSizeX; x++) {
            for (int z = 0; z < gridSizeZ; z++) {
                if(TheGrid[x,y,z] == null){
                    return false;
                }
            }
        }
        return true;
    }

    public void DeleteLayer()
    {
        int layersCleared = 0;
        for (int y = gridSizeY - 1; y >= 0; y--) {
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
        for (int x = 0; x < gridSizeX; x++) {
            for (int z = 0; z < gridSizeZ; z++) {
                Destroy(TheGrid[x, y, z].gameObject);
                // ? music add
                // FindObjectOfType<AudioManager>().Play("ClearLayer");
                TheGrid[x, y, z] = null;
            }
        }
    }
    void MoveAllLayerDown(int y)
    {
        for (int i = y; i < gridSizeY; i++) {
            MoveOneLayerDown(i);
        }
    }

    void MoveOneLayerDown(int y)
    {
        for (int x = 0; x < gridSizeX; x++) {
            for (int z = 0; z < gridSizeZ; z++) {
                if (TheGrid[x, y, z] != null) {
                    TheGrid[x, y - 1, z] = TheGrid[x, y, z];
                    TheGrid[x, y, z] = null;
                    TheGrid[x, y - 1, z].position +=  new Vector3(0, -0.1f, 0);
                }
            }
        }
    }

    public Vector3 Round(Vector3 vec)
    {
        return new Vector3((vec.x - first_cell.x) / size_cell - 1,
                            (vec.y - first_cell.y) / size_cell - 1,
                             (vec.z - first_cell.z) / size_cell - 1);
    }

    public Transform GetTransformOnGridPos(Vector3 pos)
    {
        //ARManager.instance.txt.text += "old " + pos;
        pos = Round(pos);
        //ARManager.instance.txt.text += "new  " + pos;
        ARManager.instance.txt.text += "Round(pos);" + pos;
        if (pos.y > gridSizeY - 1) {
            return null;
        }
        else {
            return TheGrid[(int)pos.x, (int)pos.y, (int)pos.z];
        }
    }
    public bool CheckInsideGrid(Vector3 pos)
    {
        float dif = (float)gridSizeZ/10 / 2;
        float x_start = transform.position.x;
        float y_start = transform.position.y;
        float z_start = transform.position.z;
        Debug.Log("pos " + pos);
        // ARManager.instance.txt.text += "pos" + pos + "    ";
        // for (int x = 0; x < gridSizeX; x++) 
        //             for (int z = 0; z < gridSizeZ; z++) 
        //                 for (int y = 0; y < gridSizeY; y++) { 
        //                     ARManager.instance.txt.text += "the grid" + (TheGrid[x, y, z] ? TheGrid[x, y, z].position : "null") + " " ;
        //                 }
        // ARManager.instance.txt.text += "x" + (pos.x <= x_start + dif) + "    " + (pos.x >= x_start - dif);
        // ARManager.instance.txt.text += "z" + (pos.z <= z_start + dif) + "    " + (pos.z >= z_start - dif);
        // ARManager.instance.txt.text += "y" + (pos.y >= (y_start - (float)gridSizeY/10 / 2)) + " ";
            //  "    " + z_start +  " " + ( z_start + dif) +
            //  "    " + y_start;

        // ARManager.instance.txt.text += "res" + (pos.x <= x_start + dif && pos.x >= x_start - dif &&
        //         pos.z <= z_start + dif && pos.z >= z_start + dif &&
        //         pos.y >= (y_start - (float)gridSizeY/10 / 2)) + " ";
        //TODO Abs
        return (pos.x <= x_start + dif && pos.x >= x_start - dif &&
                pos.z <= z_start + dif && pos.z >= z_start - dif &&
                pos.y >= (y_start - (float)gridSizeY/10 / 2)); 
    }

    public void UpdateGrid(Tetromino block)
    {
        for (int x = 0; x < gridSizeX; x++) 
            for (int z = 0; z < gridSizeZ; z++) 
                for (int y = 0; y < gridSizeY; y++) 
                    if (TheGrid[x,y,z] != null) 
                        if (TheGrid[x, y, z].parent == block.transform) {
                            TheGrid[x, y, z] = null;
                        }

        foreach (Transform child in block.transform) {
            Vector3 pos = Round(child.position);
            if (pos.y < gridSizeY) {
                TheGrid[(int)pos.x, (int)pos.y, (int)pos.z] = child;
            }
        }
    }
    void OnDrawGizmos()
    {
        // Gizmos.color = Color.red;
        // // TheGrid[0, 0, 0] = transform;
        // Gizmos.DrawCube(new Vector3(transform.position.x + 0.05f, 
        //                             transform.position.y + (float)gridSizeY/10 / 2 + 0.05f, 
        //                             transform.position.z + 0.05f), new Vector3(0.1f, 0.1f, 0.1f));
        // //GameObject newBlock = Instantiate(blockList[1], spawnPoint, Quaternion.identity) as GameObject;
        // blockList[1].transform.position += new Vector3(transform.position.x + 0.05f, 
        //                             transform.position.y + (float)gridSizeY/10 / 2 + 0.05f, 
        //                             transform.position.z + 0.05f);
        
        //RESIZE BOTTOM PLANE
        //Vector3 scaler = new Vector3((float)gridSizeX/10, 1, (float)gridSizeZ / 10);
        // bottomPlane.transform.localScale = scaler;
        //REPOSITION
        bottomPlane.transform.position = new Vector3(transform.position.x,
                                                        transform.position.y - (float)gridSizeY/10 / 2,
                                                        transform.position.z);
        //RETILE MATERIAL
        //bottomPlane.GetComponent<MeshRenderer>().sharedMaterial.mainTextureScale = new Vector2(gridSizeX, gridSizeZ);
        if (North != null)
        {
            //RESIZE BOTTOM PLANE
            // North.transform.localScale = scaler;

            //REPOSITION
            North.transform.position = new Vector3(transform.position.x,
                                                   transform.position.y,
                                                   transform.position.z - (float)gridSizeZ/10 / 2);

            //RETILE MATERIAL
            //North.GetComponent<MeshRenderer>().sharedMaterial.mainTextureScale = new Vector2(gridSizeX, gridSizeY);
        }

        if (South != null)
        {
            //RESIZE BOTTOM PLANE
            //South.transform.localScale = scaler;

            //REPOSITION
            South.transform.position = new Vector3(transform.position.x,
                                                   transform.position.y,
                                                   transform.position.z + (float)gridSizeZ/10 / 2);

            //RETILE MATERIAL
            //S.GetComponent<MeshRenderer>().sharedMaterial.mainTextureScale = new Vector2(gridSizeX, gridSizeY);
        }

        if (East != null)
        {
            //RESIZE BOTTOM PLANE
            
            // East.transform.localScale = scaler;

            //REPOSITION
            East.transform.position = new Vector3(transform.position.x + (float)gridSizeX/10 / 2,
                                                    transform.position.y,
                                                    transform.position.z );

            //RETILE MATERIAL
            //East.GetComponent<MeshRenderer>().sharedMaterial.mainTextureScale = new Vector2(gridSizeZ, gridSizeY);
        }

        if (West != null)
        {
            //RESIZE BOTTOM PLANE
            // West.transform.localScale = scaler;

            //REPOSITION
            West.transform.position = new Vector3(transform.position.x - (float)gridSizeX/10 / 2,
                                                    transform.position.y,
                                                    transform.position.z );
            //RETILE MATERIAL
            //West.GetComponent<MeshRenderer>().sharedMaterial.mainTextureScale = new Vector2(gridSizeZ, gridSizeY);
        }

    }

}
