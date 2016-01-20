using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FpsCounter : MonoBehaviour 
{
    public Text FpsLabel;
    public Text FrameTimeMinLabel;
    public Text FrameTimeMaxLabel;
    public float interval;

    int frames;
    MinMax frameTime;
    float timer;

	void Start () 
    {
        frames = 0;
        timer = 0;
        frameTime.min = frameTime.max = 0;
	}

    void Update()
    {
        timer += Time.deltaTime;
        ++frames;
        frameTime.min = Mathf.Min(frameTime.min, Time.deltaTime);
        frameTime.max = Mathf.Max(frameTime.max, Time.deltaTime);

        if (timer >= interval)
        {
            FpsLabel.text = "FPS: " + (frames / timer).ToString("0.0");
            FrameTimeMinLabel.text = "MIN: " + (frameTime.min * 1000.0f).ToString("0.0");
            FrameTimeMaxLabel.text = "MAX: " + (frameTime.max * 1000.0f).ToString("0.0");

            timer = 0;
            frames = 0;
            frameTime.min = frameTime.max = Time.deltaTime;
        }
    }
}
