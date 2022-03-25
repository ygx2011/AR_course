/*************************************************************************************

 * 游研堂: www.gamedev3d.com

 *（1）本站致力于为广大的游戏从业者提供相关的资源素材与资讯。

 *（2）本站会持续更新更多相关的资源素材，为游戏领域开发者提供更好的资讯与灵感！

 *（3）本站所有资源素材仅供学习参考，切勿用作商业用途，并请在下载后的24小时内进行删除，

 *     否则由此引发的法律纠纷及连带责任本站和发布者概不承担。
 
*************************************************************************************/
using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

/// <summary>
/// Custom character controller, to be used by attaching the component to an object
/// and writing scripts attached to the same object that recieve the "SuperUpdate" message
/// </summary>
public class SuperCharacterController : MonoBehaviour
{
    [SerializeField]
    Vector3 debugMove = Vector3.zero;

    [SerializeField]
    bool fixedTimeStep;

    [SerializeField]
    int fixedUpdatesPerSecond;

    [SerializeField]
    bool debugSpheres;

    [SerializeField]
    bool debugPushbackMesssages;

    /// <summary>
    /// Describes the Transform of the object we are standing on as well as it's CollisionType, as well
    /// as how far the ground is below us and what angle it is in relation to the controller.
    /// </summary>
    [SerializeField]
    public struct Ground
    {
        public RaycastHit Hit;
        public RaycastHit NearHit;
        public RaycastHit FarHit;
        public SuperCollisionType CollisionType;
        public Transform Transform;

        public Ground(RaycastHit hit, RaycastHit nearHit, RaycastHit farHit, SuperCollisionType superCollisionType, Transform hitTransform)
        {
            Hit = hit;
            NearHit = nearHit;
            FarHit = farHit;
            CollisionType = superCollisionType;
            Transform = hitTransform;
        }
    }

    [SerializeField]
    CollisionSphere[] spheres =
        new CollisionSphere[3] {
            new CollisionSphere(0.5f, true, false),
            new CollisionSphere(1.0f, false, false),
            new CollisionSphere(1.5f, false, true),
        };

    public LayerMask Walkable;

    [SerializeField]
    Collider OwnCollider;

    public float radius = 0.5f;

    public float deltaTime { get; private set; }
    public Ground currentGround { get; private set; }
    public CollisionSphere feet { get; private set; }
    public CollisionSphere head { get; private set; }
    public float height { get { return Vector3.Distance(OffsetPosition(head.Offset), OffsetPosition(feet.Offset)); } }
    public Vector3 up { get { return transform.up; } }
    public Vector3 down { get { return -transform.up; } }
    public List<SuperCollision> collisionData { get; private set; }
    public Transform currentlyClampedTo { get; set; }

    private Vector3 initialPosition;
    private Vector3 groundOffset;
    private bool clamping = true;
    private bool slopeLimiting = true;

    private List<Collider> ignoredColliders;
    private List<IgnoredCollider> ignoredColliderStack;

    private const float Tolerance = 0.05f;
    private const float TinyTolerance = 0.01f;
    private const string TemporaryLayer = "TempCast";
    private int TemporaryLayerIndex;
    private float fixedDeltaTime;

    public void Awake()
    {
        collisionData = new List<SuperCollision>();

        TemporaryLayerIndex = LayerMask.NameToLayer(TemporaryLayer);

        ignoredColliders = new List<Collider>();
        ignoredColliderStack = new List<IgnoredCollider>();

        currentlyClampedTo = null;

        fixedDeltaTime = 1.0f / fixedUpdatesPerSecond;

        if (OwnCollider)
            IgnoreCollider(OwnCollider);

        foreach (var sphere in spheres)
        {
            if (sphere.IsFeet)
                feet = sphere;

            if (sphere.IsHead)
                head = sphere;
        }

        if (feet == null)
            Debug.LogError("[SuperCharacterController] Feet not found on controller");

        if (head == null)
            Debug.LogError("[SuperCharacterController] Head not found on controller");
			
		gameObject.SendMessage("SuperStart", SendMessageOptions.DontRequireReceiver);
    }

