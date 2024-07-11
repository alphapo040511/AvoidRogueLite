using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphericalCamera : MonoBehaviour
{
    public Transform target;                    //카메라가 바라볼 대상
    public float distance = 10.0f;              //대상으로부터의 거리
    public float mouseSensitivity = 100.0f;     //마우스 감도
    public float scrollScsitivity = 2.0f;       //스크롤 감도
    public float minYAngle = 5.0f;              //최소 수직 각도
    public float maxYAngle = 85.0f;             //최대 수직 각도

    private float currentHorizontalAngle = 0.0f;            //수평 회전 각도
    private float currentVerticalAngle = 4.5f;              //수직 회전 각도

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    
    void Update()
    {
        if(target != null)
        {
            HandleInput();
            UpdateCameraPosition();
        }
    }

    private void HandleInput()
    {
        //마우스 이동에 따른 각도 조정
        currentHorizontalAngle -= Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        currentVerticalAngle += Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        currentVerticalAngle = Mathf.Clamp(currentVerticalAngle, minYAngle, maxYAngle);         //최대 최소값으로 수직 이동을 막는다.

        //스크롤에 따른 거리 조정
        distance += -Input.GetAxis("Mouse ScrollWheel") * scrollScsitivity;
        distance = Mathf.Clamp(distance, 2.0f, 100.0f);                             //거리 제한
    }

    private void UpdateCameraPosition()
    {
        //구면 좌표계를 사용한 위치 계산
        float verticalAngleRadians = currentVerticalAngle * Mathf.Deg2Rad;                  //각도를 래디안으로 변환
        float horizontalAngleRaians = currentHorizontalAngle * Mathf.Deg2Rad;

        float x = distance * Mathf.Sin(verticalAngleRadians) * Mathf.Cos(horizontalAngleRaians);
        float z = distance * Mathf.Sin(verticalAngleRadians) * Mathf.Sin(horizontalAngleRaians);
        float y = distance * Mathf.Cos(verticalAngleRadians);

        Vector3 newPosition = new Vector3(x, y, z);
        newPosition = target.position + newPosition;

        //지금 위치에서 새 위치로 이동
        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * 6);     //보간 속도 설정
        transform.LookAt(target);
    }
}
