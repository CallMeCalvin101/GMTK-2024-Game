using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HarpoonFire : MonoBehaviour
{

    // Text field
    public Text powerText;

    // Harpoon and properties
    public GameObject harpoon;
    public float shootForce, upwardForce, maxDistance, pullForce, grabRadius, chargeRate;

    // Gun Properties
    public float timeBetweenShooting, spread, reloadTime, timeBetweenShots;
    public int magazineSize, bulletsPerTap;
    public bool allowBulletHold, isActive;

    int bulletsLeft, bulletsShot;
    float curShootForce;

    // bools
    bool shooting, readyToShoot, reloading, isPulling, isCharging, isSticked;

    // Reference
    public Camera fpsCam;
    public Transform attackPoint;
    public PullingMinigame minigame;
    public Image hitPopup;
    public Text hitText;

    public bool allowInvoke = true;
    private GameObject currentHarpoon;

    private void Awake()
    {
        isActive = true;
        updateText();
        bulletsLeft = magazineSize;
        readyToShoot = true;
        isPulling = false;
        isCharging = false;
        curShootForce = 0;
        showHitMarker(false);
    }

    private void Update()
    {
        if (isActive == true)
        {
            MyInput();
            checkHarpoon();
            if (isPulling) Pull();
            if (isCharging) chargeHarpoon();
            if (!readyToShoot && !isPulling)
            {
                if (currentHarpoon != null)
                {
                    if (currentHarpoon.TryGetComponent(out HarpoonSticking firedHarpoon))
                    {
                        if (firedHarpoon.isSticked()) showHitMarker(true);
                    }
                }
            }
        }
    }

    private void MyInput()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && isCharging == false) { 
            isCharging = true;
        }

        if (allowBulletHold) shooting = Input.GetKeyUp(KeyCode.Mouse0);
        else shooting = Input.GetKeyUp(KeyCode.Mouse0);

        // Reloading
        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading) Reload();

        // Shooting
        if (readyToShoot && shooting && !reloading && bulletsLeft > 0 && isCharging == true)
        {
            bulletsShot = 0;
            isCharging = false;
            Shoot();
        }

        // Pulling
        bool pulling = Input.GetKey(KeyCode.Mouse1);
        if (pulling && !readyToShoot && !isPulling && currentHarpoon) parsePull();
    }

    private void parsePull()
    {
        if (currentHarpoon.TryGetComponent(out HarpoonSticking firedHarpoon)) {
            if (firedHarpoon.isSticked())
            {
                startMinigame();
            }
            else isPulling = true;
        }
        else isPulling = true;
    }

    private void Shoot()
    {
        readyToShoot = false;

        // find hit position
        Ray ray = fpsCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        // checks to see ray hits something
        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit)) {
            targetPoint = hit.point;
        } else {
            targetPoint = ray.GetPoint(maxDistance);
        }

        // get direction from bullet to target
        Vector3 directionToTarget = targetPoint - attackPoint.position;

        // Spawns bullet
        currentHarpoon = Instantiate(harpoon, attackPoint.position, Quaternion.identity);
        currentHarpoon.transform.forward = directionToTarget.normalized;
        currentHarpoon.GetComponent<Rigidbody>().AddForce(directionToTarget.normalized * curShootForce, ForceMode.Impulse);

        curShootForce = 0;
        bulletsLeft--;
        bulletsShot++;
            /*
        // Invoke reset function with time between shooting
        if (allowInvoke)
        {
            Invoke("ResetShot", timeBetweenShooting);
            allowInvoke = false;
        }
            */

    }

    private void startMinigame()
    {
        showHitMarker(false);
        isActive = false;
        minigame.startNewMinigame();
    }

    public void endMinigame(bool result)
    {
        isActive = true;
        isPulling = true;
        showHitMarker(false);
    }

    private void Pull()
    {
        Vector3 directionToPlayer = attackPoint.position - currentHarpoon.transform.position;
        currentHarpoon.transform.forward = directionToPlayer.normalized;
        currentHarpoon.GetComponent<Rigidbody>().AddForce(directionToPlayer.normalized * pullForce, ForceMode.Impulse);
        currentHarpoon.GetComponent<Rigidbody>().drag = 0;
    }

    private void chargeHarpoon()
    {
        if (curShootForce < shootForce)
        {
            curShootForce += chargeRate;
            updateText();
        }
    }

    private void checkHarpoon()
    {
        if (isPulling && Vector3.Distance(currentHarpoon.transform.position, attackPoint.position) < grabRadius)
        {
            resetHarpoon();
        }
    }

    private void resetHarpoon()
    {
        Destroy(currentHarpoon);
        isPulling = false;
        ResetShot();
    }

    private void ResetShot()
    {
        // Allow shooting and invoke again
        readyToShoot = true;
        allowInvoke = true;
    }

    private void Reload()
    {
        reloading = true;
        Invoke("ReloadFinished", reloadTime);
    }

    private void ReloadFinished()
    {
        bulletsLeft = magazineSize;
        reloading = false;
    }

    private void updateText()
    {
        powerText.text = curShootForce.ToString();
    }

    private void showHitMarker(bool state)
    {
        hitPopup.enabled = state;
        hitText.enabled = state;
    }
}
