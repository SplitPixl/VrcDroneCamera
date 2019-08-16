using System;
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
            Console.WriteLine("GetCam()");
            while (!userCam)
            {
                Console.WriteLine("Trying to find cam...");
                userCam = GameObject.Find("TrackingVolume/UserCamera");
                camController = userCam.GetComponent<UserCameraController>();
                yield return new WaitForSeconds(0.5f);
            }
            Console.WriteLine("Camera found!");
        }

        public void Update()
        {
            switch (mode)
            {
                case DroneMode.DISABLED:
                    break;
                case DroneMode.FLYMODEGMOD:
                    transform.Translate(inputTranslation * translateSpeed * Time.deltaTime);
                    transform.eulerAngles += inputRotation * (rotateSpeed * Time.deltaTime);
                    break;
                case DroneMode.FLYMODEMINECRAFT:
                    Vector3 oldAngles = transform.eulerAngles;
                    transform.eulerAngles = new Vector3(0, oldAngles.y, 0);
                    transform.Translate(inputTranslation * translateSpeed * Time.deltaTime);
                    transform.eulerAngles = oldAngles + (inputRotation * (rotateSpeed * Time.deltaTime));
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
                camController.photoCamera.transform.position = Vector3.Lerp(camController.photoCamera.transform.position, transform.position, 1 / translateSmooth);
                camController.photoCamera.transform.rotation = Quaternion.Lerp(camController.photoCamera.transform.rotation, transform.rotation, 1 / rotateSmooth);
                camController.videoCamera.transform.position = Vector3.Lerp(camController.videoCamera.transform.position, transform.position, 1 / translateSmooth);
                camController.videoCamera.transform.rotation = Quaternion.Lerp(camController.videoCamera.transform.rotation, transform.rotation, 1 / rotateSmooth);
            }

        }

        public void SetMode(DroneMode newMode)
        {
            mode = newMode;
            if (mode == DroneMode.DISABLED)
            {
                Reset();
            }
            else
            {
                transform.position = camController.viewFinder.transform.position;

            }
        }


        public void SetInput(Vector3 translate, Vector3 rotate)
        {
            inputTranslation = translate;
            inputRotation = rotate;
            Console.Write(string.Format("{0}     \r", mode.ToString()));
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
            camController.photoCamera.transform.localPosition = new Vector3(0, 1, 0);
            camController.photoCamera.transform.localEulerAngles = new Vector3(90, 0, 0);
            camController.videoCamera.transform.localPosition = new Vector3(0, 1, 0);
            camController.videoCamera.transform.localEulerAngles = new Vector3(90, 0, 0);

            transform.position = new Vector3(camController.cameraMount.position.x, camController.cameraMount.position.y, camController.cameraMount.position.z);
            transform.eulerAngles = new Vector3(camController.cameraMount.eulerAngles.x, camController.cameraMount.eulerAngles.y, camController.cameraMount.eulerAngles.z);

            Console.WriteLine("Reset Camera!");
        }

        public void Shutter()
        {
            camController.onUseDown(0);
        }

        public enum DroneMode
        {
            DISABLED = 0,
            FLYMODEMINECRAFT = 1,
            FLYMODEGMOD = 2,
            FOLLOW = 3
        }
    }
}
