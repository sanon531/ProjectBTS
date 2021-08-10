using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.TopDownEngine;
using MoreMountains.Tools;
public class BommerangBullet : MonoBehaviour
{
    [SerializeField]
    MMAutoRotate autoRotate;
    [SerializeField]
    Projectile projectile;
    [SerializeField]
    Event triggerEvent;

    Coroutine GetTargetCoroutine;

    void Reset()
    {
        autoRotate = GetComponent<MMAutoRotate>();
        projectile = GetComponent<Projectile>();
    }

    void Awake()
    {
        autoRotate = GetComponent<MMAutoRotate>();
        projectile = GetComponent<Projectile>();
       // projectile.OnSpawnComplete += SetDirr;
    }

    private void Start()
    {
        autoRotate.OrbitCenterTransform = PlayerOrbitSetter.instance.transform;
    }


    public void SetDirr()
    {
        Debug.Log(projectile.Direction + " + " + autoRotate.OrbitRotationAxis);
        autoRotate.OrbitRotationAxis =  Vector2.Perpendicular(projectile.Direction);
    }

    private void OnEnable()
    {
        //Debug.Log("enable"+gameObject.name);
        GetTargetCoroutine = StartCoroutine(targetCheckCoroutine());
        
    }
    private void OnDisabled()
    {
        StopCoroutine(GetTargetCoroutine);

    }
    IEnumerator targetCheckCoroutine()
    {
        yield return new WaitForEndOfFrame();

        SetDirr();
    }


        // Update is called once per frame
    void Update()
    {
        
    }
}
