using UnityEngine;

public class SelectorController : MonoBehaviour
{

    public void BackFeedback ()
    {
        SendMessage ("OnEscapeHit", SendMessageOptions.DontRequireReceiver);
    }

    public void LeftFeedback ()
    {
        SendMessage ("OnLeftHit", SendMessageOptions.DontRequireReceiver);
    }

    public void RightFeedback ()
    {
        SendMessage ("OnRightHit", SendMessageOptions.DontRequireReceiver);
    }

    // Update is called once per frame
    void Update ()
    {
        if (!Input.anyKeyDown)
            return;
        if (Input.GetKeyDown (KeyCode.Escape))
            SendMessage ("OnEscapeHit", SendMessageOptions.DontRequireReceiver);
        else if (Input.GetKeyDown (KeyCode.PageDown))
            SendMessage ("OnLeftHit", SendMessageOptions.DontRequireReceiver);
        else if (Input.GetKeyDown (KeyCode.PageUp))
            SendMessage ("OnRightHit", SendMessageOptions.DontRequireReceiver);
        else if (Input.GetKeyDown (KeyCode.Alpha0))
            SendMessage ("OnNumberHit", 0, SendMessageOptions.DontRequireReceiver);
        else if (Input.GetKeyDown (KeyCode.Alpha1))
            SendMessage ("OnNumberHit", 1, SendMessageOptions.DontRequireReceiver);
        else if (Input.GetKeyDown (KeyCode.Alpha2))
            SendMessage ("OnNumberHit", 2, SendMessageOptions.DontRequireReceiver);
        else if (Input.GetKeyDown (KeyCode.Alpha3))
            SendMessage ("OnNumberHit", 3, SendMessageOptions.DontRequireReceiver);
        else if (Input.GetKeyDown (KeyCode.Alpha4))
            SendMessage ("OnNumberHit", 4, SendMessageOptions.DontRequireReceiver);
        else if (Input.GetKeyDown (KeyCode.Alpha5))
            SendMessage ("OnNumberHit", 5, SendMessageOptions.DontRequireReceiver);
        else if (Input.GetKeyDown (KeyCode.Alpha6))
            SendMessage ("OnNumberHit", 6, SendMessageOptions.DontRequireReceiver);
        else if (Input.GetKeyDown (KeyCode.Alpha7))
            SendMessage ("OnNumberHit", 7, SendMessageOptions.DontRequireReceiver);
        else if (Input.GetKeyDown (KeyCode.Alpha8))
            SendMessage ("OnNumberHit", 8, SendMessageOptions.DontRequireReceiver);
        else if (Input.GetKeyDown (KeyCode.Alpha9))
            SendMessage ("OnNumberHit", 9, SendMessageOptions.DontRequireReceiver);
    }

    public static float GetAxis (KeyCode plus, KeyCode minus)
    {
        if (Input.GetKey (plus))
            return Time.deltaTime;
        else if (Input.GetKey (minus))
            return -Time.deltaTime;
        else
            return 0f;
    }


    public static int GetAxisDown (KeyCode plus, KeyCode minus)
    {
        if (Input.GetKeyDown (plus))
            return 1;
        else if (Input.GetKeyDown (minus))
            return -1;
        else
            return 0;
    }

    public static float GetAxis (string axis)
    {
        return Input.GetAxis(axis);
    }

}

