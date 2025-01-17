using UnityEngine;


public class Kuze : APlayer
{

    //TEMPORARY, HACKY ANIMATION CONTROLLER FOR PITCH
    //TODO (ben): FIX THIS TRASH
    // TODO(mish question): This should be refactored? Maybe have an automatic animation
    // loader.
    private Animator _anim;
    // Sample Abilities
    private MoveFastAbility _moveFastAbility;
    private BlinkAbility _blinkAbility;
    private DashAbility _dashAbility;

    private static readonly int IsFiring = Animator.StringToHash("isFiring");
    private static readonly int VelX = Animator.StringToHash("velX");
    private static readonly int VelY = Animator.StringToHash("velY");
    private static readonly int IsMoving = Animator.StringToHash("isMoving");
    private static readonly int LookAtCamera = Animator.StringToHash("lookAtCamera");
    private static readonly int IsAiming = Animator.StringToHash("isAiming");
    private static readonly int InAir = Animator.StringToHash("inAir");
    private static readonly int DistToGround = Animator.StringToHash("distToGround");
    private static readonly int JumpDown = Animator.StringToHash("jumpDown");
    private static readonly int IsFalling = Animator.StringToHash("isFalling");

    protected override void StartPlayer()
    {
        // TODO(ben): Will this be player specific or general for all the
        // players?
        _anim = base.GetComponentInChildren<Animator>();

        // NEW: GIVING ABILITIES TO KUZE
        // base.RegisterAbility(_moveFastAbility = new MoveFastAbility(this));

        // NEW: Blink ability where you can blink every 45 seconds. To blink, call onEnable.
        base.RegisterAbility(_blinkAbility = new BlinkAbility(this, 45f));

        // NEW: This uses the default dash cooldown and cast delay
        base.RegisterAbility(_dashAbility = new DashAbility(this, characterData.dashAbilityCooldownTime, characterData.dashAbilityTime));
    }

    protected override void UpdatePlayer()
    {
        base.UpdatePlayer();
        
        // Implement Kuze specific update code here

    }

    protected override void HandlePlayerInputs()
    {
        HandleAnimationInputs();

        // Implement other Kuze specific inputs here

        // If you press a specific key, call this function to toggle the ability
        // _moveFastAbility.Toggle();
    }

    protected override void UseAbility1()
    {
        base.UseAbility1();
        _dashAbility.Enable();
    }

    protected override void UseAbility2()
    {
        base.UseAbility2();
    }

    // Other players could have different animations.
    private void HandleAnimationInputs()
    {
        if (!_anim) return;
        _anim.SetBool(IsFiring, isFiring);
        _anim.SetBool(IsAiming, isAiming);
        _anim.SetBool(JumpDown, _jumpedThisFrame);
        _anim.SetBool(InAir, !motor.GroundingStatus.IsStableOnGround);
        _anim.SetBool(IsFalling, !motor.GroundingStatus.IsStableOnGround && motor.Velocity.y <= -.05f);
        if (_anim.GetBool(InAir) || _anim.GetBool(IsFalling))
        {
            Ray r = new Ray(transform.position, Vector3.down);
            if (Physics.Raycast(r, out var hit, 500f, 1 << LayerMask.NameToLayer("Default")))
            {
                _anim.SetFloat(DistToGround, hit.distance);
            }
        }
        _anim.SetFloat(VelX, Mathf.MoveTowards(_anim.GetFloat(VelX), inputVector.x, .025f));
        _anim.SetFloat(VelY, Mathf.MoveTowards(_anim.GetFloat(VelY), inputVector.z, .025f));
        _anim.SetBool(IsMoving, inputVector.magnitude > 0.5f);
        _anim.SetBool(LookAtCamera, orientationMethod == OrientationMethod.TowardsCamera);
    }
}