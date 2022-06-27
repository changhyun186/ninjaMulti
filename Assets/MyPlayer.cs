using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Text;


public class MyPlayer : Agent
{
    [SerializeField]
    OtherPlayer otherPlayer;
    public bool isGround,isSit;

    CharacterController characterController;
    public AnimationController animationController { get; private set; }
    public Rigidbody rigidbody { get; private set; }
    CameraController cameraController;
    PlayerAiming playerAiming;
    public CameraController MainCam { get => cameraController; }
    [SerializeField]
    float moveSpeed = 1,jumpSpeed,grappingMoveSpeed,grappingGravity;
    [SerializeField]
    float gravityScale;
    [SerializeField]
    float jumpDuration;
    [SerializeField]
    GrapSkill grapSkill;
    [SerializeField]
    Gun gun;
    [SerializeField]
    float RunSpeed,defaultSpeed,sitSpeed;

    public bool isDash;
    public PositionPacket positionPacket;
    private void Awake()
    {
        
        moveSpeed = defaultSpeed;
        Physics.gravity = Vector3.down* gravityScale;
        rigidbody = GetComponent<Rigidbody>();
        animationController = GetComponent<AnimationController>();
        cameraController = FindObjectOfType<CameraController>();
        playerAiming = GetComponent<PlayerAiming>();
        characterController = GetComponent<CharacterController>();

    }
    public override void OnDie()
    {
        
    }

    public override void OnSpawn()
    {

    }
    float timer = 0;
    public float distanceSendPos;
    private void Update()
    {
        if (!isGround)
            CancleSit();
        timer += Time.deltaTime;
        if (isMoveSend &&timer > distanceSendPos)
        {
            positionPacket.pos = transform.position;
            positionPacket.rot = transform.rotation;
            byte[] arr = BackEndIngame.StringToByte((int)packetType.PositionPacket + JsonUtility.ToJson(positionPacket));
            BackEndIngame.Instance.Send(arr);
            timer = 0;
        }
    }
    Coroutine coroutine;
    bool isMoveSend;
    public void StartPositionCor()
    {
        isMoveSend = true;
        //coroutine = StartCoroutine(SendPos());
    }
    public IEnumerator SendPos()
    {
        WaitForSeconds waitFor = new WaitForSeconds(0.05f);

        while (true)
        {
            yield return waitFor;
            positionPacket.pos = transform.position;
            positionPacket.rot = transform.rotation;
            byte[] arr = BackEndIngame.StringToByte((int)packetType.PositionPacket + JsonUtility.ToJson(positionPacket));
            BackEndIngame.Instance.Send(arr);
        }
    }

    public override void RecieveMoveInput(Vector3 unNormalizedDir)
    {
        float maxerFrame = Time.deltaTime * 120;

        positionPacket.velocity = unNormalizedDir;
        unNormalizedDir = CheckSlope(unNormalizedDir);
        unNormalizedDir.y = 0;
        unNormalizedDir =  transform.TransformDirection(unNormalizedDir);
        isGround = animationController.animator.GetBool(animationController.isGround);
        if (grapSkill.isGrapping&&!isGround)
        {
            rigidbody.AddForce((unNormalizedDir.normalized * grappingMoveSpeed *maxerFrame) + Vector3.down*grappingGravity * maxerFrame, ForceMode.Acceleration);
        }
        else if(!grapSkill.isGrapping && !isGround)
        {
            rigidbody.AddForce((unNormalizedDir.normalized * moveSpeed * maxerFrame) + Vector3.down * gravityScale * maxerFrame , ForceMode.Acceleration);
            //characterController.Move((unNormalizedDir.normalized * moveSpeed) + Vector3.down * gravityScale);
            //characterController.SimpleMove((unNormalizedDir.normalized * moveSpeed));
        }
        else if(grapSkill.isGrapping && isGround)
        {
            rigidbody.AddForce((unNormalizedDir.normalized * grappingMoveSpeed * maxerFrame) + Vector3.down * grappingGravity * maxerFrame, ForceMode.Acceleration);
            //characterController.Move(unNormalizedDir.normalized * moveSpeed*Time.deltaTime + Physics.gravity*Time.deltaTime);
        }
        else if (!grapSkill.isGrapping && isGround)
        {
            rigidbody.velocity = (unNormalizedDir.normalized * moveSpeed * maxerFrame) + Physics.gravity * maxerFrame;
            //characterController.Move(unNormalizedDir.normalized * moveSpeed*Time.deltaTime + Physics.gravity*Time.deltaTime);
        }
        cameraController.isEffecting = !(unNormalizedDir == Vector3.zero);
    }
    public override void RecieveMouseInput(Vector3 dir)
    {
        var rot = transform.rotation;
        Vector3 targetAngle = rot.eulerAngles + dir;
       // if(isGround)
        positionPacket.rot = transform.rotation = Quaternion.Euler(new Vector3(rot.x,targetAngle.y,rot.z));
        
       // else
            //transform.rotation = Quaternion.Euler(targetAngle);

        //playerAiming.Aim(cameraController);
    }

