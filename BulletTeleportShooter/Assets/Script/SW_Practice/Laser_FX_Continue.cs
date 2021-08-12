﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser_FX_Continue : MonoBehaviour
{
    private GameObject StartObject;
    private GameObject EndObject;
    public Vector3 step;
    public LineRenderer lineRenderer ;


    public Vector3 StartPosition;
    public Vector3 EndPosition;

    [Range(0, 8)]
    [Tooltip("How manu generations? Higher numbers create more line segments.")]
    public int Generations = 6;

    [Range(0.01f, 1.0f)]
    [Tooltip("How long each bolt should last before creating a new bolt. In ManualMode, the bolt will simply disappear after this amount of seconds.")]
    public float Duration = 0.05f;
    private float timer;

    [Range(0.0f, 1.0f)]
    [Tooltip("How chaotic should the lightning be? (0-1)")]
    public float ChaosFactor = 0.15f;

    [Tooltip("In manual mode, the trigger method must be called to create a bolt")]
    public bool ManualMode;

    [Range(1, 64)]
    [Tooltip("The number of rows in the texture. Used for animation.")]
    public int Rows = 1;

    [Range(1, 64)]
    [Tooltip("The number of columns in the texture. Used for animation.")]
    public int Columns = 1;

    [Tooltip("The animation mode for the lightning")]
    public LightningBoltAnimationMode AnimationMode = LightningBoltAnimationMode.PingPong;

    /// <summary>
    /// Assign your own random if you want to have the same lightning appearance
    /// </summary>
    [HideInInspector]
    [System.NonSerialized]
    public System.Random RandomGenerator = new System.Random();
    [SerializeField]
    private List<KeyValuePair<Vector3, Vector3>> segments = new List<KeyValuePair<Vector3, Vector3>>();
    private int startIndex;
    private Vector2 size;
    private Vector2[] offsets;
    private int animationOffsetIndex;
    private int animationPingPongDirection = 1;
    [SerializeField]
    private float ColliderWidth = 4f;
    private PolygonCollider2D polyCollider;

    public bool isSetted = false;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 0;
        UpdateFromMaterialChange();
        StartObject = Player_Electric.Instance.gameObject;
        EndObject = gameObject;
        polyCollider = GetComponent<PolygonCollider2D>();
        isSetted = true;
    }

    private void OnEnable()
    {
    }

    private void Update()
    {
        Trigger();
        SetCollider();
    }

    private void SetCollider()
    {
        Vector3 SPosition = StartObject.transform.position;
        Vector3 EPosition = EndObject.transform.position;

        float m = (EPosition.y - SPosition.y) / (EPosition.x - SPosition.x);
        float dX = (ColliderWidth / 2f) * (m / Mathf.Pow(m * m + 1, 0.5f));
        float dY = (ColliderWidth / 2f) * (1 / Mathf.Pow(1 + m * m, 0.5f));

        Vector3[] offsets= new Vector3[2];

        offsets[0] = new Vector3(-dX, dY);
        offsets[1] = new Vector3(dX, -dY);


        List<Vector2> ColliderPoints = new List<Vector2>
        {
            SPosition + offsets[0],EPosition + offsets[0],EPosition + offsets[1],
            SPosition + offsets[1]
        };



        
        //polyCollider.offset = new Vector2(-SPosition.x, -SPosition.y);
        polyCollider.SetPath(0, ColliderPoints.ConvertAll(p=>(Vector2)transform.InverseTransformPoint(p)));

    }

    public void SetTransform(GameObject t1, GameObject t2)
    {
        if (!isSetted)
        {
            StartObject = t1;
            EndObject = t2;
            isSetted = true;
        }
    }

    public void SetPosition(Vector3 t1, Vector3 t2)
    {
        if (!isSetted)
        {
            StartPosition = t1;
            EndPosition = t2;
            isSetted = true;
        }

    }

    public enum LightningBoltAnimationMode
    {
        /// <summary>
        /// No animation
        /// </summary>
        None,

        /// <summary>
        /// Pick a random frame
        /// </summary>
        Random,

        /// <summary>
        /// Loop through each frame and restart at the beginning
        /// </summary>
        Loop,

        /// <summary>
        /// Loop through each frame then go backwards to the beginning then forward, etc.
        /// </summary>
        PingPong
    }

    /// <summary>
    /// Allows creation of simple lightning bolts
    /// </summary>

     

        private void GetPerpendicularVector(ref Vector3 directionNormalized, out Vector3 side)
        {
            if (directionNormalized == Vector3.zero)
            {
                side = Vector3.right;
            }
            else
            {
                // use cross product to find any perpendicular vector around directionNormalized:
                // 0 = x * px + y * py + z * pz
                // => pz = -(x * px + y * py) / z
                // for computational stability use the component farthest from 0 to divide by
                float x = directionNormalized.x;
                float y = directionNormalized.y;
                float z = directionNormalized.z;
                float px, py, pz;
                float ax = Mathf.Abs(x), ay = Mathf.Abs(y), az = Mathf.Abs(z);
                if (ax >= ay && ay >= az)
                {
                    // x is the max, so we can pick (py, pz) arbitrarily at (1, 1):
                    py = 1.0f;
                    pz = 1.0f;
                    px = -(y * py + z * pz) / x;
                }
                else if (ay >= az)
                {
                    // y is the max, so we can pick (px, pz) arbitrarily at (1, 1):
                    px = 1.0f;
                    pz = 1.0f;
                    py = -(x * px + z * pz) / y;
                }
                else
                {
                    // z is the max, so we can pick (px, py) arbitrarily at (1, 1):
                    px = 1.0f;
                    py = 1.0f;
                    pz = -(x * px + y * py) / z;
                }
                side = new Vector3(px, py, pz).normalized;
            }
        }

        private void GenerateLightningBolt(Vector3 start, Vector3 end, int generation, int totalGenerations, float offsetAmount)
        {
            if (generation < 0 || generation > 8)
            {
                return;
            }

            segments.Add(new KeyValuePair<Vector3, Vector3>(start, end));
            if (generation == 0)
            {
                return;
            }

            Vector3 randomVector;
            if (offsetAmount <= 0.0f)
            {
                offsetAmount = (end - start).magnitude * ChaosFactor;
            }

            while (generation-- > 0)
            {
                int previousStartIndex = startIndex;
                startIndex = segments.Count;
                for (int i = previousStartIndex; i < startIndex; i++)
                {
                    start = segments[i].Key;
                    end = segments[i].Value;

                    // determine a new direction for the split
                    Vector3 midPoint = (start + end) * 0.5f;

                    // adjust the mid point to be the new location
                    RandomVector(ref start, ref end, offsetAmount, out randomVector);
                    midPoint += randomVector;

                    // add two new segments
                    segments.Add(new KeyValuePair<Vector3, Vector3>(start, midPoint));
                    segments.Add(new KeyValuePair<Vector3, Vector3>(midPoint, end));
                }

                // halve the distance the lightning can deviate for each generation down
                offsetAmount *= 0.5f;
            }
        }

        public void RandomVector(ref Vector3 start, ref Vector3 end, float offsetAmount, out Vector3 result)
        {
                Vector3 directionNormalized = (end - start).normalized;
                Vector3 side;
                GetPerpendicularVector(ref directionNormalized, out side);

                // generate random distance
                float distance = (((float)RandomGenerator.NextDouble() + 0.1f) * offsetAmount);

                // get random rotation angle to rotate around the current direction
                float rotationAngle = ((float)RandomGenerator.NextDouble() * 360.0f);

                // rotate around the direction and then offset by the perpendicular vector
                result = Quaternion.AngleAxis(rotationAngle, directionNormalized) * side * distance;
            
        }

        private void SelectOffsetFromAnimationMode()
        {
            int index;

            if (AnimationMode == LightningBoltAnimationMode.None)
            {
                lineRenderer.material.mainTextureOffset = offsets[0];
                return;
            }
            else if (AnimationMode == LightningBoltAnimationMode.PingPong)
            {
                index = animationOffsetIndex;
                animationOffsetIndex += animationPingPongDirection;
                if (animationOffsetIndex >= offsets.Length)
                {
                    animationOffsetIndex = offsets.Length - 2;
                    animationPingPongDirection = -1;
                }
                else if (animationOffsetIndex < 0)
                {
                    animationOffsetIndex = 1;
                    animationPingPongDirection = 1;
                }
            }
            else if (AnimationMode == LightningBoltAnimationMode.Loop)
            {
                index = animationOffsetIndex++;
                if (animationOffsetIndex >= offsets.Length)
                {
                    animationOffsetIndex = 0;
                }
            }
            else
            {
                index = RandomGenerator.Next(0, offsets.Length);
            }

            if (index >= 0 && index < offsets.Length)
            {
                lineRenderer.material.mainTextureOffset = offsets[index];
            }
            else
            {
                lineRenderer.material.mainTextureOffset = offsets[0];
            }
        }

        private void UpdateLineRenderer()
        {
            int segmentCount = (segments.Count - startIndex) + 1;
            lineRenderer.positionCount = segmentCount;

            if (segmentCount < 1)
            {
                return;
            }

            int index = 0;
            lineRenderer.SetPosition(index++, segments[startIndex].Key);

            for (int i = startIndex; i < segments.Count; i++)
            {
                lineRenderer.SetPosition(index++, segments[i].Value);
            }

            segments.Clear();

            SelectOffsetFromAnimationMode();
        }



        /// <summary>
        /// Trigger a lightning bolt. Use this if ManualMode is true.
        /// </summary>
        public void Trigger()
        {
            Vector3 start, end;
            timer = Duration + Mathf.Min(0.0f, timer);
            if (StartObject == null)
            {
                start = StartPosition;
            }
            else
            {
                start = StartObject.transform.position + StartPosition;
            }
            if (EndObject == null)
            {
                end = EndPosition;
            }
            else
            {
                end = EndObject.transform.position + EndPosition;
            }
            startIndex = 0;
            GenerateLightningBolt(start, end, Generations, Generations, 0.0f);
            UpdateLineRenderer();
        }

        /// <summary>
        /// Call this method if you change the material on the line renderer
        /// </summary>
        public void UpdateFromMaterialChange()
        {
            size = new Vector2(1.0f / (float)Columns, 1.0f / (float)Rows);
            lineRenderer.material.mainTextureScale = size;
            offsets = new Vector2[Rows * Columns];
            for (int y = 0; y < Rows; y++)
            {
                for (int x = 0; x < Columns; x++)
                {
                    offsets[x + (y * Columns)] = new Vector2((float)x / Columns, (float)y / Rows);
                }
            }
        }

}
