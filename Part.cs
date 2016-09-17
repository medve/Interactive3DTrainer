using UnityEngine;
using System.Collections;

public interface Part {
    
    void FlyToPosition (Vector3 position);
    void FlyToFront ();
    void FlyBack ();
    void ReturnToStartPosition ();

    void RotateToDelta (Vector3 rotation);
    void ScaleToDelta (Vector3 rotation); 
    void Move (Vector3 position);


    void Highlight ();
    void CancelHighlight ();


    Part GetParrent ();


    void SetTipText (string inputText);
    string GetTipText ();
    void SetInfoText (string inputText);
    string GetInfoText ();


    Vector3 GetPosition();
    void SetPosition (Vector3 position);

	void RotationModeOn (bool mode);
	void DragModeOn(bool mode);
    void ScaleModeOn(bool mode);

}
