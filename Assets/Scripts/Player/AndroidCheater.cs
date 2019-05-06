
using UnityEngine;

public class AndroidCheater : MonoBehaviour
{
    int nbClick = 0;

    public void Start() {
        Debug.LogFormat("DONT FORGET to remove Cheater script on {0}", gameObject.name);
    }

    public void Money() {
        nbClick++;
        if (nbClick >= 10) {
            ApplicationController.ac.UpdateCoins(99999);
            nbClick = 0;
        }

    }
}
