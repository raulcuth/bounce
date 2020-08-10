using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MouseControll : MonoBehaviour {
    public float sidewaysSpeed = 10f;
    private Vector3 mousePosition;
    private Rigidbody2D _rigidbody2D;

    private void Start() {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Update() {
        var screenHalf = Screen.width / 2;
        if (Input.GetMouseButton(0)) {
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (mousePosition.x > screenHalf) {
                GoRight();
            }

            if (mousePosition.x < screenHalf) {
                GoLeft();
            }
        } else {
            _rigidbody2D.velocity = Vector2.zero;
        }
    }

    private void GoLeft() {
        _rigidbody2D.velocity = new Vector2(-sidewaysSpeed, 0f);
    }

    private void GoRight() {
        _rigidbody2D.velocity = new Vector2(sidewaysSpeed, 0f);
    }
}