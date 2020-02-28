using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Odepchniecie : MonoBehaviour
{

    public float wartoscPchniecia;
    public float czasPchniecia;
    public int dmg;

    private void OnTriggerEnter2D(Collider2D cos)
    {
        if(cos.gameObject.CompareTag("Opponent") || cos.gameObject.CompareTag("Player"))
        {
            Rigidbody2D uderzenie = cos.GetComponent<Rigidbody2D>();
            if(uderzenie != null)
            {
                Vector2 roznica = uderzenie.transform.position - transform.position;
                roznica = roznica.normalized * wartoscPchniecia;
                uderzenie.AddForce(roznica, ForceMode2D.Impulse);
                if(cos.gameObject.CompareTag("Opponent"))
                {
                    uderzenie.GetComponent<KontrolerEnemy>().obecnyStan = OpponentStany.odepchniecie;
                    cos.GetComponent<KontrolerEnemy>().Uderzenie(uderzenie, czasPchniecia);
                }
                if(cos.gameObject.CompareTag("Player"))
                {
                    if (cos.GetComponent<KontrolerGracza>().obecnyStan != GraczStany.odepchniecie)
                    {
                        uderzenie.GetComponent<KontrolerGracza>().obecnyStan = GraczStany.odepchniecie;
                        cos.GetComponent<KontrolerGracza>().Uderzenie(czasPchniecia, dmg);
                    }
                }
            }
        }
    }
}

