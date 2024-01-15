using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICameraManager : MonoBehaviour
{
    private Camera _camera;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
    }

    void Start()
    {
        SetCameraColor();
    }

    void SetCameraColor()
    {
        var color = StageManager.Instance.GetStageColor();
        _camera.backgroundColor = color;
    }
}
