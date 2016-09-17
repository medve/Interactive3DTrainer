using UnityEngine;
using System.Collections;

public class KinectInputManager : MonoBehaviour, InputManager 
{
	private KinectFigures _kinectFigures;

	private void Start()
	{
		_kinectFigures = GetComponent<KinectFigures>(); 
	}

    public bool CameraOnDefaultPlace()
    {
        return _kinectFigures.LeftHandDoubleShrink();
    }

    public bool ModelToPieces()
    {
        return _kinectFigures.HandsClap();
    }

    public bool FocusOnSelectedPart()
    {
        return _kinectFigures.RightHandDoubleShrink();
    }

    public bool RotateSelectedPart()
    {
        return _kinectFigures.RightHandDrag();
    }

    public bool DragSelectedPart()
    {
        return _kinectFigures.LeftHandDrag();
    }

    public bool ScaleSelectedPart()
    {
        return _kinectFigures.HandsDragTowards();
    }

    public Vector3 GetTranslationVector()
    {
        return _kinectFigures.LeftHandMoveVector();
    }

	public Vector3 GetRotationVector()
    {
        return _kinectFigures.RightHandMoveVector();
    }

    public Vector3 GetScaleVector()
    {
        return _kinectFigures.TowardMoveVector();
    }
}
