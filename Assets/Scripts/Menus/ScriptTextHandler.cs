
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScriptTextHandler : MonoBehaviour {

    public int SceneIdForTerrainView = 0;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetButtonDown("Fire1") )
	    {
	        SceneManager.LoadScene(SceneIdForTerrainView);
        }
    }
}
