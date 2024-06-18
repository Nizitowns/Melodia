using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacter : MonoBehaviour
{
    public void wakeUp()
    {
        Vector3 rotDifference = transform.rotation.eulerAngles - Quaternion.identity.eulerAngles;
        transform.rotation = Quaternion.identity;
        transform.Find("SaturationEffect").rotation = Quaternion.Euler(transform.Find("SaturationEffect").rotation.eulerAngles + rotDifference);
    }
}
