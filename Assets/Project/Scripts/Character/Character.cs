using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : GameUnit
{
    [Header("Character Detail")]
    [SerializeField] protected float moveSpeed = 5;
    [SerializeField] protected float rotationSpeed = 600;
    [SerializeField] protected float attackDelay = 0.5f;
    [SerializeField] protected float charSize = 1f;
    [SerializeField] protected string currentAnim;

    [Header("Constant")]
    public float MAX_SIZE = 4f;
    public float MIN_SIZE = 1f;
    public float BASE_ATTACK_RANGE = 5f;

    [Header("Component")]
    [SerializeField] protected Rigidbody rb;
    [SerializeField] protected new CapsuleCollider collider;
    [SerializeField] protected LayerMask groundLayer;
    [SerializeField] protected Skin charModel;
    protected Animator anim;

    [Header("Match Info")]
    public int score;
    public bool IsDead { get; protected set; }
    public bool IsCanAttack => currentWeapon.IsCanAttack;

    [Header("Target Info")]
    [SerializeField] protected Transform indicatorPoint;
    [SerializeField] protected GameObject maskTarget;
    [SerializeField] protected List<Character> targets = new List<Character>();
    protected Character curTarget;
    protected TargetIndicator indicator;
    private Vector3 targetPoint;

    protected WeaponBase currentWeapon;
    protected Hat currentHat;
    protected Accessory currentAcc;
    protected Skin currentSkin;

    #region Function

    public virtual void OnInit()
    {
        anim = charModel.anim;
        IsDead = false;
        score = 0;
        SetSize(MIN_SIZE);
        indicator = SimplePool.Spawn<TargetIndicator>(PoolType.TargetIndicator);
        indicator.SetTarget(indicatorPoint);
    }

    // Bat dau animation tan cong
    public virtual void OnAttack()
    {
        curTarget = GetTargetInRange();

        if (IsCanAttack && curTarget != null && !curTarget.IsDead)
        {
            targetPoint = curTarget.TF.position;
            TF.LookAt(targetPoint + (TF.position.y - targetPoint.y) * Vector3.up);
            ChangeAnim(Constants.ANIM_ATTACK);
        }
    }

    // Bi trung dan
    public virtual void OnHit()
    {
        if (!IsDead)
        {
            AudioManager.Ins.PlaySfx(Constants.EFF_HIT);
            IsDead = true;
            OnDeath();
        }
    }

    //Chuyen trang thai chet trong van dau
    public virtual void OnDeath()
    {
        ChangeAnim(Constants.ANIM_DIE);
    }

    //Ket thuc viec su dung nhan vat nay
    public virtual void OnDespawn()
    {
        //tra ve tat ca nhung object pool
        TakeOffSkin();
        SimplePool.Despawn(indicator);
    }

    //Hoi sinh lai
    public virtual void OnRespawn()
    {
        
    }

    #endregion

    #region Change Property

    //danh dau trong tam muc tieu
    public void SetMask(bool active)
    {
        maskTarget.SetActive(active);
    }

    public void ChangeAnim(string animName)
    {
        if (currentAnim != animName)
        {
            anim.ResetTrigger(animName);
            currentAnim = animName;
            anim.SetTrigger(currentAnim);
        }
    }

    #endregion

    #region Change Clothes

    public void ChangeSkin(SkinType skinType)
    {
        currentSkin = SimplePool.Spawn<Skin>((PoolType)skinType, TF);
        charModel = currentSkin;
        anim = charModel.anim;
    }

    public void ChangeWeapon(WeaponType weaponType)
    {
        currentWeapon = SimplePool.Spawn<WeaponBase>((PoolType)weaponType, charModel.rightHand);
    }

    public void ChangeAccessory(AccessoryType accessoryType)
    {
        if (!charModel.isFullSet && accessoryType != AccessoryType.ACC_None)
        {
            currentAcc = SimplePool.Spawn<Accessory>((PoolType)accessoryType, charModel.leftHand);
        }
    }

    public void ChangeHat(HatType hatType)
    {
        if (!charModel.isFullSet && hatType != HatType.HAT_None)
        {
            currentHat = SimplePool.Spawn<Hat>((PoolType)hatType, charModel.head);
        }
    }

    public void ChangePant(PantType pantType)
    {
        if (!charModel.isFullSet)
        {
            charModel.pant.material = charModel.materialData.GetMat(pantType);
        }
    }

    // Xoa cac item hien tai neu lua chon default
    public void DespawnHat()
    {
        if (currentHat) 
            SimplePool.Despawn(currentHat);
    }
    public void DespawnAccessory()
    {
        if (currentAcc) 
            SimplePool.Despawn(currentAcc);
    }

    public void DespawnWeapon()
    {
        if (currentWeapon) 
            SimplePool.Despawn(currentWeapon);
    }

    public virtual void TakeOffSkin()
    {
        DespawnHat();
        DespawnAccessory();
        SimplePool.Despawn(currentSkin);
    }

    // Ham thay toan bo do ( de trong cho Bot va Player )
    public virtual void ChangeClothes()
    {

    }

    #endregion

    #region Size

    protected virtual void SetSize(float size)
    {
        size = Mathf.Clamp(size, MIN_SIZE, MAX_SIZE);
        charSize = size;
        TF.localScale = charSize * Vector3.one;
    }

    protected virtual void UpSize()
    {
        float newSize = charSize + 0.1f;
        SetSize(newSize);
    }

    public void AddScore(int amount = 1)
    {
        SetScore(score + amount);
        ParticlePool.Play(Utilities.RandomInMember(ParticleType.LevelUp_1, ParticleType.LevelUp_2, ParticleType.LevelUp_3), TF.position, TF.rotation);
        UpSize();
    }

    protected virtual void SetScore(int score)
    {
        this.score = score > 0 ? score : 0;
        indicator.SetScore(this.score);
    }

    #endregion

    #region Action

    //them muc tieu trong tam danh
    public virtual void AddTarget(Character target)
    {
        targets.Add(target);
    }

    //xoas muc tieu khi roi khoi tam danh
    public virtual void RemoveTarget(Character target)
    {
        if(curTarget == target)
        {
            curTarget = null;
        }
        targets.Remove(target);
    }

    //tim muc tieu trong tam danh
    public Character GetTargetInRange()
    {
        Character target = null;
        float distance = float.PositiveInfinity;

        for (int i = 0; i < targets.Count; i++)
        {
            if (targets[i] != null && targets[i] != this && !targets[i].IsDead)
            {
                float dis = Vector3.Distance(TF.position, targets[i].TF.position);

                if (dis < distance && dis <= BASE_ATTACK_RANGE * charSize + targets[i].charSize)
                {
                    distance = dis;
                    target = targets[i];
                }
            }
        }

        return target;
    }

    protected void ClearTarget()
    {
        targets.Clear();
    }

    public void ThrowWeapon()
    {
        AudioManager.Ins.PlaySfx(Constants.EFF_THROW);
        currentWeapon.Throw(this, targetPoint, charSize);
    }

    #endregion

    #region Get component

    public CapsuleCollider GetCollider()
    {
        return collider;
    }

    public float GetAttackCountdown()
    {
        return attackDelay;
    }

    #endregion
}
