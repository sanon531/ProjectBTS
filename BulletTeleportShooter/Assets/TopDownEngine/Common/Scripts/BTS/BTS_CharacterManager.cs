using UnityEngine;


namespace MoreMountains.TopDownEngine
{ 
    public class BTS_CharacterManager : MonoBehaviour
    {
        public Weapon initialWeapon;
        private CharacterHandleWeapon _characterHandleWeapon;

        //Health.cs -> int Initial/Maximum Health
        private Health _health;                     
        public int health;

        //ProjectileWeapon.cs -> int projectileDamage
        private ProjectileWeapon _projectileWeapon;
        public int attackDMG;           //-1이면, 기본 데미지로 적용 (Projectile.cs -> DamageCaused = 10)

        //Weapon.cs -> float TimeBetweenUses 
        private Weapon _weapon;                      
        public float shootDelay;        //기본 : 1f (1초마다)

        //CharacterMovement.cs -> float WalkSpeed
        private CharacterMovement _characterMovement;
        public float walkSpeed;         //기본 : 6f

        public int MaxOverload;     //과부하 최대치(new)
        public int flashDMG;        //점멸 데미지(new)
        public int flashRange;      //점멸 범위(new)

        

        void Awake()
        {
            _characterHandleWeapon = GetComponent<CharacterHandleWeapon>();
            _characterHandleWeapon.InitialWeapon = initialWeapon;

            _health = GetComponent<Health>();
            _projectileWeapon = initialWeapon.GetComponent<ProjectileWeapon>();
            _weapon = initialWeapon.GetComponent<Weapon>();
            _characterMovement = GetComponent<CharacterMovement>();
            
            //기존 스크립트와 스텟 연결
            _health.InitialHealth = health;
            _health.MaximumHealth = health;
            _projectileWeapon.projectileDamage = attackDMG;
            _weapon.TimeBetweenUses = shootDelay;
            _weapon.TimeBetweenUsesReleaseInterruption = false;
            _characterMovement.WalkSpeed = walkSpeed;
        }

       
    }
}