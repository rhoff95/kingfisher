using UnityEngine;

namespace Actors.Enemies
{
    public class EnemyPlane : Enemy
    {
        [Range(0f, 90f)] public float angleThreshold;
        [Range(0f, 100f)] public float accelerationRange;
        [Range(0f, 100f)] public float closeupRange;
        [Range(0f, 100f)] public float attackRange;
        public float waterLevel;

        private Color _thrustMode;

        private void Update()
        {
            var playerPosition = ActorTransform.position;
            var position = transform.position;
            var toPlayer = playerPosition - position;

            var right = Actor.Rb.transform.right;

            var angle = Vector3.SignedAngle(right, toPlayer, Vector3.forward);
            var angleAbs = Mathf.Abs(angle);

            if (angleAbs > angleThreshold)
            {
                Actor.SetRotationInput(Mathf.Sign(angle));
            }
            else
            {
                Actor.SetRotationInput(0f);
            }

            var distanceToPlayerSqr = toPlayer.sqrMagnitude;

            if (distanceToPlayerSqr > accelerationRange * accelerationRange && angleAbs < 40f)
            {
                _thrustMode = Color.green;
                Actor.SetThrustActive(true);
            }
            else if (transform.position.y < waterLevel && Actor.Rb.rotation is > 20f and < 160f)
            {
                _thrustMode = Color.blue;
                Actor.SetThrustActive(true);
            }
            else if (distanceToPlayerSqr < closeupRange * closeupRange)
            {
                _thrustMode = Color.magenta;
                Actor.SetThrustActive(true);
            }
            else
            {
                _thrustMode = Color.red;
                Actor.SetThrustActive(false);
            }

            if (angleAbs < 40f && distanceToPlayerSqr < attackRange * attackRange)
            {
                Actor.StartFiring();
            }
            else
            {
                Actor.StopFiring();
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, accelerationRange);
            Gizmos.DrawWireSphere(transform.position, closeupRange);
            Gizmos.color = _thrustMode;
            Gizmos.DrawWireSphere(transform.position, 1f);
        }
    }
}