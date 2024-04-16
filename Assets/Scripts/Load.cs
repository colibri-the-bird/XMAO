using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Load : MonoBehaviour
{


    public GameObject[] Details;
    public Transform parent;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            this.GetComponent<SaveLoadManager>().LoadScene();
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            SceneManager.LoadScene("SampleScene");
        }
    }
    public void LoadS(Save.DetailSaveData detail)
    {
        var clone = Instantiate(Details[(int)detail.ID], new Vector3(detail.Position.x, detail.Position.y, detail.Position.z), Quaternion.identity, parent);
        clone.transform.eulerAngles = new Vector3(detail.Rotation.x, detail.Rotation.y, detail.Rotation.z);
        clone.transform.localScale = new Vector3(detail.Scale.x, detail.Scale.y, detail.Scale.z);
    }
    public void DelChilds()
    {
        foreach (Transform child in parent)
        {
            Destroy(child.gameObject);
        }
    }
}
