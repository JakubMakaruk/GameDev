using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void GraczVsGracz()
    {
        SceneManager.LoadScene(2);
    }

    public void GraczVsKOMPUTER()
    {
        SceneManager.LoadScene(1);
    }

    public void Wyjscie()
    {
        Application.Quit();
    }
}
