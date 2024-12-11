using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoaderCallback : MonoBehaviour
{
    private bool isFirstUpdate = true;
    //amikor bet�lt a load scene akkor visszajelez a Loader scriptnek hogy elkezdheti bet�lteni a rendes j�t�kot
    //erre az�rt van sz�ks�g hogy amikor a j�t�kos r�nyom a continue/newgame gombra akkor ne fagyjon be am�g bet�lt a j�t�k, helyette kap egy loading scene-t
    //ahelyett hogy befagyna a men�ben am�g t�lt, helyette bet�lti a load scenet ami sokkal kisebb scene ez�rt instant bet�lt �s onnan t�lt�dik be a j�t�k
    private void Update()
    {
        if (isFirstUpdate)
        {
            isFirstUpdate = false;

            Loader.LoaderCallback();
        }
    }
}
