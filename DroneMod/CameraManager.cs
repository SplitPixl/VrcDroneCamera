﻿using System;
using System.Collections;
using UnityEngine;
using WebSocketSharp.Server;

namespace DroneMod
{
    class CameraManager : MonoBehaviour
    {
        Vector3 inputTranslation;
        Vector3 inputRotation;
        public float translateSpeed = 2;
        public float rotateSpeed = 90;
        public float translateSmooth = 10;
        public float rotateSmooth = 10;
        GameObject userCam;
        UserCameraController camController;
        Transform followTarget;
        public DroneMode mode = DroneMode.DISABLED;

        public void Start()
        {
            StartCoroutine(GetCam());
        }

        private IEnumerator GetCam()
        {
            yield return new WaitForSeconds(2);
            Console.WriteLine("GetCam()");
            Console.WriteLine("Trying to find cam...");

            userCam = GameObject.Find("TrackingVolume/UserCamera");
            camController = userCam.GetComponent<UserCameraController>();

            Console.WriteLine("Camera found!");

        }

        public void Update()
        {
            switch (mode)
            {
                case DroneMode.DISABLED:
                    break;
                case DroneMode.FLY:
                    transform.Translate(inputTranslation * translateSpeed * Time.deltaTime);
                    transform.eulerAngles += inputRotation * (rotateSpeed * Time.deltaTime);
                    break;
                case DroneMode.FOLLOW:
                    if (followTarget == null)
                    {
                        SetMode(DroneMode.DISABLED);
                    }
                    else
                    {
                        transform.LookAt(followTarget);
                    }
                    break;
            }

            if (mode != DroneMode.DISABLED)
            {
                camController.cameraMount.position = Vector3.Lerp(camController.cameraMount.position, transform.position, 1 / translateSmooth);
                camController.cameraMount.rotation = Quaternion.Lerp(camController.cameraMount.rotation, transform.rotation, 1 / rotateSmooth);
            }

        }

        public void SetMode(DroneMode newMode)
        {
            mode = newMode;
            if (mode == DroneMode.DISABLED)
            {
                Reset();
            }
        }


        public void SetInput(Vector3 translate, Vector3 rotate)
        {
            inputTranslation = translate;
            inputRotation = rotate;
            Console.Write(string.Format("{0}\r", mode.ToString()));
        }

        public void SetFollow(Transform target)
        {
            throw new NotImplementedException();
            //followTarget = target;
            //mode = DroneMode.FOLLOW;
        }

        public void TeleportPos(Vector3 translate, Vector3 rotate)
        {
            transform.position = translate;
            transform.eulerAngles = rotate;
            Console.WriteLine("Teleported Camera!");
        }

        public void Reset()
        {
            Transform viewFinder = camController.viewFinder.transform;
             
            transform.position = new Vector3(viewFinder.position.x, viewFinder.position.y, viewFinder.position.z);
            transform.eulerAngles = new Vector3(viewFinder.eulerAngles.x - 90, viewFinder.eulerAngles.y + 180, viewFinder.eulerAngles.z);

            camController.cameraMount.position = new Vector3(viewFinder.position.x, viewFinder.position.y, viewFinder.position.z);
            camController.cameraMount.eulerAngles = new Vector3(viewFinder.eulerAngles.x + 90, viewFinder.eulerAngles.y, viewFinder.eulerAngles.z);
            Console.WriteLine("Reset Camera!");
        }

        public void Shutter()
        {
            camController.onUseDown(0);
        }

        public enum DroneMode
        {
            DISABLED = 0,
            FLY = 1,
            FOLLOW = 2,
        }
    }
}