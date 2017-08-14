using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SplineWalkerMode
{
    once,
    loop,
    pingpong
}

public class SplineWalker : MonoBehaviour {

    public BezierSpline spline;
    public float duration;
    public bool lookForward;
    public SplineWalkerMode mode;
    private bool goingForward = true;
    private float progress;
	
	// Update is called once per frame
	void Update () {
        if (goingForward)
        {
            progress += Time.deltaTime / duration;
            if (progress > 1f)
            {
                if (mode == SplineWalkerMode.once)
                {
                    progress = 1f;
                }
                else if (mode == SplineWalkerMode.loop)
                {
                    progress -= 1f;
                }
                else
                {
                    progress = 2f - progress;
                    goingForward = false;
                }
            }
        }
        else
        {
            progress -= Time.deltaTime / duration;
            if (progress < 0f)
            {
                progress = -progress;
                goingForward = true;
            }
        }
        Vector3 position = spline.GetPoint(progress);
        transform.localPosition = position;
        if (lookForward)
        {
            transform.LookAt(position + spline.GetDirection(progress));
        }
	}
}
