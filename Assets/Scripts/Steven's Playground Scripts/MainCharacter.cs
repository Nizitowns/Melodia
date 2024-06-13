using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacter : MonoBehaviour
{
    public void wakeUp()
    {
        transform.rotation = Quaternion.identity;
    }
}
