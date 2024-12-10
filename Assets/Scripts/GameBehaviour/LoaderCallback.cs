using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoaderCallback : MonoBehaviour
{
    private bool isFirstUpdate = true;
    //amikor betölt a load scene akkor visszajelez a Loader scriptnek hogy elkezdheti betölteni a rendes játékot
    //erre azért van szükség hogy amikor a játékos rányom a continue/newgame gombra akkor ne fagyjon be amíg betölt a játék, helyette kap egy loading scene-t
    //ahelyett hogy befagyna a menüben amíg tölt, helyette betölti a load scenet ami sokkal kisebb scene ezért instant betölt és onnan töltõdik be a játék
    private void Update()
    {
        if (isFirstUpdate)
        {
            isFirstUpdate = false;

            Loader.LoaderCallback();
        }
    }
}
