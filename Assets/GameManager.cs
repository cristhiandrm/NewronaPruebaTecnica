using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class GameManager : MonoBehaviour
{
    public DroneController droneController;

    public Button startEngine;
    public Button stopEngine;

    public Joystick joystick;

    public ARRaycastManager raycastManager;
    public ARPlaneManager planeManager;
    List<ARRaycastHit> hit = new List<ARRaycastHit>();

    public GameObject drone;
    public Camera camera;
    
    private void Update()
    {
        //float speed_X = Input.GetAxis("Horizontal");
        //float speed_Z = Input.GetAxis("Vertical");
        float speed_X = joystick.Horizontal;
        float speed_Z = joystick.Vertical;

        droneController.Move(speed_X, speed_Z);

        if (droneController.IsIdle())
        {
            updateAR();
        }
    }

    void updateAR()
    {
        Vector2 positionOnScreenSpace = camera.ViewportToScreenPoint(new Vector2(0.5f,0.5f));
        raycastManager.Raycast(positionOnScreenSpace, hit, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinBounds);

        if (hit.Count > 0)
        {
            if (planeManager.GetPlane(hit[0].trackableId).alignment==UnityEngine.XR.ARSubsystems.PlaneAlignment.HorizontalUp)
            {
                Pose pose = hit[0].pose;
                drone.transform.position = pose.position;
                drone.SetActive(true);
                drone.SetActive(true);
            }
        }
    }

    public void StartButton()
    {
        if (droneController.IsIdle())
        {
            droneController.TakeOff();
            startEngine.gameObject.SetActive(false);
            stopEngine.gameObject.SetActive(true);
        }
    }

    public void StopButton()
    {
        if (droneController.IsFlying())
        {
            droneController.Land();
            stopEngine.gameObject.SetActive(false);
            startEngine.gameObject.SetActive(true);
        }
    }

    
}
