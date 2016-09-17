using UnityEngine;
using System.Collections;

public interface InputManager {

    bool CameraOnDefaultPlace();

    bool ModelToPieces();

    bool FocusOnSelectedPart();

    bool RotateSelectedPart();
    bool DragSelectedPart();
    bool ScaleSelectedPart();

    Vector3 GetTranslationVector();
    Vector3 GetRotationVector();
    Vector3 GetScaleVector();

}