    void Update()
    {
        // If we are using a fixed timestep, ensure we run the main update loop
        // a sufficient number of times based on the Time.deltaTime

        if (!fixedTimeStep)
        {
            deltaTime = Time.deltaTime;

            SingleUpdate();
            return;
        }
        else
        {
            float delta = Time.deltaTime;

            while (delta > fixedDeltaTime)
            {
                deltaTime = fixedDeltaTime;

                SingleUpdate();

                delta -= fixedDeltaTime;
            }

            if (delta > 0f)
            {
                deltaTime = delta;

                SingleUpdate();
            }
        }
    }

    void SingleUpdate()
    {
        // Check if we are clamped to an object implicity or explicity
        bool isClamping = clamping || currentlyClampedTo != null;
        Transform clampedTo = currentlyClampedTo != null ? currentlyClampedTo : currentGround.Transform;

        // Move our controller if clamped object moved in the previous frame
        if (isClamping && groundOffset != Vector3.zero && clampedTo != null)
            transform.position = clampedTo.position + groundOffset;

        initialPosition = transform.position;

        ProbeGroundRecursive();

        transform.position += debugMove * deltaTime;

        gameObject.SendMessage("SuperUpdate", SendMessageOptions.DontRequireReceiver);

        Pushback();

        ProbeGroundRecursive();

        if (slopeLimiting)
            SlopeLimit();

        ProbeGroundRecursive();

        if (clamping)
            ClampToGround();

        isClamping = clamping || currentlyClampedTo != null;
        clampedTo = currentlyClampedTo != null ? currentlyClampedTo : currentGround.Transform;

        if (isClamping)
            groundOffset = transform.position - clampedTo.position;

    }

    /// <summary>
    /// Prevents the player from walking up slopes of a larger angle than the object's SlopeLimit.
    /// NOTE: Since ProbeGroundRecursive ignores any slopes greater than StandAngle, the controller
    /// will not be slope limited against these slopes.
    /// </summary>
    /// <returns>True if the controller attemped to ascend a too steep slope and had their movement limited</returns>
    bool SlopeLimit()
    {
        Vector3 n = currentGround.Hit.normal;
        float a = Vector3.Angle(n, up);

        if (a > currentGround.CollisionType.SlopeLimit)
        {
            Vector3 absoluteMoveDirection = Math3d.ProjectVectorOnPlane(n, transform.position - initialPosition);

            // Retrieve a vector pointing down the slope
            Vector3 r = Vector3.Cross(n, down);
            Vector3 v = Vector3.Cross(r, n);

            float angle = Vector3.Angle(absoluteMoveDirection, v);

            if (angle <= 90.0f)
                return false;

            // Calculate where to place the controller on the slope, or at the bottom, based on the desired movement distance
            Vector3 resolvedPosition = Math3d.ProjectPointOnLine(initialPosition, r, transform.position);
            Vector3 direction = Math3d.ProjectVectorOnPlane(n, resolvedPosition - transform.position);

            RaycastHit hit;

            // Check if our path to our resolved position is blocked by any colliders
            if (Physics.CapsuleCast(OffsetPosition(feet.Offset), OffsetPosition(head.Offset), radius, direction.normalized, out hit, direction.magnitude, Walkable))
            {
                transform.position += v.normalized * hit.distance;
            }
            else
            {
                transform.position += direction;
            }

            return true;
        }

        return false;
    }

    void ClampToGround()
    {
        float d = currentGround.Hit.distance;
        transform.position -= up * d;
    }

    public void EnableClamping()
    {
        clamping = true;
    }

    public void DisableClamping()
    {
        clamping = false;
    }

    public void EnableSlopeLimit()
    {
        slopeLimiting = true;
    }

    public void DisableSlopeLimit()
    {
        slopeLimiting = false;
    }

    public bool IsClamping()
    {
        return clamping;
    }

