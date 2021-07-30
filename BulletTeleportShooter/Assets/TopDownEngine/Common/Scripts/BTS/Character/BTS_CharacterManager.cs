using UnityEngine;

namespace MoreMountains.TopDownEngine
{
    public class BTS_CharacterManager : MonoBehaviour
    {
        public Weapon initialWeapon;
        private CharacterHandleWeapon _characterHandleWeapon;
        private Explosion _explosion;
        private Health _health;
        private ProjectileWeapon _projectileWeapon;
        private Weapon _weapon;
        private CharacterMovement _characterMovement;

        [Header("Health")]
        [Tooltip("캐릭터 최대 체력")]
        public int MaximumHealth;
        [Tooltip("캐릭터 시작 체력")]
        public int InitialHealth;

        [Header("Attack Stat")]
        [Tooltip("캐릭터 무기 공격 데미지")]
        //-1이면, 기본 데미지로 적용 (Projectile.cs -> DamageCaused = 10)
        public int AttackDMG;             
        [Tooltip("캐릭터 연사 딜레이 시간 (연사 속도)")]
        //기본 : 1f (1초마다)
        public float ShootDelay;

        [Header("Movement")]
        [Tooltip("캐릭터 이동 속도")]
        //기본 : 6f
        public float WalkSpeed;

        [Header("Flash Stat")]
        [Tooltip("텔레포트 토큰 최대치")]
        public int MaxTeleportToken;
        [Tooltip("점멸 데미지")]
        public int FlashDMG;
        [Tooltip("과부하 범위")]
        public int FlashRange;


        void Awake()
        {
            _characterHandleWeapon = GetComponent<CharacterHandleWeapon>();
            _characterHandleWeapon.InitialWeapon = initialWeapon;

            _health = GetComponent<Health>();
            _projectileWeapon = initialWeapon.GetComponent<ProjectileWeapon>();
            _weapon = initialWeapon;
            _characterMovement = GetComponent<CharacterMovement>();
            _explosion = GetComponent<Explosion>();


            //기존 스크립트와 스텟 연결
            _health.InitialHealth = InitialHealth;
            _health.MaximumHealth = MaximumHealth;
            _projectileWeapon.projectileDamage = AttackDMG;
            _weapon.TimeBetweenUses = ShootDelay;
            _weapon.TimeBetweenUsesReleaseInterruption = false;
            _characterMovement.WalkSpeed = WalkSpeed;
            _explosion.Radius = FlashRange;
        }     
    }
}