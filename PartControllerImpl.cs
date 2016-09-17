using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PartControllerImpl : MonoBehaviour, PartController
{
    public GameObject TipTextGameObject = null;
    public GameObject InfoTextGameObject = null;
    public GameObject CameraControllerGameObject = null;
    private Part[] _parts = null;
    private TextController _tipText = null;   //текст, который появляется при выделении детали
    private TextController _infoText = null;  //текст который появляется при фокусе на детали
    private CameraController _cameraController = null;
    private Part _selectedPart = null;        //деталь на которую смотрит пользователь
    private Part _focusedPart = null;         //деталь на которую наведена камера и информация о которой показывается 
    private Part _manipulateWithPart = null;  //деталь которую двигает или вращает пользователь
    private bool _parted = false;             //находится ли модель в разобранном состоянии
    private InputManager _inputManager = null;
    private MovingLocker _movingLock;

    private enum FocusType
    {
        ROTATE,
        DRAG,
        SCALE,
    }

    Part[] FindParts ()
    {
        
        GameObject[] partsGameObjects = GameObject
                                         .FindGameObjectsWithTag ("Part");
        Part[] parts = new Part[partsGameObjects.Length];
        for (int partObjectNum = 0; partObjectNum < partsGameObjects.Length; 
                                                            partObjectNum++) {
            Part thisObjectPart = partsGameObjects [partObjectNum]
                                    .GetComponent<Part> ();

            if (thisObjectPart == null) {
                throw new NullReferenceException(
                                partsGameObjects [partObjectNum].ToString());
            }

            parts [partObjectNum] = thisObjectPart;
        }

        return parts;
    }

    void Start ()
    {
        
        _tipText = TipTextGameObject.GetComponent<TextController> ();
        _infoText = InfoTextGameObject.GetComponent<TextController> ();
        _cameraController = CameraControllerGameObject
                             .GetComponent<CameraController> ();

        _inputManager = GameObject.FindGameObjectWithTag ("InputManager")
                            .GetComponent<InputManager>();
        _movingLock = GameObject.FindGameObjectWithTag ("MovingLocker")
                       .GetComponent<MovingLocker>();
        _parts = FindParts ();
    }

    void FixedUpdate ()
	{ 
        //Ищем деталь на которую наведена камера
        //Отменяем выделение, если ее нет
        Part targetPart = GetTartgetPart();
        if (targetPart == null) {
            CancelSelection ();
        }

        //обрабатываем события ввода
        HandleFocus(targetPart);
        HandleTransform(_focusedPart);
        HandleParting();

        //необходимо отключать фокус при возвращении камеры домой
        if(_inputManager.CameraOnDefaultPlace()) {
            DisableFocus();
            DisableManipulations ();
        }

        //выделить деталь, на которую обращено внимание
        if (targetPart != null && _focusedPart == null) {
            SelectPart (targetPart);
        }   
        
    }

    private void HandleTransform(Part targetPart)
    {   
        //если не нужно вращать и перемещать - отключаем
        //вращения и перемещения
        if(!_inputManager.RotateSelectedPart() &&
            !_inputManager.DragSelectedPart() && 
            _manipulateWithPart != null) {
            DisableManipulations();
        }

        //только если пользователь выбрал деталь
        if(targetPart == null) {
            return ;
        }

        //обрабатываем вращение
        if(_inputManager.RotateSelectedPart()) {
            ManipulatePart (targetPart, FocusType.ROTATE);
        }

        //обрабатываем перетаскивание
        if (_inputManager.DragSelectedPart()) {
            ManipulatePart (targetPart, FocusType.DRAG);
        }

        //обрабатываем масштабирование
        if (_inputManager.ScaleSelectedPart()) {
            ManipulatePart (targetPart, FocusType.SCALE);
        }
    }

    private void HandleFocus(Part targetPart)
    {
        //если сигнала выделять не получено - выходим
        if(!_inputManager.FocusOnSelectedPart()) {
            return ;
        }

        if(_focusedPart == null) {
            //если пользователь выбрал деталь - выбираем
            if (targetPart != null) {
                FocusOn(targetPart);
            } 
        } else {
            DisableFocus();
        }
    }

    private void HandleParting()
    {
        //если не поступало сигнала на разделение - выходим
        if(!_inputManager.ModelToPieces()) {
            return ;
        }

        if(_parted) {
            PutDetailsOnDefaultPlaces();
            _parted = false;
        } else {
            ModelToPieces();
            _parted = true;
        }
    }


    public void AddPart (Part part)
    {
        
        //  _parts.Add (part);  

    }

    public void DeletePart (Part part)
    {

        //  _parts.RemoveAt (id);

    }

    private Part GetTartgetPart() {
        Ray ray = Camera.main.ScreenPointToRay (
                      new Vector3 (Camera.main.pixelWidth / 2.0f,
                          Camera.main.pixelHeight / 2.0f, 0.0f));
        RaycastHit hit;
        Part targetPart = null;
        if (Physics.Raycast (ray, out hit, 100)){
            targetPart = hit.collider.gameObject.GetComponent<Part> ();
        }
        return targetPart;
    }


    private void ManipulatePart (Part part, FocusType ft)
    {
        //если деталь уже захвачена или пустая - выходим
        if(_manipulateWithPart == part || part == null) {
            return ;
        }

        //меняем деталь, с которой будем работать
        DisableManipulations ();
        _manipulateWithPart = part;
        
        //отключаем управление камеры вводом
        BlockCamera ();

        //включаем необходимый режим взаимодействия
        switch(ft) {
            case (FocusType.ROTATE):
                part.RotationModeOn (true);
                break;
            case (FocusType.DRAG):
                part.DragModeOn (true);
                break;
            case (FocusType.SCALE):
                part.ScaleModeOn (true);
                break;
        }
    }

    private void DisableManipulations ()
    {       
        if (_manipulateWithPart == null) {
            return ;
        }

        _manipulateWithPart.RotationModeOn (false);
        _manipulateWithPart.DragModeOn (false);
        _manipulateWithPart.ScaleModeOn (false);
        _manipulateWithPart = null;
        UnBlockCamera ();
    }



////////////////////отделить??
    public void ShowInfo (string text)
    {

        _infoText.SetText (text);
        _infoText.ShowText ();
    }

    public void ShowTip (string text)
    {
        
        _tipText.SetText (text);
        _tipText.ShowText ();
    }

    public void HideInfo ()
    {
        if (_infoText != null) {
            _infoText.HideText ();
        }
    }

    public void HideTip ()
    {
        if (_tipText != null) {     
            _tipText.HideText ();
        }
    }
////////////////////отделить??

/////////////////////////////////////////////////
    private void ModelToPieces ()
    {
        if(!_movingLock.LockUp(_parts.Length)) {
            return ;
        }

        Part part;
        for (int partNum = 0; partNum < _parts.Length; partNum++) {
            part = (Part)_parts [partNum];
            part.FlyToFront ();
        }
    }

    private void PutDetailsOnDefaultPlaces ()
    {
        if(!_movingLock.LockUp(_parts.Length)) {
            return ;
        }

        DisableFocus();
        Part part;
        for (int partNum = 0; partNum < _parts.Length; partNum++) {
            part = (Part)_parts [partNum];
            part.FlyBack ();
        }
    }

/////////////////////////////////////////////////

//чинить блокировку
///////////////////////////////////////////

    private void FocusOn (Part part)
    {
        // если отделена деталь
        // нужно ли хранить список отделенных деталей
        // нужно ли проверять??
        if (!_parted) {
            if(!_movingLock.LockUp()) {
                return ;
            }
            part.FlyToFront ();
        }

        _focusedPart = part;
        //SetCameraOn (_focusedPart);
        ShowInfo (part.GetInfoText ());
        // SetCameraOn (part);
    }

    private void DisableFocus ()
    {
        if (_focusedPart == null) {
            return;
        }

        HideInfo();
        //камера на пред место??
        _focusedPart = null;
        
    }

    private void SelectPart (Part part)
    {

        if (_selectedPart != part) {
            CancelSelection ();
            _selectedPart = part;
            _selectedPart.Highlight ();
            ShowTip (part.GetTipText ());
        }   
    }

    private void CancelSelection ()
    {
        if (_selectedPart != null) {
            _selectedPart.CancelHighlight ();
            HideTip ();
            _selectedPart = null;
        }
    }

    private void ExpandSelection ()
    {

        if (_selectedPart != null) {
            Part parrentPart = _selectedPart.GetParrent ();
            SelectPart (parrentPart);
        } 
    }


    public void SetCameraOn (Part part)
    {
        //TODO:рассчитать расстояние так чтобы деталь была на весь экран
        // _cameraController.LookAt(part.GetPosition());
        // Vector3 flyingPosition = (Vector3) part.GetPosition ();
        // _cameraController.FlyToPosition (flyingPosition 
        //     - new Vector3(0.0f,0.0f,5.0f));
        //TODO:запомнить предыдущее положение
    }

    private void BlockCamera ()
    {

        _cameraController.BlockCamera ();
    }

    private void UnBlockCamera ()
    {
        
        _cameraController.UnBlockCamera ();
    }
}
