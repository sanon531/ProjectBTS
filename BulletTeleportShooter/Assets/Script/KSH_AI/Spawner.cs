using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Spawner : MonoBehaviour
{
    [Header("- Delay")]
    [SerializeField] private float spawnDelay;

    [Header("- Spawn Object")]
    [SerializeField] private Enemy spawnableEnemy;

    private Sequence spawnSequence;


    private void Start()
    {
        spawnSequence = DOTween.Sequence();
        spawnSequence.
            AppendCallback(() =>
                {
                    Instantiate(spawnableEnemy, transform.position, Quaternion.identity).Init();
                }).
            AppendInterval(spawnDelay).
            SetLoops(-1);
    }
}
