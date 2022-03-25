/*************************************************************************************

 * 游研堂: www.gamedev3d.com

 *（1）本站致力于为广大的游戏从业者提供相关的资源素材与资讯。

 *（2）本站会持续更新更多相关的资源素材，为游戏领域开发者提供更好的资讯与灵感！

 *（3）本站所有资源素材仅供学习参考，切勿用作商业用途，并请在下载后的24小时内进行删除，

 *     否则由此引发的法律纠纷及连带责任本站和发布者概不承担。
 
*************************************************************************************/
using UnityEngine;
using System.Collections;

public partial class MarioMachine : SuperStateMachine {

    public bool DebugGui;

    public float InitialRotation = 0.0f;

    public MarioVerySmartCamera SmartCamera;

    public Transform AnimatedMesh;
    public AdditiveTransform ChestBone;
    public ParticleSystem RunSmokeEffect;
    public GameObject GroundPoundSmokeEffect;
    public GameObject WallKickSmokeEffect;
    public GameObject GroundFallSmokeEffect;
    public GameObject WallHitStarEffect;
    public GameObject EnemyHitEffect;
    public GameObject TakeDamageEffect;
    public GameObject UpgradeEffect;
    public Transform ScaleAnimator;

    public LayerMask EnemyLayerMask;

    public float runSpeed = 7.0f;
    public float turnSpeed = 10.0f;
    public float maxSlideSpeed = 26.0f;
    public float jumpDamageHeight = 20.0f;
    public float acquireGroundDistance = 0.05f;
    public float maintainGroundDistance = 0.5f;

    private Animation anim;

    private MarioInput input;
    private MarioStatus status;
    private MarioSound sound;
    private SuperCharacterController controller;
    private ShaderSwapper transparencyShaderSwapper;
    private MaterialSwapper goldMaterialSwapper;
    private TransparencyFade transparencyFade;

    private Vector3 moveDirection;
    private Vector3 lookDirection;
    private Vector3 artUpDirection;

    private JumpProfile currentJumpProfile;

    private float moveSpeed;
    private float verticalMoveSpeed;
    private float lastLandTime;

    private float chestBendAngle;
    public float chestTwistAngle;

    private bool isTakingFallDamage;
    private bool goldMario;

    private JumpProfile jumpStandard = new JumpProfile
    {
        CanDive = true,
        CanKick = true,
        CanControlHeight = true,
        JumpHeight = 2.0f,
        Animation = "jump"
    };

    private JumpProfile jumpDouble = new JumpProfile
    {
        CanDive = true,
        CanKick = true,
        CanControlHeight = true,
        JumpHeight = 3.6f,
        Animation = "double_jump",
        FallAnimation = "fall"
    };

    private JumpProfile jumpTriple = new JumpProfile
    {
        CanDive = true,
        JumpHeight = 6.0f,
        Animation = "triple_jump"
    };

    private JumpProfile jumpBackFlip = new JumpProfile
    {
        JumpHeight = 5.0f,
        InitialForwardVelocity = -7.0f,
        Animation = "backflip"
    };

    private JumpProfile jumpSideFlip = new JumpProfile
    {
        CanDive = true,
        JumpHeight = 5.0f,
        Animation = "side_flip_v2"
    };

    private JumpProfile jumpLong = new JumpProfile
    {
        JumpHeight = 2.7f,
        InitialForwardVelocity = 4.0f,
        Gravity = 17.0f,
        Animation = "long_jump",
        CrossFadeTime = 0.1f
    };

    private JumpProfile jumpWall = new JumpProfile
    {
        CanDive = true,
        CanControlHeight = true,
        JumpHeight = 5.0f,
        InitialForwardVelocity = 7.4f,
        Animation = "wall_kick",
        FallAnimation = "wall_kick_fall"
    };

    private JumpProfile jumpKick = new JumpProfile
    {
        JumpHeight = 1.0f,
        Animation = "jump_kick",
        Gravity = 45.0f
    };

    private JumpProfile fallProfile = new JumpProfile
    {
    };
}
