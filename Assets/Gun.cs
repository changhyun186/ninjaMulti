using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour,Item
{
    [Header("Stat")]
    [SerializeField]
    int damage = 10;
    [SerializeField]
    int bulletCount = 40;
    [SerializeField]
    int bulletFullCount = 40;
    [SerializeField]
    int totalBulletCount = 120;

    public bool isSit;
    [SerializeField]
    MyPlayer player;
    [SerializeField]
    OtherPlayer otherPlayer;
    [SerializeField]
    GameObject bulletPrefab,muzzle;
    [SerializeField]
    float delay,shakePower,shakeRadius;
    [SerializeField]
    CameraController cameraController;
    Transform shootPoint;
    Coroutine autoShoot;
    [SerializeField]
    float reloadDuration;
    [SerializeField]
    ParticleSystem smokeParticle;

    [SerializeField]
    TMPro.TMP_Text ammoText;
    private void Awake()
    {
        shootPoint = transform.Find("ShootPoint");
    }
    public void Shoot()
    {
        if (bulletCount <= 0)
        {
            StopShoot();
            return;
        }
        bulletCount--;
        ammoTextUpdate();
        Shake();
        //var bullet = Instantiate(bulletPrefab);
        var bullet = PoolManager.Instance.Pop("bullet");
        bullet.transform.position = shootPoint.position;
        bullet.transform.rotation = transform.rotation;
        bullet.GetComponent<Bullet>().SetDir();

        var vel = player.rigidbody.velocity;
        vel.y = player.isGround?0:vel.y;
        bullet.GetComponent<Rigidbody>().velocity = vel;
    }
    
    public void ammoTextUpdate()
    {
        if (player != null)
        ammoText.text = bulletCount + "/" + bulletFullCount;
    }
    IEnumerator _StartAutoShoot()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(delay);
        while (true)
        {
            Shoot();
            yield return waitForSeconds;
        }
    }

    public void StartAutoShoot()
    {
        muzzle.SetActive(true);
        autoShoot =  StartCoroutine(_StartAutoShoot());
    }
    public void StopShoot()
    {
        muzzle.SetActive(false);
        StopCoroutine(autoShoot);
        //StopAllCoroutines();
        smokeParticle.Play();
    }
    
    public void Shake()
    {
        if (player == null) print("player is null");
        if (player.rigidbody == null) print("player.transform is null");

        Vector3 dir = Vector3.left * shakePower + Random.insideUnitSphere * shakeRadius;
        if (isSit)
            dir *= 0.1f;
        cameraController.RecieveMouseInput(dir);
    }

    [ContextMenu("REload")]
    public void Reload()
    {
        if (bulletCount >= bulletFullCount) return;
        if(player!=null)
        player.animationController.Reload();
        if(otherPlayer!=null)
            otherPlayer.animationController.Reload();
        StartCoroutine(ReloadCoroutine());
    }

    public IEnumerator ReloadCoroutine()
    {
        yield return new WaitForSeconds(reloadDuration);
        bulletCount = bulletFullCount;
        ammoTextUpdate();
    }
}
