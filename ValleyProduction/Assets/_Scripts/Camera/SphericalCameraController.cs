using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public class SphericalCameraController : MonoBehaviour
//{

//    [SerializeField] private SphericalCoordinates cameraSphericalCoordinates = default;
//    [SerializeField] private Transform playerCamera = default;
//    [SerializeField] private Transform cameraViewTarget = default;
//    [SerializeField] private Transform cameraPositionTarget = default;

//    [SerializeField] private float speed = default;

//    private void Start()
//    {
//        cameraSphericalCoordinates = SphericalSystem.ToSpherical(playerCamera);
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        MoveHorizontal(cameraViewTarget, playerCamera);
//        Rotate(cameraSphericalCoordinates, ref playerCamera);
//    }

//    void MoveHorizontal(Transform obj)
//    {
//        Vector3 dir = obj.forward * Input.GetAxis("Vertical") + obj.right * Input.GetAxis("Horizontal");
//        dir.Normalize();
//        obj.position = obj.position + dir * speed * Time.deltaTime;
//    }
//    void MoveHorizontal(Transform obj, Transform axisObj)
//    {
//        Vector3 dir = axisObj.forward * Input.GetAxis("Vertical") + axisObj.right * Input.GetAxis("Horizontal");
//        dir.Normalize();
//        obj.position = obj.position + dir * speed * Time.deltaTime;
//    }

//    void UpdateObjectSpericalCoords(Transform targetObj, SphericalCoordinates destCoords)
//    {
//        destCoords = SphericalSystem.ToSpherical(targetObj);
//    }

//    void Rotate(SphericalCoordinates sphericalCoords, ref Transform objToRotate)
//    {
//        if (Input.GetKey(KeyCode.Mouse2))
//        {
//            sphericalCoords.AzimuthalAngle += Input.GetAxis("Mouse X") * 10;
//            Debug.Log(sphericalCoords.AzimuthalAngle);
//        }

//        objToRotate.position = SphericalSystem.ToCart(sphericalCoords);
//    }
//}
