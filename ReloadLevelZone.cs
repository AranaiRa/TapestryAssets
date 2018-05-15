using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReloadLevelZone : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("hit reload zone");
        if (other.gameObject.GetComponentInParent<Tapestry_Player>() != null)
        {
            Debug.Log("aadaadsfas");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
