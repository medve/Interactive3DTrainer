using UnityEngine;
using System.Collections;

public class PartImpl : MonoBehaviour, Part
{

    private Vector3 _startPosition = Vector3.zero;
    private Vector3 _startRotation = Vector3.zero;

    //[SerializeField]private Quaternion _rotationInFront = Quaternion.identity;
    [SerializeField]private Vector3 _positionInFront = Vector3.zero;

    [SerializeField] private string tipText = "";
    [SerializeField][TextAreaAttribute] private string infoText = "";
    [SerializeField] private Color _highlightColor;
    [SerializeField] private float _flyTime = 0.5f;
    private Color _defaultColor;
    private Renderer _renderer = null;
    private float xRotAngle = 0.0f;        
    private float yRotAngle = 0.0f;  
    private float zRotAngle = 0.0f;
    [Range(0f, 10f)] [SerializeField] private float turnSpeed = 1.5f;
    [Range(0f, 10f)] [SerializeField] private float moveSpeed = 1.5f;
    [Range(0f, 10f)] [SerializeField] private float scaleSpeed = 0.01f;
    private bool _rotate = false;
    private bool _drag = false;
    private bool _scale = false;
    private bool _is_moving = false;
    private InputManager _inputManager = null;
    private MovingLocker _movingLock;

    // public enum LockResult
    // {
    //     SUCCESS,
    //     FAIL
    // }

    void Start()
    {
        _renderer = GetComponent<Renderer> ();
        _inputManager = GameObject.FindGameObjectWithTag ("InputManager")
                            .GetComponent<InputManager>();

        _defaultColor = _renderer.material.color;
        _startPosition = transform.position;
        _startRotation = transform.eulerAngles;

        xRotAngle = transform.eulerAngles.x;        
        yRotAngle = transform.eulerAngles.y;  
        zRotAngle = transform.eulerAngles.z;
        _movingLock = GameObject.FindGameObjectWithTag ("MovingLocker")
                       .GetComponent<MovingLocker>();
    }

    void FixedUpdate () 
    {
        if(_rotate){
            RotateToDelta(_inputManager.GetRotationVector());
        }

        if(_drag) {
            Move(_inputManager.GetTranslationVector());
        }

        if(_scale) {
            ScaleToDelta(_inputManager.GetScaleVector());
        }
    }

    public void FlyToPosition (Vector3 position)
    {
        StartCoroutine(FlyCoroutine(position));
    }

    private IEnumerator FlyCoroutine(Vector3 position)
    {
        if(LockMoving()) {
            float speed = 1.0f/_flyTime;
            for(float pathPart = 0.0f; pathPart < 1.0f; 
                            pathPart += speed * Time.deltaTime){ 
                pathPart = Mathf.Clamp(pathPart, 0.0f, 1.0f);
                transform.position = Vector3.Lerp(transform.position,
                                                   position, pathPart);
                yield return null;
            }

            UnLockMoving();
        }

        _movingLock.Down();
    }

    public void SetPosition (Vector3 position)
    {
        transform.position = position;
    }

    public void SetRotation (Vector3 rotation)
    {
        transform.eulerAngles = rotation;
        xRotAngle = transform.eulerAngles.x;  
        yRotAngle = transform.eulerAngles.y;        
        zRotAngle = transform.eulerAngles.z;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public void ReturnToStartPosition()
    {
        FlyToPosition (_startPosition);
    }

    private void ReturnToStartRotation()
    {
        SetRotation(_startRotation);
    }

    public void FlyToFront() 
    {
        FlyToPosition (_positionInFront);
    }

    public void FlyBack() 
    {
        ReturnToStartPosition ();
        ReturnToStartRotation();
    }
    
    public void RotateToDelta(Vector3 rotation) 
    {
        RotateToDelta(rotation, true);
    } 


    private void RotateToDelta(Vector3 rotation, bool smooth ) 
    {
        xRotAngle -= rotation.x*turnSpeed;
        yRotAngle += rotation.y*turnSpeed;
        zRotAngle -= rotation.z*turnSpeed;
        Quaternion targetRot = Quaternion.Euler(xRotAngle, yRotAngle,
                                                   zRotAngle);
        if(smooth) {
            transform.rotation = Quaternion.Lerp(transform.rotation, 
                                                   targetRot, Time.time);
        } else {
            transform.rotation = targetRot;
        }   
    }

    public void Move(Vector3 delta)
    {   
        delta = (delta.x*transform.forward + 
                         delta.y*transform.right);
        Vector3 targetPos = transform.position + delta*moveSpeed;
        transform.position = Vector3.Lerp(transform.position,
                                           targetPos, Time.time);
    }

    public void ScaleToDelta(Vector3 delta)
	{   
        Vector3 targetScale = transform.localScale + delta*scaleSpeed;
        transform.localScale = Vector3.Lerp(transform.localScale,
                                           targetScale, Time.time);
    }

    public void Highlight()
    {   
        _renderer.material.color = _highlightColor;
    }

    public void CancelHighlight()
    {   
        _renderer.material.color = _defaultColor;
    }

    public Part GetParrent ()
    {
        return GetComponentInParent<Part> ();
    }

    public void SetTipText (string inputText)
    {
        infoText = inputText;
    }

    public string GetTipText()
    {
        return tipText;
    }

    public void SetInfoText (string inputText)
    {
        tipText = inputText;
    }

    public string GetInfoText() 
    {
        return infoText;
    }

    public void RotationModeOn(bool mode) 
    {
        _rotate = mode;
    }

    public void DragModeOn(bool mode)
    {
        _drag = mode;
    }

    public void ScaleModeOn(bool mode)
    {
        _scale = mode;
    }

    private bool LockMoving()
    {
        bool result = false;
        if(!_is_moving) {
            _is_moving = true;
            result = true;
        }
        return result;
    }

    private bool UnLockMoving()
    {
        _is_moving = false;
        return true;
    }

}
