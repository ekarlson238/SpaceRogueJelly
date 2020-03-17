using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAim : MonoBehaviour
{
    [SerializeField]
    private float aimSensitivity;

    [SerializeField]
    private GameObject player;

    [SerializeField]
    private float yRotMin = -30, yRotMax = 60;

    private float yRotationClamp = 0;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        Rotate();
    }

    private void Rotate()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        float rotationX = mouseX * aimSensitivity;
        float rotationY = mouseY * aimSensitivity;

        yRotationClamp += rotationY;
        Debug.Log(yRotationClamp);

        Vector3 rotation = this.transform.rotation.eulerAngles;
        Vector3 playerRotation = player.transform.rotation.eulerAngles;

        rotation.x -= rotationY;
        rotation.y += rotationX;
        playerRotation.y += rotationX;

        if (yRotationClamp > yRotMax)
        {
            yRotationClamp = yRotMax;
            rotation.x = -yRotMax;
        }

        if (yRotationClamp < yRotMin)
        {
            yRotationClamp = yRotMin;
            rotation.x = -yRotMin;
        }

        player.transform.rotation = Quaternion.Euler(playerRotation);
        this.transform.rotation = Quaternion.Euler(rotation);
    }
}
