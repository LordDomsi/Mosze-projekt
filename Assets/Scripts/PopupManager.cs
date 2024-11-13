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
    [SerializeField] float zoomInAnimSpeed;
    [SerializeField] float zoomOutAnimSpeed;
    [SerializeField] float zoomInSize;
    [SerializeField] float zoomOutSize;

    

    private void Awake()
    {
        Instance = this;
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

        VirtualCamera.m_Lens.OrthographicSize = targetSize;
    }

    public void StartBlackHoleAnim(GameObject gameObject)
    {
        StartCoroutine(PopupCurveAnim(gameObject, blackHoleAnimSpeed, BlackHoleCurve));
    }

    public void StartZoomInAnim()
    {
        StartCoroutine(CameraZoomAnim(ZoomInCurve, zoomInSize, zoomInAnimSpeed));
    }

    public void StartZoomOutAnim()
    {
        StartCoroutine(CameraZoomAnim(ZoomOutCurve, zoomOutSize, zoomOutAnimSpeed));
    }

    
    public IEnumerator TransitionZoom(GameObject player, Transform spawnPos)
    {
        PlayerMovement.Instance.canMove = false;
        PlayerMovement.Instance.StopPlayer();
        StartZoomInAnim();
        yield return new WaitForSeconds(zoomInAnimSpeed);//pályaváltásnál meg kell várni hogy bezoomoljon az animáció és utána vált pályát
        player.transform.position =spawnPos.position;
        if (StageManager.Instance.currentStage < 3) StageManager.Instance.NextStage();
        StartZoomOutAnim();
        PlayerMovement.Instance.canMove = true;
    }

    public void Transition(GameObject player, Transform spawnPos)
    {
        StartCoroutine(TransitionZoom(player, spawnPos));
    }

}
