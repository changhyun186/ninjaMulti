using System.Collections;
using System.Text;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : PoolableMono
{
    float damage = 10;
    [SerializeField]
    ForceMode forceMode;
    [SerializeField]
    float gravityScale,speed;
    [SerializeField]
    TrailRenderer trailRenderer;
    Rigidbody rigidbody;
    Vector3 dir;
    public override void OnDie()
    {
        trailRenderer.Clear();
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
        //rigidbody.velocity = Vector3.zero;
        rigidbody.ResetInertiaTensor();
    }

    public override void OnSpawn()
    {
        Invoke("Kill", 5);
    }

    // Update is called once per frame
    private void Awake()
    {
        trailRenderer = GetComponent<TrailRenderer>();
        rigidbody = GetComponent<Rigidbody>();
    }

    public void SetDir()
    {
        dir = -transform.forward;
    }
    void Update()
    {
        rigidbody.AddForce(dir*speed*Time.deltaTime+Vector3.down*gravityScale, forceMode);
        Vector3 vel = rigidbody.velocity;
        transform.rotation = Quaternion.LookRotation(Quaternion.Euler(90,0,0) * vel);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Play")) return;
            Kill();
            print(other.gameObject.name);
            PoolableMono poolableMono = PoolManager.Instance.Pop("bulletImpact");
            poolableMono.transform.position = other.contacts[0].point;
            poolableMono.transform.rotation = Quaternion.LookRotation(other.contacts[0].normal);
        if(other.gameObject.layer == LayerMask.NameToLayer("Other"))
        {
            DamagePacket packet = new DamagePacket();
            packet.damage = damage;
            PoolableMono poolableMono2 = PoolManager.Instance.Pop("blood");
            poolableMono2.transform.position = other.contacts[0].point;
            poolableMono2.transform.rotation = Quaternion.LookRotation(other.contacts[0].normal);
            byte[] arr = BackEndIngame.StringToByte((int)packetType.DamagePacket+JsonUtility.ToJson(packet));
            BackEndIngame.Instance.Send(arr);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        print(other.name);

        if (other.gameObject.layer == LayerMask.NameToLayer("Play")) return;
        Kill();
        print(other.gameObject.name);
        PoolableMono poolableMono = PoolManager.Instance.Pop("bulletImpact");
        poolableMono.transform.position = transform.position;//other.contacts[0].point;
        poolableMono.transform.rotation = Quaternion.LookRotation(transform.position - other.gameObject.transform.position);
        if (other.gameObject.layer != LayerMask.NameToLayer("Other"))
        {
            DamagePacket packet = new DamagePacket();
            packet.damage = damage;

            byte[] arr = BackEndIngame.StringToByte((int)packetType.DamagePacket + JsonUtility.ToJson(packet));
            BackEndIngame.Instance.Send(arr);
        }
    }



}
