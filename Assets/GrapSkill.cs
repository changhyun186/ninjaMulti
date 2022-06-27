using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapSkill : MonoBehaviour,Item
{
    [SerializeField]
    AnimationController animationController;
    [SerializeField]
    ForceMode forceMode;
    public Rigidbody myPlayer;
    public Transform target;
    public Transform rope;
    LineRenderer line;
    [SerializeField] float Distance;
    [SerializeField] float MinDistance;
    public bool isGrapping;
    public bool isLocked;
    [SerializeField] float ropePlus;
    [SerializeField] float inCircleForce;
    [SerializeField] float inCircleDefaultForce;
    
    // Start is called before the first frame update
    void Start()
    {
        animationController = GetComponentInParent<AnimationController>();
    }

    public void Grap(Vector3 grapPoint)
    {
        animationController.Throw();
        gameObject.SetActive(true);
        isGrapping = true;
        isLocked = false;
        print("grap");
        //target.gameObject.SetActive(true);
        target.transform.position = grapPoint;
        transform.LookAt(target);
       
        transform.localScale = new Vector3(1, 1, 1);

        Distance = Vector3.Distance(target.position, transform.position);



    }

    public void RemoveGrap(Vector3 grapPoint)
    {
        isGrapping = false;
        isLocked = false;
        print("RemoveGrap");
        gameObject.SetActive(false);
    }

    public void RemoveGrap()
    {
        isGrapping = false;
        isLocked = false;
        print("RemoveGrap");
        gameObject.SetActive(false);
    }

    bool FirstLock;
    // Update is called once per frame
    void Update()
    {
        if(isGrapping)
        {
            transform.LookAt(target);
            if(!isLocked)
            {
                var scale = transform.localScale;
                scale.x = 1;
                scale.y = 1;
                scale.z += ropePlus*Time.deltaTime;
                transform.localScale = scale;
            }
            else
            {

                var targetDir = target.position - transform.position;
                if(!FirstLock)
                {
                    FirstLock = true;

                    myPlayer.AddForce(targetDir.normalized * inCircleDefaultForce, ForceMode.Impulse);
                }
                //myPlayer.AddForce(targetDir * inCircleDefaultForce * Time.deltaTime, forceMode);
                if (Vector3.SqrMagnitude(targetDir) > Distance * Distance)
                {
                    myPlayer.AddForce(targetDir.normalized * inCircleForce*Time.deltaTime, forceMode);
                }
                else if (Vector3.SqrMagnitude(targetDir) < Distance * Distance)
                {
                    Distance = Mathf.Clamp(Mathf.Sqrt(Vector3.SqrMagnitude(targetDir)), MinDistance , Distance);
                }
            }

        }
        else if(FirstLock)
        {
            FirstLock = false;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        print("OnTriggerEnter");
        if (other.tag == "GrapTarget")
            isLocked = true;
    }

    public void InitMinDistance()
    {
        var mypos = transform.position;
        Distance =  Vector3.Distance(mypos, target.position);
        Distance = Mathf.Clamp(Distance,MinDistance, Mathf.Infinity);
    }
}
