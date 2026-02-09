using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;

public class carController : MonoBehaviour
{
    public Rigidbody rb;
    public CarDna thisCarDna;
    public float turnSpeed = 120f;
    public float speed = 10f;
    public float fitness;
    public bool crashed = false;

    public void FixedUpdate()
    {
        float[] inputs = Sense();
        float[] outputs = Think(inputs);
        drive(outputs[1], outputs[0]); // throttle, steer
        fitness = transform.position.z + speed*outputs[1];
        if (crashed)
        {
            fitness = fitness/2;
        }
    }
    public void drive(float throttle, float steer)
    {
        // Handles car forward movement
        Vector3 forwardMovement = transform.forward * throttle * Time.fixedDeltaTime * speed;
        rb.MovePosition(rb.position + forwardMovement);
        // Handles the cars rotation
        Quaternion turnRotation = Quaternion.Euler(0f, steer * turnSpeed * Time.fixedDeltaTime, 0f);
        rb.MoveRotation(rb.rotation * turnRotation);
    }
    public float[] Think(float[] inputs)
    {
        float steer = 0;
        float throttle = 0;
        float[] carsDna = thisCarDna.getGenome();

        for (int i = 0; i < inputs.Length; i++)
        {
            steer += inputs[i] * carsDna[i];
            throttle += inputs[i] * carsDna[i + inputs.Length];
        }

        float steerOutput = (float)Math.Tanh(steer);   // steer is float
        float throttleOutput = (float)Math.Tanh(throttle);
        return new float[] { steerOutput, throttleOutput };
    }
    public float[] Sense()
    {
        float[] inputs = new float[5]; // 5 rays
        float[] angles = { -45f, -20f, 0f, 20f, 45f };

        for (int i = 0; i < angles.Length; i++)
        {
            // Rotate forward vector by angle
            Vector3 dir = Quaternion.Euler(0, angles[i], 0) * transform.forward;

            // Cast the ray
            if (Physics.Raycast(transform.position, dir, out RaycastHit hit, 15f))
            {
                inputs[i] = 1f - (hit.distance / 15f); // normalize 0 to 1
            }
            else
            {
                inputs[i] = 1f; // nothing hit, max distance
            }
            Debug.DrawRay(transform.position, dir * 15f, Color.yellow); // visualize rays
        }

        return inputs;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            crashed = true;
            Debug.Log("Hit a wall!");
        }
    }

}
