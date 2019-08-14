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
        Transform camWeirdnessFixer = new GameObject("camWeirdnessFixer").transform;
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
                camController.cameraMount.position = Vector3.Lerp(camController.cameraMount.position, transform.position, 1 / translateSmooth);
                camController.cameraMount.rotation = Quaternion.Lerp(camController.cameraMount.rotation, transform.rotation, 1 / rotateSmooth);

                //camWeirdnessFixer.position = Vector3.Lerp(camWeirdnessFixer.position, transform.position, 1 / translateSmooth);
                //camWeirdnessFixer.rotation = Quaternion.Lerp(camWeirdnessFixer.rotation, transform.rotation, 1 / rotateSmooth);

                //camController.cameraMount.position = new Vector3(camWeirdnessFixer.position.x, camWeirdnessFixer.position.y, camWeirdnessFixer.position.z);
                //camController.cameraMount.rotation = new Quaternion(camWeirdnessFixer.rotation.x, camWeirdnessFixer.rotation.y, camWeirdnessFixer.rotation.z, camWeirdnessFixer.rotation.w);
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
            camController.cameraMount.localPosition = new Vector3(0, 0, 0);
            camController.cameraMount.localEulerAngles = new Vector3(-90, 0, 0);

            transform.position = new Vector3(camController.cameraMount.position.x, camController.cameraMount.position.y, camController.cameraMount.position.z);
            transform.eulerAngles = new Vector3(camController.cameraMount.eulerAngles.x, camController.cameraMount.eulerAngles.y, camController.cameraMount.eulerAngles.z);

            camWeirdnessFixer.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            camWeirdnessFixer.rotation = new Quaternion(transform.rotation.x, transform.rotation.y, transform.rotation.z, transform.rotation.w);

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
