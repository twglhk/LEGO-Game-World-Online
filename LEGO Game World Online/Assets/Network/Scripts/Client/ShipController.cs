using LEGOModelImporter;
using System.Collections;
using System.Collections.Generic;
using Unity.LEGO.Behaviours;
using Unity.LEGO.Game;
using UnityEngine;



namespace Unity.LEGO.Behaviours
{
    public class ShipController : MonoBehaviour
    {
        public Brick shipBrick;
        public float velocity;
        public float maxVel;
        private float rotateSpeed = 16.0f;

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKey(KeyCode.W))
                velocity += 0.1f;

            if (Input.GetKey(KeyCode.S))
                velocity -= 0.1f;

            velocity -= 0.025f;

            velocity = Mathf.Clamp(velocity, 0f, maxVel);
            transform.position += transform.forward * velocity * Time.deltaTime;

            Rotate3D();
        }

        void Rotate3D()
        {
            float h = Input.GetAxis("Horizontal");
            transform.Rotate(0f, h * rotateSpeed * Time.deltaTime, 0f);
        }
    }
}