    public void Sit()
    {
        if (!isGround) return;
        isSit = animationController.Sit();
        moveSpeed = isSit?sitSpeed:defaultSpeed;
        gun.isSit = isSit;
    }

    public void CancleSit()
    {
        if (!isSit) return;
        moveSpeed = defaultSpeed;
        gun.isSit = false;
        isSit = false;
    }
    public void RecieveShiftDownInput()
    {
        if (isSit) return;
        moveSpeed = RunSpeed;
        cameraController.EffectDistance = cameraController.RunEffectDistance;
    }
    public void RecieveShiftUpInput()
    {
        if (isSit) return;
        moveSpeed = defaultSpeed;
        cameraController.EffectDistance = cameraController.WalkEffectDistance;
    }
    public void RecieveJumpInput()
    {
        if (!GetComponent<Animator>().GetBool("isGround"))
        {
            return;
        }
        StopAllCoroutines();
        StartCoroutine(Jump());
        
    }

    IEnumerator Jump()
    {
        
        print("jump");
        float tiemCounter=jumpDuration;
        while(tiemCounter>0)
        {
            var vel = rigidbody.velocity;
            vel.y = Mathf.Lerp(0, jumpSpeed, tiemCounter / jumpDuration);
            rigidbody.velocity = vel;
            tiemCounter -= Time.deltaTime;
            yield return null;
        }

        while(tiemCounter<jumpDuration)
        {
            var vel = rigidbody.velocity;
            vel.y = -Mathf.Lerp(0, jumpSpeed, tiemCounter / jumpDuration);
            rigidbody.velocity = vel;
            tiemCounter += Time.deltaTime;
            yield return null;
        }
    }

    Vector3 before;
    //private void OnCollisionStay(Collision collision)
    //{
    //    print(collision.gameObject.name);
    //    if (collision.gameObject.layer == LayerMask.NameToLayer("Player")) return;
    //    Vector3 curRot = transform.rotation.eulerAngles;
    //    print(collision.contacts[0].normal);
    //    ContactPoint? point = null;
    //    float diff = 0;
    //    foreach(var p in collision.contacts)
    //    {
    //        var cur = p.normal;
    //        float itsDiff = Vector3.Distance(cur, before);
    //        if (point == null ||itsDiff<diff)
    //        {
    //            point = p;
    //            before = p.normal;
    //        }
    //    }
    //    Vector4 dir = point.Value.normal;
    //    dir = Quaternion.Euler(90, 0, 0) * dir;//윗방향을 앞방향으로
    //    transform.parent.rotation = Quaternion.LookRotation(dir);
    //    //print(dir);
    //    //dir = transform.TransformDirection(dir);
    //    //Debug.DrawRay(transform.position, dir*1000,Color.green);
    //    //print("draw");
    //    //Quaternion quaternion = Quaternion.LookRotation(dir);
    //    //transform.rotation.Set(dir.x, dir.y, dir.z, dir.w);
    //    //Debug.LogError(dir+" rpt"+transform.rotation.eulerAngles);
    //    //transform.rotation = quaternion;
    //}
    [Header("Slope"),SerializeField]
    float slopeCheckDistance;
    Vector3 CheckSlope(Vector3 dir)
    {

        if (Physics.Raycast(transform.position, Vector3.down * slopeCheckDistance, out RaycastHit hit))
        {
            if (hit.normal == Vector3.up)
            {
                return dir;
            }
            else
            {
                Debug.DrawRay(transform.position, Vector3.ProjectOnPlane(dir, hit.normal)*100,Color.blue);
                return Vector3.ProjectOnPlane(dir, hit.normal);
            }
        }
        else
            return dir;
    }

    public void Dash()
    {
        isDash = true;
        Invoke("dashFalse", 0.2f);
    }

    public void dashFalse()
    {
        print(otherPlayer);
        print(otherPlayer.transform.position);
        print(transform.position);
        if(Vector3.Distance(otherPlayer.transform.position,transform.position)<2)
        {
            DamagePacket packet = new DamagePacket();
            packet.damage = 10;

            byte[] arr = BackEndIngame.StringToByte((int)packetType.DamagePacket + JsonUtility.ToJson(packet));
            BackEndIngame.Instance.Send(arr);
        }
        isDash = false;
    }
}

