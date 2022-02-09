using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Proceed : MonoBehaviour
{
    // Start is called before the first frame update
    public void Continue()
    {
        ChangeScene.LoadActMap();
    }
}
