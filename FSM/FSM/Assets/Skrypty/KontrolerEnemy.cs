using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum OpponentStany
{
    ruch, chodzenie, atak, odepchniecie
}

public class KontrolerEnemy : MonoBehaviour
{
    public OpponentStany obecnyStan;
    public int zycie;
    public int wartoscAtaku;
    public float predkosc;

    public void Uderzenie(Rigidbody2D myRigidbody, float czasUderzenia)
    {
        StartCoroutine(KopniecieCO(myRigidbody, czasUderzenia));
    }

    private IEnumerator KopniecieCO(Rigidbody2D myRigidbody, float czasUderzenia)
    {
        if (myRigidbody != null)
        {
            yield return new WaitForSeconds(czasUderzenia);
            myRigidbody.velocity = Vector2.zero;
            obecnyStan = OpponentStany.ruch;
            myRigidbody.velocity = Vector2.zero;
        }
    }
}
