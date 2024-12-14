using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupManager : MonoBehaviour
{
    // ebben a scriptben lesz minden ablak popup vagy bármilyen animációs script

    public static PopupManager Instance {  get; private set; }

    [SerializeField] private AnimationCurve BlackHoleCurve;
    [SerializeField] private float blackHoleAnimSpeed;
    [SerializeField] private CinemachineVirtualCamera VirtualCamera;
    [SerializeField] private AnimationCurve ZoomInCurve;
    [SerializeField] private AnimationCurve ZoomOutCurve;
    [SerializeField] private AnimationCurve PowerUpUICurveOpen;
    [SerializeField] private AnimationCurve PowerUpUICurveClose;
    [SerializeField] private AnimationCurve UIScaleForTransitionCurve;
    [SerializeField] private GameObject PowerUpUIGameObject;
    [SerializeField] private GameObject SpeedBoostUIGameObject;
    [SerializeField] private GameObject UIGameObject;

    [SerializeField] private float zoomInAnimSpeed;
    [SerializeField] private float zoomOutAnimSpeed;
    [SerializeField] private float zoomInSize;
    [SerializeField] private float zoomOutSize;
    
    public float powerUpUIAnimSpeed;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;
        }
    }

    

    //alap animáció
    public IEnumerator PopupCurveAnim(GameObject gameObject, float speed, AnimationCurve curve)
    {
        float curveTime = 0;
        float curveAmount = curve.Evaluate(curveTime);
        while (curveTime < speed)
        {
            curveTime += Time.deltaTime;
            float t = Mathf.Clamp01(curveTime / speed);
            curveAmount = curve.Evaluate(t);
            gameObject.transform.localScale = new Vector3 (curveAmount, curveAmount, curveAmount);
            yield return null;
        } 
    }

    //unscaled verzió hogy ne állítsa meg a timescale = 0
    public IEnumerator PopupCurveAnimUnscaled(GameObject gameObject, float speed, AnimationCurve curve)
    {
        float curveTime = 0;
        float curveAmount = curve.Evaluate(curveTime);
        while (curveTime < speed)
        {
            curveTime += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(curveTime / speed);
            curveAmount = curve.Evaluate(t);
            gameObject.transform.localScale = new Vector3(curveAmount, curveAmount, curveAmount);
            yield return null;
        }
    }

    public IEnumerator PopupCurveAnimReverse(GameObject gameObject, float speed, AnimationCurve curve)
    {
        float curveTime = 0;
        float curveAmount = curve.Evaluate(curveTime);
        while (curveTime < speed)
        {
            curveTime += Time.deltaTime;
            float t = Mathf.Clamp01(curveTime / speed);
            curveAmount = curve.Evaluate(t);
            curveAmount = 1 - curveAmount;
            gameObject.transform.localScale = new Vector3(curveAmount, curveAmount, curveAmount);
            yield return null;
        }

    }

    public IEnumerator PopupCurveAnimReverseUnscaled(GameObject gameObject, float speed, AnimationCurve curve)
    {
        float curveTime = 0;
        float curveAmount = curve.Evaluate(curveTime);
        while (curveTime < speed)
        {
            curveTime += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(curveTime / speed);
            curveAmount = curve.Evaluate(t);
            curveAmount = 1 - curveAmount;
            gameObject.transform.localScale = new Vector3(curveAmount, curveAmount, curveAmount);
            yield return null;
        }

    }

    //kamera zoom animáció
    public IEnumerator CameraZoomAnim(AnimationCurve curve, float targetSize, float zoomAnimSpeed)
    {
        float startSize = VirtualCamera.m_Lens.OrthographicSize;
        float curveTime = 0;
        float curveAmount = curve.Evaluate(curveTime);
        while (curveTime < zoomAnimSpeed)
        {
            curveTime += Time.deltaTime;
            float t = Mathf.Clamp01(curveTime/zoomAnimSpeed);

            curveAmount = curve.Evaluate(t);
            VirtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(startSize, targetSize, curveAmount);
            yield return null;
        }
        if(targetSize == zoomOutSize) { GameStateManager.Instance.allowedToPause = true; }
        VirtualCamera.m_Lens.OrthographicSize = targetSize;
    }
    public IEnumerator TransitionZoom(GameObject player, Transform spawnPos)
    {
        GameStateManager.Instance.allowedToPause = false;
        PlayerMovement.Instance.canMove = false;
        PlayerMovement.Instance.StopPlayer();
        AudioManager.Instance.PlaySFX(AudioManager.SFX_enum.ENTER_BLACKHOLE);
        StartZoomInAnim();
        StartUIScaleAnimClose();
        yield return new WaitForSeconds(zoomInAnimSpeed);//pályaváltásnál meg kell várni hogy bezoomoljon az animáció és utána vált pályát
        player.transform.position = spawnPos.position;
        if (StageManager.Instance.currentStage < 3) StageManager.Instance.NextStage();
        StartZoomOutAnim();
        StartUIScaleAnimOpen();
        PlayerMovement.Instance.canMove = true;
    }

    public void StartBlackHoleAnim(GameObject gameObject)
    {
        StartCoroutine(PopupCurveAnimUnscaled(gameObject, blackHoleAnimSpeed, BlackHoleCurve));
    }

    public void StartZoomInAnim()
    {
        StartCoroutine(CameraZoomAnim(ZoomInCurve, zoomInSize, zoomInAnimSpeed));
    }

    public void StartZoomOutAnim()
    {
        StartCoroutine(CameraZoomAnim(ZoomOutCurve, zoomOutSize, zoomOutAnimSpeed));
    }

    public void StartPowerUpUIAnimOpen()
    {
        StartCoroutine(PopupCurveAnim(PowerUpUIGameObject, powerUpUIAnimSpeed, PowerUpUICurveOpen));
    }

    public void StartPowerUpUIAnimClose()
    {
        StartCoroutine(PopupCurveAnimReverse(PowerUpUIGameObject, powerUpUIAnimSpeed, PowerUpUICurveClose));
    }

    public void StartSpeedBoostUIAnimOpen()
    {
        StartCoroutine(PopupCurveAnim(SpeedBoostUIGameObject, powerUpUIAnimSpeed, PowerUpUICurveOpen));
    }

    public void StartSpeedBoostUIAnimClose()
    {
        StartCoroutine(PopupCurveAnimReverse(SpeedBoostUIGameObject, powerUpUIAnimSpeed, PowerUpUICurveClose));
    }

    public void Transition(GameObject player, Transform spawnPos)
    {
        StartCoroutine(TransitionZoom(player, spawnPos));
    }

    public void StartUIScaleAnimClose()
    {
        StartCoroutine(PopupCurveAnimReverseUnscaled(UIGameObject, zoomInAnimSpeed, UIScaleForTransitionCurve));
    }
    public void StartUIScaleAnimOpen()
    {
        StartCoroutine(PopupCurveAnimUnscaled(UIGameObject, zoomOutAnimSpeed, ZoomOutCurve));
    }

}
