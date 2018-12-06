using UnityEngine;

namespace Aspekt.PlayerController
{
    public class ShootAbility : PlayerAbility
    {
        public float CooldownTimer = 0f;
        public float ShootSpeed = 25f;
        public Modes Mode;
        public Bullet BulletPrefab;

        private IO.PlayerController controller;

        private float timeShot;

        public enum Modes
        {
            SingleShot, Automatic
        }

        private enum States
        {
            Ready, Cooldown
        }
        private States state;

        
        public void Init(Player player, IO.PlayerController playerController)
        {
            state = States.Ready;
            controller = playerController;
        }

        public void Activate()
        {
            if (state != States.Ready) return;

            if (CooldownTimer > 0)
            {
                timeShot = Time.time;
                state = States.Cooldown;
            }
            Shoot();
        }

        private void Update()
        {
            switch (state)
            {
                case States.Ready:
                    break;
                case States.Cooldown:
                    if (Time.time > timeShot + CooldownTimer)
                    {
                        state = States.Ready;
                    }
                    break;
                default:
                    break;
            }
        }

        private void Shoot()
        {
            var bullet = Instantiate(BulletPrefab);
            Vector2 direction = controller.GetDynamicDirection(Player.Instance.transform.position);

            if (direction == Vector2.zero)
            {
                direction = Player.Instance.IsFacingRight() ? Vector2.right : Vector2.left;
            }

            Vector2 origin = (Vector2)transform.position + direction.normalized * 0.5f;
            bullet.GetComponent<Bullet>().Fire(origin, direction, ShootSpeed);
        }

    }
}
