using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KontrolerZycia : MonoBehaviour
{
    public GameObject[] serca;
    public void WlaczZycia()
    {
        serca[0].SetActive(true);
        serca[1].SetActive(true);
        serca[2].SetActive(true);
    }

    public void OdejmijZycie(int zycie)
    {
        switch (zycie)
        {
            case 1:
                serca[0].SetActive(false);
                break;
            case 2:
                serca[1].SetActive(false);
                break;
            case 3:
                serca[2].SetActive(false);
                break;
        }
    }
}
