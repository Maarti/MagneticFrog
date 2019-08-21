
using UnityEngine;

public class AndroidCheater : MonoBehaviour
{
    int nbClick = 0;

    public void Start() {
        Debug.LogWarningFormat("DONT FORGET to remove Cheater script on {0}", gameObject.name);
    }

    public void Money() {
        nbClick++;
        if (nbClick >= 5) {
            ApplicationController.ac.UpdateCoins(5);
            nbClick = 0;
        }

    }
}
