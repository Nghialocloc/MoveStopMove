using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Bot : Character
{
    [Header("NavMesh")]
    public NavMeshAgent agent;
    private Vector3 destionation;

    [Header("Bot Component")]
    [SerializeField] private IState<Bot> currentState;
    public TimeCounter botCounter = new TimeCounter();

    public bool IsInMatch => (GameManager.Ins.IsState(GameState.Gameplay) || GameManager.Ins.IsState(GameState.Revive));

    public bool IsDestination => Vector3.Distance(TF.position, destionation) - Mathf.Abs(TF.position.y - destionation.y) < 0.1f;

    // Update is called once per frame
    private void Update()
    {
        if (IsInMatch && currentState != null && !IsDead)
        {
            currentState.OnExecute(this);
        }
        else if (!IsDead)
        {
            ChangeState(new IdleState());
        }
    }

    #region Function

    public override void OnInit()
    {
        ChangeClothes();
        base.OnInit();
        SetMask(false);
        if (agent == null)
            return;
        agent.speed = moveSpeed;
        indicator.SetName(NameUtilities.GetRandomName());
    }

    public override void OnHit()
    {
        base.OnHit();
    }

    public override void OnDeath()
    {
        ChangeState(null);
        StopMovement();
        base.OnDeath();
        SetMask(false);
        Invoke(nameof(OnDespawn), 1.5f);
    }

    public override void OnDespawn()
    {
        base.OnDespawn();
        SimplePool.Despawn(this);
        LevelManager.Ins.CharecterDeath(this);
        CancelInvoke();
    }

    public override void OnRespawn()
    {
        base.OnRespawn();
        OnInit();
        int level =Random.Range(LevelManager.Ins.currentPlayer.score, LevelManager.Ins.currentPlayer.score + 3);
        SetScore(level);
        SetSize((level * 0.1f) + MIN_SIZE);
    }

    #endregion

    #region Movement

    //set diem den
    public void SetDestination(Vector3 position)
    {
        agent.enabled = true;
        destionation = position;
        agent.SetDestination(position);
        ChangeAnim(Constants.ANIM_RUN);
    }

    public void StopMovement()
    {
        agent.enabled = false;
    }

    #endregion

    #region Change Clothes

    public override void ChangeClothes()
    {
        base.ChangeClothes();
        SkinType chooseSkin = Utilities.RandomEnumValue<SkinType>();
        if (chooseSkin != SkinType.SKIN_None)
        {
            if (Utilities.Chance(25))
            {
                ChangeSkin(chooseSkin);
            }
            else
            {
                ChangeSkin(SkinType.SKIN_None);
                ChangeHat(Utilities.RandomEnumValue<HatType>());
                ChangeAccessory(Utilities.RandomEnumValue<AccessoryType>());
                ChangePant(Utilities.RandomEnumValue<PantType>());
            }
        }
        else
        {
            ChangeSkin(chooseSkin);
            ChangeHat(Utilities.RandomEnumValue<HatType>());
            ChangeAccessory(Utilities.RandomEnumValue<AccessoryType>());
            ChangePant(Utilities.RandomEnumValue<PantType>());
        }
        ChangeWeapon(Utilities.RandomEnumValue<WeaponType>());
    }

    #endregion

    #region Size

    protected override void SetSize(float size)
    {
        base.SetSize(size);
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
        if (!IsDead && IsInMatch)
        {
            ChangeState(new AttackState());
        }
    }

    #endregion

    #region Change State

    public void ChangeState(IState<Bot> state)
    {
        // Kiem tra xem state cu co bang null ko. Neu khong thi loai bo
        if (currentState != null)
        {
            currentState.OnExit(this);
        }

        currentState = state;

        //Neu current state khac null, bat dau truy cap vao cac ham state moi
        if (currentState != null)
        {
            currentState.OnEnter(this);
        }
    }

    #endregion
}
