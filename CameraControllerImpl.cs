using UnityEngine;
using System.Collections;

public class CameraControllerImpl : MonoBehaviour, CameraController
{
    private Vector3 _cameraPosition { set { transform.position = value; } get { return transform.position; } }
    private Vector3 _defaultPlace = Vector3.zero;
    private Vector3 _previousPlace = Vector3.zero;
    private Vector3 _defaultRotation = Vector3.zero;
    private Vector3 _previousRotation = Vector3.zero;
    [SerializeField]private float cameraRotationSpeed_x = 0.0f;
    [SerializeField]private float cameraRotationSpeed_y = 0.0f; 
    [SerializeField] private float m_TiltMax = 75f;                       // The maximum value of the x axis rotation of the pivot.
    [SerializeField] private float m_TiltMin = 45f;  
    [SerializeField] private Vector3 speed = Vector3.zero;
    
    private float m_LookAngle = 0.0f;                    // The rig's y axis rotation.
    private float m_TiltAngle = 0.0f;                    // The pivot's x axis rotation.
    private Quaternion m_TransformTargetRot = Quaternion.identity;
    private bool _blocked = false;
    private InputManager _inputManager = null;


    protected void Start()
    {
        _inputManager = GameObject.FindGameObjectWithTag ("InputManager")
                            .GetComponent<InputManager>();
        _defaultPlace = transform.position;
        _previousPlace = _defaultPlace;
        _defaultRotation = transform.eulerAngles;
        _previousRotation = _defaultRotation;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        m_TiltAngle = transform.eulerAngles.x;
        m_LookAngle = transform.eulerAngles.y;
    }

    void FixedUpdate()
    {
        if(_inputManager.CameraOnDefaultPlace()){
            PutCameraOnDefaultPlace();
        } else {
            if(!_blocked) {
                HandleRotationMovement();
                HandleTranslateMovement();
            }
        }
    }

    private void HandleTranslateMovement()
    {
		Vector3 trans_vect = _inputManager.GetTranslationVector ();
        trans_vect = (trans_vect.x*transform.forward + 
                         trans_vect.y*transform.right);
		trans_vect.Scale (speed);
		Vector3 move_vector = trans_vect*Time.deltaTime;
        FlyToPosition (transform.position + move_vector);
    }

    private void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void HandleRotationMovement()
    {
        Vector3 rot_vector = _inputManager.GetRotationVector();

        m_LookAngle += rot_vector.y*cameraRotationSpeed_y*Time.deltaTime;

        m_TiltAngle -= rot_vector.x*cameraRotationSpeed_x*Time.deltaTime;
        m_TiltAngle = Mathf.Clamp(m_TiltAngle, -m_TiltMin, m_TiltMax);

        m_TransformTargetRot = Quaternion.Euler(m_TiltAngle,
                                                 m_LookAngle, 0f);

        transform.rotation = m_TransformTargetRot;
    }


    public void SetCameraPosition (Vector3 position) 
    {
        
        _cameraPosition = position;
    }

    public Vector3 GetCameraPosition () 
    {

        return _cameraPosition;
    }

	public void Rotate(Vector3 rotation) 
    {
        transform.eulerAngles = rotation;
        m_TiltAngle = transform.eulerAngles.x;
        m_LookAngle = transform.eulerAngles.y;
        // transform.rotation = Quaternion.Slerp(transform.rotation, 
        //                        rotation, Time.time * 0.1f);
    }

    public void FlyToPosition (Vector3 position) 
    {

        transform.position = Vector3.Slerp(transform.position, position, 
                                      Time.time);
    }


    public void PutCameraOnDefaultPlace() 
    {
        
        FlyToPosition (_defaultPlace);
        Rotate (_defaultRotation);
    }

    public void PutCameraOnPreviousPlace()
    {

        FlyToPosition (_previousPlace);
        Rotate (_previousRotation);
    }

    public void BlockCamera()
    {
        _blocked = true;
    }

    public void UnBlockCamera()
    {
        _blocked = false;
    }
    
    public void LookAt(Vector3 pos)
    {
        transform.LookAt(pos);
    }
        
}
