using UnityEngine;
using System.Collections;

public interface CameraController {

    void SetCameraPosition (Vector3 position);
    Vector3 GetCameraPosition ();
    void FlyToPosition (Vector3 position);

    void PutCameraOnDefaultPlace();
    void PutCameraOnPreviousPlace();

    void Rotate (Vector3 rotation);

    void BlockCamera();
    void UnBlockCamera();

    void LookAt(Vector3 pos);
}
