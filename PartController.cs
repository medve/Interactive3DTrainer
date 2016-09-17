using UnityEngine;
using System.Collections;

public interface PartController {

    void AddPart (Part part);
    void DeletePart (Part part);


    // void FocusOn (Part part);
    // void DisableFocus ();


    void SetCameraOn (Part part);


    void ShowInfo (string text);
    void ShowTip (string text);
    void HideInfo ();
    void HideTip ();


    //void ModelToPieces ();
    //void PutDetailsOnDefaultPlaces ();


    // void SelectPart (Part part);
    // void CancelSelection ();
    // void ExpandSelection ();

}
