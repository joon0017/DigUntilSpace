using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class btnCTRLR : MonoBehaviour
{
    [SerializeField]
    private string sName;
    public Button btn;
    // Start is called before the first frame update
    void Start()
    {
        btn.onClick.AddListener(TaskOnClick);
        // sName = activateScene.name;
    }
    void TaskOnClick(){
        if (string.Equals(sName,"Exit")) {
            Debug.Log("Quit Game!"); 
            Application.Quit(); 
        }
        else SceneManager.LoadScene(sName);
    }
}