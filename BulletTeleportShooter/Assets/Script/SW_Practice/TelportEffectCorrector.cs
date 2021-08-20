using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TelportEffectCorrector : MonoBehaviour
{

    [SerializeField]
    Transform Effect; 
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnEnable()
    {
        Effect.localScale = new Vector3(6/transform.localScale.x, 6/ transform.localScale.x, 6/ transform.localScale.x);
    }





}