    /// <summary>
    /// SphereCasts directly below the controller recurisvely until it either finds no ground, or a ground
    /// at an angle less than StandAngle
    /// </summary>
    void ProbeGroundRecursive()
    {
        ProbeGroundRecursive(OffsetPosition(feet.Offset), 0);
    }

    void ProbeGroundRecursive(Vector3 origin, float distanceTraveled)
    {
        PushIgnoredColliders();

        // Add a small amount of Tolerance before casting downwards
        Vector3 o = origin + (up * Tolerance);
        
        RaycastHit hit;

        if (Physics.SphereCast(o, radius, down, out hit, Mathf.Infinity, Walkable))
        {
            var wall = hit.collider.gameObject.GetComponent<SuperCollisionType>();

            if (wall == null)
            {
                // TODO: just use some derived default values?
                Debug.LogError("[SuperCharacterComponent]: Object on SuperCharacterController walkable layer does not have SuperCollisionType component attached");
            }

            Vector3 newOrigin = o + down * (hit.distance + TinyTolerance + Tolerance);

            hit.distance = Mathf.Clamp(hit.distance - Tolerance, 0, Mathf.Infinity);

            hit.distance += distanceTraveled;

            // If the StandAngle is not satisfactory, adjust our origin to be slightly below where we last hit
            // and SphereCast again
            if (Vector3.Angle(hit.normal, up) > wall.StandAngle)
            {
                PopIgnoredColliders();

                ProbeGroundRecursive(newOrigin, hit.distance + TinyTolerance);
                return;
            }
            
            // Because when SphereCast hits an edge on a surface it returns the interpolation of the two normals of the
            // two triangles joined to that edge, we need to retrieve the actual normal of both of the triangles
            Vector3 toCenter = Math3d.ProjectVectorOnPlane(up, (transform.position - hit.point).normalized * TinyTolerance);

            if (toCenter == Vector3.zero)
            {
                currentGround = new Ground(hit, hit, hit, wall, hit.transform);
                PopIgnoredColliders();

                return;
            }
            
            Vector3 awayFromCenter = Quaternion.AngleAxis(-80.0f, Vector3.Cross(toCenter, up)) * -toCenter;

            Vector3 nearPoint = hit.point + toCenter + (up * TinyTolerance);
            Vector3 farPoint = hit.point + (awayFromCenter * 3);

            RaycastHit nearHit;
            RaycastHit farHit;

            // Retrieve the normal of the point nearest to the center of the base of the controller
            Physics.Raycast(nearPoint, down, out nearHit, Mathf.Infinity, Walkable);
            // Retrieve the normal of the point furthest to the center of the base of the controller
            Physics.Raycast(farPoint, down, out farHit, Mathf.Infinity, Walkable);

            currentGround = new Ground(hit, nearHit, farHit, wall, hit.transform);
        }
        else
        {
            // Debug.LogError("[SuperCharacterComponent]: No ground was found below the player; player has escaped level");
        }

        PopIgnoredColliders();
    }

