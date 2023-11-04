using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ambushTrigger : MonoBehaviour
{
    public GameObject AmbushText;

    private void Start()
    {
        AmbushText.SetActive(false);
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(2);
        AmbushText.SetActive(false);
        SceneManager.LoadScene("testSwitch");
    }

    void OnMouseDown(){
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("AmbushText");
            AmbushText.SetActive(true);
            StartCoroutine(Wait());
        }
    }
}
