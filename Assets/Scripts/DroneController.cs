using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneController : MonoBehaviour
{
    Animator anim;

    Vector3 speed = new Vector3(0, 0, 0);

    enum droneState
    {
        drone_Idle, 
        drone_Start_TakingOff,
        drone_TakingOff,
        drone_MovingUp,
        drone_Flying,
        drone_Start_Landing,
        drone_Landing,
        drone_Landed,
        drone_EngineStop,
    }

    droneState state;

    public float movementSpeed = 3.0f;

    public bool IsIdle()
    {
        return (state == droneState.drone_Idle);
    }

    public void TakeOff()
    {
        state = droneState.drone_Start_TakingOff;
    }
    public bool IsFlying()
    {
        return (state == droneState.drone_Flying);
    }
    public void Land()
    {
        state = droneState.drone_Start_Landing;
    }
    private void Start()
    {
        anim = GetComponent<Animator>();

        state = droneState.drone_Idle;

        //anim.SetBool("TakeOff", true);
    }

    public void Move(float speed_X, float speed_Z)
    {
        speed.x = speed_X;
        speed.z = speed_Z;
        UpdateDrone();
    }

    void UpdateDrone()//controlla la velocidad y el movimiento del dron.
    {
        switch (state)
        {
            case droneState.drone_Idle:
                break;
            case droneState.drone_Start_TakingOff:
                anim.SetBool("TakeOff", true);
                state = droneState.drone_TakingOff;
                break;
            case droneState.drone_TakingOff:
                if (anim.GetBool("TakeOff") == false)
                {
                    state = droneState.drone_MovingUp;
                }
                break;
            case droneState.drone_MovingUp:
                if (anim.GetBool("MoveUp") == false)
                {
                    state = droneState.drone_Flying;                    
                }
                break;
            case droneState.drone_Flying:
                float angle_Z = -30f * speed.x * 60 * Time.deltaTime;
                float angle_X = 30f * speed.z * 60 * Time.deltaTime;

                Vector3 rotation = transform.localRotation.eulerAngles;

                transform.localPosition += speed * movementSpeed * Time.deltaTime;
                transform.localRotation = Quaternion.Euler(angle_X, rotation.y, angle_Z);
                break;
            case droneState.drone_Start_Landing:
                anim.SetBool("MoveDown", true);
                state = droneState.drone_Landing;
                break;
            case droneState.drone_Landing:
                if (anim.GetBool("MoveDown") == false)
                {
                    state = droneState.drone_Landed;
                }
                break;
            case droneState.drone_Landed:
                anim.SetBool("Land", true);
                state = droneState.drone_EngineStop;
                break;
            case droneState.drone_EngineStop:
                if (anim.GetBool("Land") == false)
                {
                    state = droneState.drone_Idle;
                }
                break;

        }
        
    }
}
