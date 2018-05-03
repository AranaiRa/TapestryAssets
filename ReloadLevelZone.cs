using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReloadLevelZone : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<Tapestry_Player>() != null)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
