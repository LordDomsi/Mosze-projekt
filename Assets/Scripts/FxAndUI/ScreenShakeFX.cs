using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShakeFX : MonoBehaviour
{
    public static ScreenShakeFX Instance { get; private set; }

    [SerializeField]private CinemachineVirtualCamera cinemachineCamera;

    private void Awake()
    {
        Instance = this;
    }

    public void ShakeCamera(float intensity, float time)
    {
        StartCoroutine(ShakeTimer(intensity, time));
    }

    private IEnumerator ShakeTimer(float intensity, float time)
    {

        if (cinemachineCamera != null)
        {
            CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = cinemachineCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

            cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;

            float t = 0;
            while (t <= time)
            {
                t += Time.deltaTime;
                cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = Mathf.Lerp(intensity, 0f, t / time);
                yield return null;
            }

            cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0f;
        }
    }
}
