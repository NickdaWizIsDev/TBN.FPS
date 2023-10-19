
using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// Thanks for downloading my projectile gun script! :D
/// Feel free to use it in any project you like!
/// 
/// The code is fully commented but if you still have any questions
/// don't hesitate to write a yt comment
/// or use the #coding-problems channel of my discord server
/// 
/// Dave
public class ProjectileGun : MonoBehaviour
{
    public bool shootingEnabled = true;

    [Header("Attatch your bullet prefab")]
    public GameObject bullet;

    //Gun stats
    public float shootForce;
    public float timeBetweenShooting, spread, reloadTime, timeBetweenShots;
    public int bulletsPerTap;
    public bool allowButtonHold;
    int bulletsShot;

    //recoil
    public Rigidbody playerRb;
    public float recoilForce;

    //some bools
    bool shooting, readyToShoot;

    public Camera playerCam;
    public GameObject muzzleFlash;
    public Transform attackPoint;

    //Show bullet amount
    public Sprite weapon;
    public Image weaponDisplay;

    public bool allowInvoke = true;

    private void Start()
    {
        readyToShoot = true;
    }
    void Update()
    {
        MyInput();

        //weaponDisplay.sprite = weapon;
    }
    private void MyInput()
    {
        //Input
        if (allowButtonHold) shooting = Input.GetKey(KeyCode.Mouse0);
        else shooting = Input.GetKeyDown(KeyCode.Mouse0);

        //Shoot
        if (readyToShoot && shooting){
            bulletsShot = bulletsPerTap;
            Shoot(); //Function has to be after bulletsShot = bulletsPerTap
        }
    }
    private void Shoot()
    {
        if (!shootingEnabled) return;

        readyToShoot = false;

        //Find the hit position using a raycast
        Ray ray = playerCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        //Check if the ray hits something
        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
            targetPoint = hit.point;
        else
            targetPoint = ray.GetPoint(75);

        //Calculate direction
        Vector3 directionWithoutSpread = targetPoint - attackPoint.position;

        //Spread
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);
        float z = Random.Range(-spread, spread);

        //Calc Direction with Spread
        Vector3 directionWithSpread = directionWithoutSpread + new Vector3(x, y, z);

        //Instantiate bullet/projectile
        GameObject currentBullet = Instantiate(bullet, attackPoint.position, Quaternion.identity);
        currentBullet.transform.forward = directionWithSpread.normalized;

        //AddForce
        currentBullet.GetComponent<Rigidbody>().AddForce(directionWithSpread.normalized * shootForce, ForceMode.Impulse);
        //Activate bullet
        if (currentBullet.GetComponent<CustomProjectiles>()) currentBullet.GetComponent<CustomProjectiles>().activated = true;

        GameObject flash = Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity, gameObject.transform);
        Destroy(flash, 0.5f);

        bulletsShot--;

        if (allowInvoke)
        {
            Invoke("ShotReset", timeBetweenShooting);
            allowInvoke = false;

            //Add recoil force to player (should only be called once)
            playerRb.AddForce(-directionWithoutSpread.normalized * recoilForce, ForceMode.Impulse);
        }

        if (bulletsShot > 0)
            Invoke("Shoot", timeBetweenShots);
    }
    private void ShotReset()
    {
        readyToShoot = true;
        allowInvoke = true;
    }

    #region Setters

    public void SetShootForce(float v)
    {
        shootForce = v;
    }
    public void SetFireRate(float v)
    {
        float _v = 2 / v;
        timeBetweenShooting = _v;
    }
    public void SetSpread(float v)
    {
        spread = v;
    }
    public void SetBulletsPerTap(float v)
    {
        int _v = Mathf.RoundToInt(v);
        bulletsPerTap = _v;
    }

    #endregion
}