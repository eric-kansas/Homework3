using UnityEngine;
using System.Collections;

/**
 * The CameraController controls the main camera of the game. 
 * @author  Dan Strenfeld, Eric Heaney
 * @version 1.0, June 29, 2013
 */
public class CameraController : MonoBehaviour, IController {
	
	private const float MIN_HEIGHT = 20.0f;
	private const float MAX_HEIGHT = 35.0f;
	private const float DEFAULT_CAMERA_SPEED = 15.0f;
	private const float RESPAWN_CAMERA_SPEED = 30.0f;
	private const float CAMERA_ACCELERATION = 1.0f;
	
	[SerializeField]
   // private GameObject _target;	
	
	private float _cameraHeight;
	private float _currentCameraSpeed;
	private float _minCameraSpeed;
	private float _maxCameraSpeed;
	private float _cameraAccelerationDirection;
	private Vector2[] _directionViewportOffsets;
	private Vector3 _cameraPositionOffset;
	private Vector3 _targetCameraLocation;
	
	GUIText _speedText;
	
	public float ZoomLevel {
		get {
			return (_cameraHeight - MIN_HEIGHT) / (MAX_HEIGHT - MIN_HEIGHT);
		}
	}	
	
	/** Init for the camera controller */
	public void Init() {
		_cameraAccelerationDirection = 1.0f;	
		ResetPosition();
	}
	
	/** Move the camera towards the target object */
    void LateUpdate() {
		/*
		_targetCameraLocation = new Vector3(_target.transform.position.x , _cameraHeight, _target.transform.position.z) + _cameraPositionOffset;
		
		Vector3 diff = (_targetCameraLocation - transform.position);
		if ( diff.magnitude < _currentCameraSpeed * Time.deltaTime) {
			transform.position = _targetCameraLocation;
		} else {
			transform.position += diff.normalized * _currentCameraSpeed * Time.deltaTime;
		}
		
		if (diff.magnitude < 1.5f) {
			_cameraAccelerationDirection = -0.5f;
		}
		
		_currentCameraSpeed = Mathf.Clamp(_currentCameraSpeed * ((CAMERA_ACCELERATION * _cameraAccelerationDirection * Time.deltaTime) + 1.0f), _minCameraSpeed, _maxCameraSpeed);
		*/
    }
	
	/** Handler for responding to GameNotifications OnResetLevel */
	void OnResetLevel() {
		ResetPosition();
	}
	
	/** Move the camera back to levels start position */
	private void ResetPosition() {
		//_currentCameraSpeed = (_target.transform.position - transform.position).magnitude / 2;
		//_maxCameraSpeed = _currentCameraSpeed;
		//transform.position = new Vector3(_target.transform.position.x , _cameraHeight, _target.transform.position.z) + _cameraPositionOffset;
	}
	
	/** Determine camera offsets based on players direction */
	private void CalculateCameraOffset() {
		
	}
}