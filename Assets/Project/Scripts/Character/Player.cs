using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character, IDataPersistence
{
    [Header("Player Component")]
    [SerializeField] private Joystick playerJoy;
    [SerializeField] private ParticleSystem reviveVFX;
    public bool isRevive;
    private bool isMoving;
    private bool isAttack;

    private TimeCounter playerCounter = new TimeCounter();

    [HideInInspector] public WeaponType curWeaponType;
    [HideInInspector] public HatType curHatType;
    [HideInInspector] public PantType curPantType;
    [HideInInspector] public AccessoryType curAccType;
    [HideInInspector] public SkinType curSkinType;

    #region Load and Save

    public void LoadData(GameData data)
    {
        curWeaponType = data.equipedWeapon;
        curHatType = data.equipedHat;
        curPantType = data.equipedPant;
        curAccType = data.equipedAcc;
        curSkinType = data.equipedSkin;
    }

    public void SaveData(ref GameData data)
    {
        data.equipedWeapon = curWeaponType;
        data.equipedHat = curHatType;
        data.equipedPant = curPantType;
        data.equipedAcc = curAccType;
        data.equipedSkin = curSkinType;
    }

    #endregion

    public void Start()
    {
        OnInit();
    }

    void Update()
    {
        if (!IsDead)
        {
            HandleMovement();  
        }
        HandleInput();
    }

    #region Function

    public override void OnInit()
    {
        DataPersistenceManager.Ins.CallData(this);
        ChangeClothes();
        base.OnInit();
        isMoving = false;
        isAttack = false;
        isRevive = false;
        SetMask(false);
        TF.rotation = Quaternion.Euler(Vector3.up * 180);
        indicator.SetName("You");
    }

    public override void OnAttack()
    {
        base.OnAttack();
        if (curTarget != null && IsCanAttack)
        {
            playerCounter.Assign(ThrowWeapon, 0.5f);
            Invoke(nameof(ResetAnim), 0.5f);
        }
        else
        {
            isAttack = false;
        }
    }

    public override void OnDeath()
    {
        base.OnDeath();
        playerCounter.Cancel();
        LevelManager.Ins.CharecterDeath(this);
    }

    public override void OnDespawn()
    {
        base.OnDespawn();
    }

    public override void OnRespawn()
    {
        base.OnRespawn();
        ChangeAnim(Constants.ANIM_IDLE);
        IsDead = false;
        ClearTarget();
        reviveVFX.Play();
        AudioManager.Ins.PlaySfx(Constants.EFF_REVIVE);
    }

    #endregion

    #region Movement

    private void HandleMovement()
    {
        float joyHorizontalMove = playerJoy.Horizontal * moveSpeed;
        float joyVerticalMove = playerJoy.Vertical * moveSpeed;

        if (GameManager.Ins.IsState(GameState.Gameplay) && !IsDead)
        {
            if (Input.GetMouseButtonDown(0))
            {
                playerCounter.Cancel();
            }

            if (Input.GetMouseButton(0))
            {
                Vector3 joyMovement = new Vector3(joyHorizontalMove, 0, joyVerticalMove);
                Quaternion Rotate = Quaternion.LookRotation(joyMovement, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Rotate, rotationSpeed * Time.deltaTime);

                if (joyMovement != Vector3.zero)
                {
                    rb.velocity = joyMovement.normalized * moveSpeed;
                    ChangeAnim(Constants.ANIM_RUN);
                    isMoving = true;
                }
            }
            else
            {
                playerCounter.Count();
            }

            if (Input.GetMouseButtonUp(0))
            {
                isAttack = true;
                isMoving = false;
                StopPlayerMovement();
                OnAttack();
            }

            if (!isAttack && !isMoving)
            {
                ResetAnim();
            }
        }
    }

    private void HandleInput()
    {
        if (!GameManager.Ins.IsState(GameState.Gameplay) || IsDead)
        {
            playerJoy.gameObject.SetActive(false);
        }
        else
        {
            playerJoy.gameObject.SetActive(true);
        }

    }

    public void StopPlayerMovement()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    #endregion

    #region Change Clothes

    public override void ChangeClothes()
    {
        base.ChangeClothes();

        if(curSkinType != SkinType.SKIN_None)
        {
            ChangeSkin(curSkinType);
        }
        else
        {
            ChangeSkin(curSkinType);
            ChangeHat(curHatType);
            ChangeAccessory(curAccType);
            ChangePant(curPantType);
        }

        ChangeWeapon(curWeaponType);
    }

    #endregion

    #region Size

    protected override void SetSize(float size)
    {
        base.SetSize(size);
        GameManager.Ins.camera.SetRateOffset((this.charSize - MIN_SIZE) / ((MAX_SIZE - MIN_SIZE) * 2));
    }

    protected override void UpSize()
    {
        base.UpSize();
    }

    protected override void SetScore(int score)
    {
        base.SetScore(score);
    }

    #endregion

    #region Action
    public override void AddTarget(Character target)
    {
        base.AddTarget(target);

        if (!target.IsDead && !IsDead)
        {
            target.SetMask(true);
            // Neu khong di chuyen va chua dem thoi gian tan cong
            if (!isMoving && !playerCounter.IsAlreadyAssign)
            {
                OnAttack();
            }
        }
    }

    public override void RemoveTarget(Character target)
    {
        base.RemoveTarget(target);
        target.SetMask(false);
    }

    public void ResetAnim()
    {
        ChangeAnim(Constants.ANIM_IDLE);
    }

    #endregion
}
