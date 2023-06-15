using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using System;
using System.Text;
using UnityEngine.XR;
public class Statistics : MonoBehaviour
{
    public string CustomText = "";
    int frameCount = 0;
    float dt = 0;
    readonly List<float> measurements = new();
    readonly List<float> measurementsGpu = new();
    readonly float dtRate = 1.0f / 4.0f;
    readonly string fileDate = DateTime.Now.ToString("yyyyMMddHHmm");
    void Start()
    {
        Debug.Log($"PATH TO FILE: {Path.Combine(Application.persistentDataPath, "results.txt")}");
        StartCoroutine(SaveToFile(10.0f));
    }
    void Awake()
    {
#if ENABLE_VR && UNITY_2017_1_OR_NEWER
        if (XRSettings.enabled)
        {
            var thisCamera = Camera.main.gameObject.GetComponent<Camera>();
            if (thisCamera != null)
            {
                XRDevice.DisableAutoXRCameraTracking(thisCamera, true);
            }
        }
        Unity.XR.Oculus.Stats.PerfMetrics.EnablePerfMetrics(true);
        OVRPlugin.systemDisplayFrequency = 120.0f;
#endif
    }

    void Update()
    {
        //fps
        frameCount++;
        dt += Time.unscaledDeltaTime;
        if (dt > dtRate)
        {
            measurements.Add(frameCount / dt);
            frameCount = 0;
            dt -= dtRate;
#if ENABLE_VR && UNITY_2017_1_OR_NEWER
            //measurementsGpu.Add(Unity.XR.Oculus.Stats.PerfMetrics.AppGPUTime);
#endif
        }
    }

    private IEnumerator SaveToFile(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        while (true)
        {
            StringBuilder line = new();
            for (int i = 0; i < measurements.Count; ++i)
                line.Append(measurements[i].ToString() + ";");
            line.Append("\n");

            StringBuilder line2 = new();
            for (int i = 0; i < measurementsGpu.Count; ++i)
                line2.Append(measurementsGpu[i].ToString() + ";");
            line2.Append("\n");

            File.AppendAllLines(Path.Combine(Application.persistentDataPath, $"{fileDate}_results_{CustomText.Replace(' ', '-')}.txt"), new[]
            {
                $"{CustomText};{SceneManager.GetActiveScene().name};{DateTime.Now}",
                line.ToString(),
                line2.ToString()
            });
            yield return new WaitForSeconds(waitTime);
        }
    }

}
