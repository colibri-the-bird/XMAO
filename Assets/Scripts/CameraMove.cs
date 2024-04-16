using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static System.Net.Mime.MediaTypeNames;

public class CameraMove : MonoBehaviour
{
    public float Speed;
    public float rotationSpeed;

    [SerializeField] public List<float[]> list = new List<float[]>();
    public GameObject[] Prefs;
    private int DetailID;
    private GameObject Pref;

    private float Scale;
    private float RotationX;
    private float RotationY;
    private float RotationZ;


    private GameObject clone;
    private GameObject CopiedClone;
    public GameObject arrows;
    private GameObject Carrows;
    private bool arr;

    public Transform ParentObj;
    public Vector3 BlockPosition;
    private int count;
    private bool Selected;

    bool isObjectHere(Vector3 position)
    {
        Collider[] intersecting = Physics.OverlapSphere(position, 0.01f);
        if (intersecting.Length == 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {

        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        if (Input.GetKey(KeyCode.LeftShift))
        {
            moveHorizontal = moveHorizontal * 5;
            moveVertical = moveVertical * 5;
        }
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        transform.Translate(movement * Speed * Time.fixedDeltaTime);
        if (Input.GetMouseButton(1))
        {

            //Camera Rotation
            this.transform.Rotate(new Vector3(0, Input.GetAxis("Mouse X"), 0) * rotationSpeed * Time.deltaTime, Space.World);
            this.transform.Rotate(new Vector3(-Input.GetAxis("Mouse Y"), 0, 0) * rotationSpeed * Time.deltaTime, Space.Self);
        }
    }
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.H))
        {
            Camera.main.GetComponent<SaveLoadManager>().SaveScene();
        }
        /*if (Input.GetKeyDown(KeyCode.L))
        {
            Camera.main.GetComponent<SaveLoadManager>().LoadScene();
        }*/
        if (Input.GetKey("escape"))
        {
            UnityEngine.Application.Quit();
        }
        Pref = Prefs[DetailID];
        if (!Selected)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                DetailID = 0;
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                DetailID = 1;
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                DetailID = 2;
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                DetailID = 3;
            }
            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                DetailID = 4;
            }
            if (arr)
            {
                Destroy(Carrows);
                arr = false;
            }
            clone = null;
            if (Input.GetKeyDown(KeyCode.E))
            {
                CreateDetail(Pref, false);
            }
            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.V) && CopiedClone != null)
            {
                CreateDetail(CopiedClone, true);
            }
/*            if (Input.GetKeyDown(KeyCode.E))
            {
                CreateDetail(Pref, false);
            }*/
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out RaycastHit hitInfo))
                {
                    if (hitInfo.collider.gameObject.GetComponent<MouseSelect>() != null)
                    {
                        clone = hitInfo.collider.gameObject;
                        Selected = true;
                    }
                }
            }

        }
        else
        {
            if (clone != null)
            {
                DetailID = (int)(list[int.Parse(clone.name)][7]);
                clone.GetComponent<Collider>().enabled = false;
                clone.GetComponent<Renderer>().material.color = Color.blue;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (!arr)
                {
                    Carrows = Instantiate(arrows, BlockPosition, Quaternion.identity, ParentObj);
                    arr = true;
                }
                if (Physics.Raycast(ray, out RaycastHit hitInfo))
                {
                    BlockPosition = Vector3Int.RoundToInt(hitInfo.point + hitInfo.normal / 2);
                    if (!isObjectHere(BlockPosition))
                    {
                        clone.transform.position = BlockPosition;
                        if (arr) Carrows.transform.position = clone.transform.position;
                    }
                    if (Input.GetMouseButtonDown(0))
                    {
                        list[int.Parse(clone.name)] = new float[] { clone.transform.position.x, clone.transform.position.y, clone.transform.position.z, RotationX, RotationY, RotationZ, Scale, DetailID };
                        Selected = false;
                        clone.GetComponent<Collider>().enabled = true;
                        clone.GetComponent<Renderer>().material.color = Color.white;
                        /*                        if (clone.GetComponent<Trube>() != null)
                                                {
                                                    clone.GetComponent<Trube>().Physics();
                                                }*/
                    }
                    if (Input.GetKeyDown(KeyCode.R))
                    {
                        clone.transform.Rotate(0.0f, 90.0f, 0.0f, Space.Self);
                    }
                    if (Input.GetKeyDown(KeyCode.Q))
                    {
                        clone.transform.Rotate(90.0f, 0.0f, 0.0f, Space.Self);
                    }
                }
                if (Input.GetKeyDown(KeyCode.Z))
                {

                    list.RemoveAt(int.Parse(clone.name));
                    Destroy(clone);
                    Selected = false;
                    count--;
                    StartCoroutine(NameUpdate());
                }
                if (Input.GetKey(KeyCode.X) && Input.GetMouseButton(2) && DetailID != 4 && DetailID != 5 && DetailID != 6)
                {
                    if (clone.transform.localScale.x > 1)
                    {
                        clone.transform.localScale = new Vector3(1, 1, 1);
                    }
                    if (clone.transform.localScale.x < 0.1f)
                    {
                        clone.transform.localScale = new Vector3(0.1f, 0.1f, 1);
                    }
                    else clone.transform.localScale += new Vector3(Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse Y"), 0);
                }
                if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.C))
                {
                    CopiedClone = clone;
                }
                Scale = clone.transform.localScale.x;
                RotationX = clone.transform.eulerAngles.x;
                RotationY = clone.transform.eulerAngles.y;
                RotationZ = clone.transform.eulerAngles.z;

            }
        }
    }
    private void CreateDetail(GameObject detail, bool isRotate)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            BlockPosition = Vector3Int.RoundToInt(hitInfo.point + hitInfo.normal / 2);
            if (!isObjectHere(BlockPosition))
            {
                clone = Instantiate(detail, BlockPosition, Quaternion.identity, ParentObj);
                clone.name = count.ToString();
                if (isRotate)
                {
                    clone.transform.eulerAngles = new Vector3(RotationX, RotationY, 0);
                }
                list.Add(new float[] { clone.transform.position.x, clone.transform.position.y, clone.transform.position.z, RotationX, RotationY, RotationZ, Scale, DetailID });
                count++;
                Selected = true;
            }
        }
    }
    /*    public void Test(Save.DetailSaveData detail) 
        {
            print(detail.Position.x + "," + detail.Rotation.x + "," + detail.Scale.x + "," + detail.ID);
        }*/
    private IEnumerator NameUpdate()
    {
        yield return new WaitForSeconds(Time.fixedDeltaTime);
        var i = 0;
        foreach (Transform g in ParentObj)
        {
            if (g.name != ParentObj.name)
            {
                g.gameObject.name = i.ToString();
                i++;
            }
        }
    }

    public void Saw()
    {
        DetailID = 0;
    }
    public void Common_Build()
    {
        DetailID = 1;
    }
    public void Oil()
    {
        DetailID = 2;
    }
    public void Theatre()
    {
        DetailID = 3;
    }
    public void Factory()
    {
        DetailID = 4;
    }
}
