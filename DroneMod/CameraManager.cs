using DebuggerMod.Serial;
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
            while (userCam == null && camController == null)
            {
                yield return new WaitForSeconds(0.5f);
                Console.WriteLine("Trying to find cam...");
                userCam = GameObject.Find("TrackingVolume/UserCamera");
                if (userCam != null)
                {
                    camController = userCam.GetComponent<UserCameraController>();
                }
            }
            Console.WriteLine("Camera found!");
        }

        public void OnConnect()
        {
            transform.position = camController.viewFinder.transform.position;
            transform.eulerAngles = new Vector3(camController.viewFinder.transform.eulerAngles.x, camController.viewFinder.transform.eulerAngles.y + 180, camController.viewFinder.transform.eulerAngles.z);
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
                //camController.photoCamera.transform.position = Vector3.Lerp(camController.photoCamera.transform.position, transform.position, 1 / translateSmooth);
                //camController.photoCamera.transform.rotation = Quaternion.Lerp(camController.photoCamera.transform.rotation, transform.rotation, 1 / rotateSmooth);
                //camController.videoCamera.transform.position = Vector3.Lerp(camController.videoCamera.transform.position, transform.position, 1 / translateSmooth);
                //camController.videoCamera.transform.rotation = Quaternion.Lerp(camController.videoCamera.transform.rotation, transform.rotation, 1 / rotateSmooth);
            }

            if (Input.GetKey(KeyCode.O))
            {
                try
                {
                    Console.WriteLine(new GameObjectSerial(camController.userCameraIndicator.gameObject).ToString());
                } catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }

            if (Input.GetKey(KeyCode.I))
            {
                camController.userCameraIndicator.enabled = !camController.userCameraIndicator.enabled;
            }

        }

        public void SetMode(DroneMode newMode)
        {
            if (newMode == DroneMode.DISABLED)
            {
                Reset();
            }
            else if (mode == DroneMode.DISABLED)
            {
                transform.position = camController.viewFinder.transform.position;
                transform.eulerAngles = new Vector3(camController.viewFinder.transform.eulerAngles.x, camController.viewFinder.transform.eulerAngles.y + 180, camController.viewFinder.transform.eulerAngles.z);
            }
            mode = newMode;
        }


        public void SetInput(Vector3 translate, Vector3 rotate)
        {
            inputTranslation = translate;
            inputRotation = rotate;
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
            if (mode == DroneMode.DISABLED)
            {
                camController.cameraMount.position = translate;
                camController.cameraMount.eulerAngles = rotate;
            }
            Console.WriteLine("Teleported Camera!");
            Console.WriteLine(string.Format("{0} {1}", translate, rotate));
        }

        public void Reset()
        {
            //camController.photoCamera.transform.localPosition = new Vector3(0, 1, 0);
            //camController.photoCamera.transform.localEulerAngles = new Vector3(90, 0, 0);
            //camController.videoCamera.transform.localPosition = new Vector3(0, 1, 0);
            //camController.videoCamera.transform.localEulerAngles = new Vector3(90, 0, 0);

            camController.cameraMount.localPosition = new Vector3(0, 0, 0);
            camController.cameraMount.localEulerAngles = new Vector3(90, 0, 180);

            transform.position = new Vector3(camController.viewFinder.transform.position.x, camController.viewFinder.transform.position.y, camController.viewFinder.transform.position.z);
            transform.eulerAngles = new Vector3(camController.viewFinder.transform.eulerAngles.x, camController.viewFinder.transform.eulerAngles.y + 180, camController.viewFinder.transform.eulerAngles.z);

            setOrthographic(false);
            SetFOV(60);
            Console.WriteLine("Reset Camera!");
        }

        public void SetFOV(float value)
        {
            foreach (Camera cam in userCam.GetComponentsInChildren<Camera>())
            {
                if (cam.orthographic)
                {
                    cam.orthographicSize = value;
                } else
                {
                    cam.fieldOfView = value;
                }
            }
        }

        public void setOrthographic(bool orthograph)
        {
            foreach (Camera cam in userCam.GetComponentsInChildren<Camera>())
            {
                cam.orthographic = orthograph;
            }
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
