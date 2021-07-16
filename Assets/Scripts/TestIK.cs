using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion.FinalIK;

public class TestIK : MonoBehaviour
{
    [SerializeField]
    Transform bip01, target, head, handL, vvrCamera,vcontrollerL,vcontrollerR,fvrCamera,fcontrollerL,fcontrollerR;
    public BipedIK bipedIK;
    public FullBodyBipedIK fbbIK;
    public LookAtIK lookIK;
    public Vector3 offset_position_head_bip01;
    // Start is called before the first frame update
    void Start()
    {
        // headPositionOriginal = fbbIK.references.head.position;
        // head = transform.Find("BipDummy/Bip002 Pelvis/Bip002 Spine/Bip002 Spine1/Bip002 Spine2/Bip002 Spine3/Bip002 Neck/Bip002 Head");
        bip01 = transform.Find("Bip01");
        head = transform.Find("Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Spine2/Bip01 Neck/Bip01 Head");
        offset_position_head_bip01 = head.position-transform.position;
        vvrCamera = GameObject.Find("Camera").transform;
        vcontrollerL = GameObject.Find("ControllerL").transform;
        vcontrollerR = GameObject.Find("ControllerR").transform;
        fvrCamera = transform.Find("CameraF");
        fcontrollerL = transform.Find("ControllerLF");
        fcontrollerR = transform.Find("ControllerRF");
        transform.position = vvrCamera.position - offset_position_head_bip01;
    }

    // Update is called once per frame
    void Update()
    {
        fvrCamera.position = vvrCamera.position;
        fvrCamera.rotation = vvrCamera.rotation;
        fcontrollerL.position = vcontrollerL.position;
        fcontrollerL.rotation = vcontrollerL.rotation;
        fcontrollerR.position = vcontrollerR.position;
        fcontrollerR.rotation = vcontrollerR.rotation;
        // fbbIK.solver.leftHandEffector.position = target.position;
        // bipedIK.references.head.position = target.position;
        // bipedIK.solvers.lookAt.head.defaultLocalPosition = target.position;
        // bipedIK.solvers.lookAt.head.defaultLocalRotation = Quaternion.Euler(target.position-transform.position);
        // bipedIK.solvers.lookAt.head.solverRotation = Quaternion.Euler(target.position-transform.position);
        // Vector3 offset_rotation_head = new Vector3(0.0f, -90.0f, -90.0f);
        // head.position = target.position;
        // head.rotation = Quaternion.Euler(target.position-transform.position)*Quaternion.Euler(offset_rotation_head);
        // fbbIK.solver.Update();
        // lookIK.solver.head.solverRotation = Quaternion.Euler(target.position-transform.position)*Quaternion.Euler(offset_rotation_head);
        // head.rotation = Quaternion.Euler(target.position-transform.position)*Quaternion.Euler(offset_rotation_head);;
        // lookIK.solver.IKPosition
    }
}
