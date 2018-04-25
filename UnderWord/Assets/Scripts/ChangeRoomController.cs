using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeRoomController : MonoBehaviour {
    public string loadname;
    public string unloadname;
    void OnTriggerEnter2D(Collider2D col)
    {
        if (loadname != "")
        {
            MySceneManager.Instance.Load(loadname);
        }
        if (unloadname != "")
        {
            StartCoroutine("UnloadScene");
        }
    }
    IEnumerator UnloadScene()
    {
        yield return new WaitForSeconds(.10f);
        MySceneManager.Instance.Unload(unloadname);
    }
}
