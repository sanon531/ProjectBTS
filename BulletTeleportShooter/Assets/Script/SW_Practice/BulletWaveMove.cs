using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.TopDownEngine;
public class BulletWaveMove : MonoBehaviour
{
    [SerializeField]
    Vector3 dir ;
    [SerializeField] float frequency = 20f;
    [SerializeField] float magnitude = 0.5f;
    [SerializeField]
    Vector3 LeftDirr;
    [SerializeField]
    float timerFotConstantWave = 0;
    // Start is called before the first frame update
    void OnEnable()
    {
        dir = GetComponent<Projectile>().Direction;
        LeftDirr = (Quaternion.AngleAxis(90, new Vector3(0, 0, 1)) * dir).normalized;
    }
    private void OnDisable()
    {
        timerFotConstantWave = 0;
    }
    // Update is called once per frame
    void Update()
    {
        timerFotConstantWave += Time.deltaTime;
        transform.position += LeftDirr * Mathf.Cos(timerFotConstantWave * frequency) * magnitude;
    }
}