    /// <summary>
    /// Check if any of the CollisionSpheres are colliding with any walkable objects in the world.
    /// If they are, apply a proper pushback and retrieve the collision data
    /// </summary>
    void Pushback()
    {
        PushIgnoredColliders();

        collisionData.Clear();

        foreach (var sphere in spheres)
        {
            foreach (Collider col in Physics.OverlapSphere(OffsetPosition(sphere.Offset), radius, Walkable))
            {
                Vector3 position = OffsetPosition(sphere.Offset);
                Vector3 contactPoint = SuperCollider.ClosestPointOnSurface(col, position, radius);

                if (contactPoint != Vector3.zero)
                {
                    if (debugPushbackMesssages)
                        DebugDraw.DrawMarker(contactPoint, 2.0f, Color.cyan, 0.0f, false);

                    Vector3 v = contactPoint - position;

                    if (v != Vector3.zero)
                    {
                        // Cache the collider's layer so that we can cast against it
                        int layer = col.gameObject.layer;

                        col.gameObject.layer = TemporaryLayerIndex;

                        // Check which side of the normal we are on
                        bool facingNormal = Physics.SphereCast(new Ray(position, v.normalized), TinyTolerance, v.magnitude + TinyTolerance, 1 << TemporaryLayerIndex);

                        col.gameObject.layer = layer;

                        // Orient and scale our vector based on which side of the normal we are situated
                        if (facingNormal)
                        {
                            if (Vector3.Distance(position, contactPoint) < radius)
                            {
                                v = v.normalized * (radius - v.magnitude) * -1;
                            }
                            else
                            {
                                // A previously resolved collision has had a side effect that moved us outside this collider
                                continue;
                            }
                        }
                        else
                        {
                            v = v.normalized * (radius + v.magnitude);
                        }

                        transform.position += v;

                        col.gameObject.layer = TemporaryLayerIndex;

                        // Retrieve the surface normal of the collided point
                        RaycastHit normalHit;

                        Physics.SphereCast(new Ray(position + v, contactPoint - (position + v)), TinyTolerance, out normalHit, 1 << TemporaryLayerIndex);

                        col.gameObject.layer = layer;

                        // Our collision affected the collider; add it to the collision data
                        var collision = new SuperCollision()
                        {
                            collisionSphere = sphere,
                            superCollisionType = col.gameObject.GetComponent<SuperCollisionType>(),
                            gameObject = col.gameObject,
                            point = contactPoint,
                            normal = normalHit.normal
                        };

                        collisionData.Add(collision);
                    }
                }
            }
        }
        
        PopIgnoredColliders();
    }

    protected struct IgnoredCollider
    {
        public Collider collider;
        public int layer;

        public IgnoredCollider(Collider collider, int layer)
        {
            this.collider = collider;
            this.layer = layer;
        }
    }

    private void PushIgnoredColliders()
    {
        ignoredColliderStack.Clear();

        for (int i = 0; i < ignoredColliders.Count; i++)
        {
            Collider col = ignoredColliders[i];
            ignoredColliderStack.Add(new IgnoredCollider(col, col.gameObject.layer));
            col.gameObject.layer = TemporaryLayerIndex;
        }
    }

    private void PopIgnoredColliders()
    {
        for (int i = 0; i < ignoredColliderStack.Count; i++)
        {
            IgnoredCollider ic = ignoredColliderStack[i];
            ic.collider.gameObject.layer = ic.layer;
        }

        ignoredColliderStack.Clear();
    }

    void OnDrawGizmos()
    {
        if (debugSpheres)
        {
            if (spheres != null)
            {
                foreach (var sphere in spheres)
                {
                    Gizmos.color = sphere.IsFeet ? Color.green : (sphere.IsHead ? Color.yellow : Color.cyan);
                    Gizmos.DrawWireSphere(OffsetPosition(sphere.Offset), radius);
                }
            }
        }
    }

    public Vector3 OffsetPosition(float y)
    {
        Vector3 p;

        p = transform.position;

        p += up * y;

        return p;
    }

    public bool BelowHead(Vector3 point)
    {
        return Vector3.Angle(point - OffsetPosition(head.Offset), up) > 89.0f;
    }

    public bool AboveFeet(Vector3 point)
    {
        return Vector3.Angle(point - OffsetPosition(feet.Offset), down) > 89.0f;
    }

    public void IgnoreCollider(Collider col)
    {
        ignoredColliders.Add(col);
    }

    public void RemoveIgnoredCollider(Collider col)
    {
        ignoredColliders.Remove(col);
    }

    public void ClearIgnoredColliders()
    {
        ignoredColliders.Clear();
    }
}

[Serializable]
public class CollisionSphere
{
    public float Offset;
    public bool IsFeet;
    public bool IsHead;

    public CollisionSphere(float offset, bool isFeet, bool isHead)
    {
        Offset = offset;
        IsFeet = isFeet;
        this.IsHead = isHead;
    }
}

public struct SuperCollision
{
    public CollisionSphere collisionSphere;
    public SuperCollisionType superCollisionType;
    public GameObject gameObject;
    public Vector3 point;
    public Vector3 normal;
}
