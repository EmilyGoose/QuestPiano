using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeerMover : MonoBehaviour
{
    public GameObject centerRock;
    private float angularSpeed = 50;
    private float angle = 161;
    private float radius;

    // Start is called before the first frame update
    void Start()
    {
        radius = (gameObject.transform.position - centerRock.transform.position).magnitude;
    }

    // Update is called once per frame
    void Update()
    {
        // rider said to do like this idk tbh
        if (Math.Abs(Math.Floor(angle) - 160) > 0.5F)
        {
            angle += Time.deltaTime * angularSpeed; // update angle
            // Reset angle
            if (angle > 360)
            {
                angle -= 360;
            }

            // direction
            Vector3 direction = Quaternion.AngleAxis(-angle, Vector3.up) * Vector3.back;

            // let's rotate
            transform.rotation = Quaternion.LookRotation(direction.normalized) * Quaternion.Euler(0, -90, 0);

            // code quality out window 4am btw
            Vector3 idk = centerRock.transform.position + new Vector3(0, -0.7F, 0);

            // update position based on center, the direction, and the radius (which is a constant)
            transform.position = idk + direction * radius; 
        }
    }

    public void DoALap()
    {
        // spaget (dont touch)
        if (Math.Abs(Math.Floor(angle) - 160) <= 0.5F)
        {
            angle += 1;
        }
    }
}