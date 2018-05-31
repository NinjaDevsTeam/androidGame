using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartRoom : MonoBehaviour
{

    private void Awake()
    {
        BoardState boardState = GameObject.FindGameObjectWithTag("BoardState").GetComponent<BoardState>();
        if (boardState.roomInfos != null && boardState.roomInfos.Count > 0)
        {
            var t = GameObject.FindGameObjectsWithTag("SpawnPoints");
            foreach (var o in t)
                Destroy(o);
        }
    }
    //        foreach (var pair in boardState.rooms)
    //            Instantiate(pair.Value, pair.Key, pair.Value.transform.rotation);
    //    }
    //}

}
