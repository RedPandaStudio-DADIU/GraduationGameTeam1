/*
 * @Basile Barnola
 * @WeaponBuilder.cs
 * @email : barnola.b@gmail.com
 */


using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WeaponBuilder : MonoBehaviour {


    Transform pos_accessory = null;
    Transform pos_butt = null;
    Transform pos_cannon = null;
    Transform pos_sight = null;

    GameObject weapon = null;
    GameObject accessory = null;
    GameObject butt = null;
    GameObject cannon = null;
    GameObject sight = null;

    /// <summary>
    /// Creat and initialyze the weapon with the given GameObject.
    /// Search the subObject's weapon's position in children's given gameObject.
    /// </summary>
    /// <param name="_path"></param>
    /// <returns></returns>
    public void Init(GameObject _wp)
    {
        if (_wp != null)
        {
            weapon = Instantiate(_wp, transform);
            weapon.transform.localScale = Vector3.one;

            pos_accessory = weapon.transform.Find("loc_accessory");
            pos_butt = weapon.transform.Find("loc_butt");
            pos_cannon = weapon.transform.Find("loc_cannon");
            pos_sight = weapon.transform.Find("loc_sight");
        }
    }

    // All method to add subObject on weapon -> arg. == null -> Remove the subObject if existing.
    #region Add Method :
    /// <summary>
    /// Add a copy of the given gameObject on the accessory's weapon's position.
    /// </summary>
    /// <param name="_path"></param>
    /// <returns></returns>
    public void Add_Accessory(GameObject _obj)
    {
        if(pos_accessory != null)
        {
            if (accessory != null)
                DestroyImmediate(accessory);
            if (_obj != null)
            {
                accessory = Instantiate(_obj, pos_accessory.position, pos_accessory.rotation, pos_accessory);
                accessory.transform.localScale = Vector3.one;
                accessory.transform.position = pos_accessory.position;
            }
        }
    }
    /// <summary>
    /// Add a copy of the given gameObject on the butt's weapon's position.
    /// </summary>
    /// <param name="_path"></param>
    /// <returns></returns>
    public void Add_Butt(GameObject _obj)
    {
        if(pos_butt != null)
        {
            if (butt != null)
            {
                DestroyImmediate(butt);
                butt = null;
            }
            if (_obj != null)
            {
                butt = Instantiate(_obj, pos_butt.position, pos_butt.rotation, pos_butt);
                butt.transform.localScale = Vector3.one;
                butt.transform.position = pos_butt.position;
            }
        }
    }
    /// <summary>
    /// Add a copy of the given gameObject on the cannon's weapon's position.
    /// </summary>
    /// <param name="_path"></param>
    /// <returns></returns>
    public void Add_Cannon(GameObject _obj)
    {
        if( pos_cannon != null)
        {
            if (cannon != null)
                DestroyImmediate(cannon);
            if (_obj != null)
            {
                cannon = Instantiate(_obj, pos_cannon.position, pos_cannon.rotation, pos_cannon);
                cannon.transform.localScale = Vector3.one;
                cannon.transform.position = pos_cannon.position;
            }
        }
    }
    /// <summary>
    /// Add a copy of the given gameObject on the sight's weapon's position.
    /// </summary>
    /// <param name="_path"></param>
    /// <returns></returns>
    public void Add_Sight(GameObject _obj)
    {
       if (pos_sight != null)
       {
            if (sight != null)
                DestroyImmediate(sight);
            if (_obj != null)
            {
                sight = Instantiate(_obj, pos_sight.position, pos_sight.rotation, pos_sight);
                sight.transform.localScale = Vector3.one;
                sight.transform.position = pos_sight.position;
            }
        }
    }
    #endregion

    // All method to add material on weapon or subObject's weapon.
    #region Set Material Method :
    /// <summary>
    /// Set the given material on the weapon's mesh.
    /// </summary>
    /// <param name="_path"></param>
    /// <returns></returns>
    public void Set_Weapon_Mat(Material _mat)
    {
        if (weapon != null && _mat != null)
        {
            MeshRenderer mshRnd = weapon.GetComponent<MeshRenderer>();
            if (mshRnd != null)
            {
                mshRnd.material = _mat;
            }
        }
    }
    /// <summary>
    /// Set the given material on the butt's weapon's mesh.
    /// </summary>
    /// <param name="_path"></param>
    /// <returns></returns>
    public void Set_Butt_Mat(Material _mat)
    {
        if (butt != null && _mat != null)
        {
            MeshRenderer mshRnd = butt.GetComponent<MeshRenderer>();
            if (mshRnd != null)
            {
                mshRnd.material = _mat;
            }
        }
    }
    /// <summary>
    /// Set the given material on the accessory's weapon's mesh.
    /// </summary>
    /// <param name="_path"></param>
    /// <returns></returns>
    public void Set_Accessory_Mat(Material _mat)
    {
        if (accessory != null && _mat != null)
        {
            MeshRenderer mshRnd = accessory.GetComponent<MeshRenderer>();
            if (mshRnd != null)
            {
                mshRnd.material = _mat;
            }
        }
    }
    /// <summary>
    /// Set the given material on the sight's weapon's mesh.
    /// </summary>
    /// <param name="_path"></param>
    /// <returns></returns>
    public void Set_Sight_Mat(Material _mat)
    {
        if (sight != null && _mat != null)
        {
            MeshRenderer mshRnd = sight.GetComponent<MeshRenderer>();
            if (mshRnd != null)
            {
                mshRnd.material = _mat;
            }
        }
    }
    /// <summary>
    /// Set the given material on the cannon's weapon's mesh.
    /// </summary>
    /// <param name="_path"></param>
    /// <returns></returns>
    public void Set_Cannon_Mat(Material _mat)
    {
        if (cannon != null && _mat != null)
        {
            MeshRenderer mshRnd = cannon.GetComponent<MeshRenderer>();
            if (mshRnd != null)
            {
                mshRnd.material = _mat;
            }
        }
    }
    #endregion
}
