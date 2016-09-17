using UnityEngine;
using System.Collections;

public class MouseInputManager : MonoBehaviour, InputManager 
{

    public bool CameraOnDefaultPlace()
    {
        return Input.GetKeyDown(KeyCode.Escape);
    }

    public bool ModelToPieces()
    {
        return Input.GetKeyDown(KeyCode.Space);
    }

    public bool FocusOnSelectedPart()
    {
        return Input.GetMouseButtonDown (2);
    }

    public bool RotateSelectedPart()
    {
        return Input.GetMouseButton (0);
    }

    public bool DragSelectedPart()
    {
        return Input.GetMouseButton (1);
    }

    public bool ScaleSelectedPart()
    {
        return Input.GetKey(KeyCode.F);
    }

    public Vector3 GetTranslationVector()
    {
        return new Vector3(Input.GetAxis("Vertical"),
                           Input.GetAxis("Horizontal"),0.0f);
    }

	public Vector3 GetRotationVector()
    {
        return new Vector3(Input.GetAxis("Mouse Y"),
                           Input.GetAxis("Mouse X"), 0.0f);
    }

    public Vector3 GetScaleVector()
    {
        return new Vector3(Input.GetAxis("Vertical"),
                           Input.GetAxis("Horizontal"),0.0f);
    }
}
