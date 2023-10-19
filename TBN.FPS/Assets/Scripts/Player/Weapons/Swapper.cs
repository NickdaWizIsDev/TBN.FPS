using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swapper : MonoBehaviour
{
    public GameObject weaponHolder;
    public List<Transform> weapons;
    public int index;

    public GameObject activeWeapon;

    private InputManager manager;

    private void Awake()
    {
        manager = GetComponent<InputManager>();
    }

    private void Start()
    {
        weapons.Add(weaponHolder.transform.GetChild(0));
        weapons.Add(weaponHolder.transform.GetChild(1));
        weapons.Add(weaponHolder.transform.GetChild(2));
        if (activeWeapon == null)
        {
            for (int id = 0; id < weapons.Count; id++)
            {
                if (weapons[id].gameObject.activeInHierarchy)
                {
                    activeWeapon = weapons[id].gameObject;
                    index = id;
                }
            }
        }
    }

    private void Update()
    {
        

        if (Input.GetKeyDown(KeyCode.Alpha1) && !weapons[0].gameObject.activeInHierarchy)
        {
            SwapWeapons(0);
            Debug.Log("Swapped to " + weapons[0].gameObject.name);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && !weapons[1].gameObject.activeInHierarchy)
        {
            SwapWeapons(1);
            Debug.Log("Swapped to " + weapons[1].gameObject.name);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && !weapons[2].gameObject.activeInHierarchy)
        {
            SwapWeapons(2);
            Debug.Log("Swapped to " + weapons[2].gameObject.name);
        }
    }

    private void FixedUpdate()
    {
        if (!activeWeapon.activeInHierarchy)
        {
            for (int id = 0; id < weapons.Count; id++)
            {
                if (weapons[id].gameObject.activeInHierarchy)
                {
                    activeWeapon = weapons[id].gameObject;
                    index = id;
                }
            }
        }
    }

    public void SwapWeapons(int weaponIndex)
    {
        activeWeapon.SetActive(false);
        weapons[weaponIndex].gameObject.SetActive(true);
        
        weaponIndex = Mathf.Clamp( weaponIndex, 0, weapons.Count - 1);

        activeWeapon = weapons[weaponIndex].gameObject;
    }
}
