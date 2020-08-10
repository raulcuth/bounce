using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class TapController : MonoBehaviour {
    public delegate void PlayerDelegate();

    public static event PlayerDelegate OnPlayerDied;

    public float sidewaysSpeed = 10f;
    public Camera cam;
    public Vector3 startPos;

    private Rigidbody2D _rigidbody2D;
    private Vector2 _bottomLeft;


    private void Start() {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _bottomLeft = (Vector2) cam.ScreenToWorldPoint(new Vector3(0, 0, cam.nearClipPlane));
    }

    private void OnEnable() {
        GameManager.OnGameStarted += OnGameStarted;
        GameManager.OnGameOverConfirmed += OnGameOverConfirmed;
    }

    private void OnDisable() {
        GameManager.OnGameStarted -= OnGameStarted;
        GameManager.OnGameOverConfirmed -= OnGameOverConfirmed;
    }

    private void OnGameStarted() {
        _rigidbody2D.velocity = Vector3.zero;
        _rigidbody2D.simulated = true;
    }

    private void OnGameOverConfirmed() {
        transform.localPosition = startPos;
    }

    private void Update() {
        CheckPlayerDied();
        if (Input.touchCount > 0) {
            var touch = Input.GetTouch(0);
            var screenHalf = Screen.width / 2;

            switch (touch.phase) {
                case TouchPhase.Began:
                case TouchPhase.Stationary:
                    Move(touch, screenHalf);
                    break;
                case TouchPhase.Ended:
                    _rigidbody2D.velocity = new Vector2(0, PlayerDownwardVelocity());
                    break;
                case TouchPhase.Moved:
                    break;
                case TouchPhase.Canceled:
                    CancelTouch();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    private float PlayerDownwardVelocity() {
        if (_rigidbody2D.velocity.y > 0) {
            return -_rigidbody2D.velocity.y;
        } else {
            return _rigidbody2D.velocity.y;
        }
    }

    private void Move(Touch touch, int screenHalf) {
        if (touch.position.x > screenHalf) {
            GoRight();
        }

        if (touch.position.x < screenHalf) {
            GoLeft();
        }
    }

    private void CancelTouch() {
        _rigidbody2D.velocity = new Vector2(0f, PlayerDownwardVelocity());
    }

    private void GoLeft() {
        _rigidbody2D.velocity = new Vector2(-sidewaysSpeed, PlayerDownwardVelocity());
    }

    private void GoRight() {
        _rigidbody2D.velocity = new Vector2(sidewaysSpeed, PlayerDownwardVelocity());
    }

    private void CheckPlayerDied() {
        if (_rigidbody2D.position.y <= _bottomLeft.y && _rigidbody2D.simulated) {
            _rigidbody2D.simulated = false;
            OnPlayerDied();
        }
    }
}