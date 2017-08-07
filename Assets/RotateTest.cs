using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTest : MonoBehaviour {

    public Transform target;
    public float speed;

    void Start()
    {
        //Vector3 targetDir = target.position - transform.position;
        //float step = speed * Time.deltaTime;
        //Vector3 newDir = Vector3.RotateTowards(transform.up, targetDir, step, 0.0F);
        //Debug.DrawRay(transform.position, newDir, Color.red);
        //transform.rotation = Quaternion.LookRotation(newDir);
        //Debug.Log("Target Direction:  " + targetDir);

        //float angle = Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg; //- 90 becuase the artwork is facing down
        //Debug.Log("Angle:  " + angle);
        //Debug.Log("Initial rotation angles:  " + transform.rotation.eulerAngles);
        //Debug.Log("forward Vector:  " + Vector3.forward);

        //if (angle < 0)
        //{
        //    angle = -(90 + angle) * 2;
        //}else
        //{
        //    angle = (90 - angle) * 2;
        //}
        //transform.Rotate(0f, 0f, angle);
        //Debug.Log("Target Rotation:  " + rotation.eulerAngles);
        //transform.rotation = rotation;

        //Debug.Log(transform.up.ToString());
        //transform.Rotate(Vector3.forward * 45f);
        //Debug.Log(transform.rotation.eulerAngles);
        //Debug.Log(transform.up.ToString());

        //Debug.Log(Mathf.Atan2(0.5f, 0.5f) * Mathf.Rad2Deg);
        //Debug.Log(Mathf.Atan2(-0.5f, 0.5f) * Mathf.Rad2Deg);
        //Debug.Log(Mathf.Atan2(-0.5f, -0.5f) * Mathf.Rad2Deg);
        //Debug.Log(Mathf.Atan2(0.5f, -0.5f) * Mathf.Rad2Deg);
        //Debug.Log(Mathf.Atan2(0f, 1f) * Mathf.Rad2Deg);
    }

    private void Update()
    {
        //Vector3 relativePos = target.position - transform.position;
        //Quaternion rotation = Quaternion.LookRotation(relativePos);
        //rotation.x = 0.0f;
        //rotation.y = 0.0f;
        ////rotation.z += 90f * Mathf.Deg2Rad;
        //transform.rotation = rotation;
        //transform.rotation = Quaternion.Euler(0, 0, 90f);

        Vector2 direction = target.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        angle += 90f;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = rotation;
    }
}
