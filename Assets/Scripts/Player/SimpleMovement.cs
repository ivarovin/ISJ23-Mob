using System;
using Core;
using UnityEngine;

namespace Player
{
    public class SimpleMovement : MonoBehaviour
    {
        private Vector2 movement;
        
        [Header("Components")]
        [SerializeField] private Rigidbody2D rb2d;
        [SerializeField] private Animator dayAnim;
        [SerializeField] private GameObject dayPlayer;
        [SerializeField] private Animator nigthAnim;
        [SerializeField] private GameObject nigthPlayer;
        
        [Header("Values")]
        public float speed;
        public float normalSpeed;
        [SerializeField] private float runAccelAmount;
        [SerializeField] private float runDeccelAmount;
        [SerializeField] private bool isDay;

        [HideInInspector]
        public Vector2 faceDirection;

        private void Start()
        {
            normalSpeed = speed;
        }

        // Update is called once per frame
        void Update()
        {
            if (GameStateController.Instance.gameState == GameStateController.GameState.Gameplay)
            {
                PlayerInputsValues();
            }
            
            PlayerAnimationController();
        }

        private void FixedUpdate()
        {
            if (GameStateController.Instance.gameState == GameStateController.GameState.Gameplay)
            {
                CanMove();
            }
            else
            {
                rb2d.velocity = Vector2.zero;
                movement = Vector2.zero;//Temporal
            }

        }

        private void PlayerInputsValues()
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");
            CheckFaceDirection();
        }

        private void CanMove()
        {
            PlayerMoveImprove();
        }

        private void PlayerMoveImprove()
        {
            Vector2 accelRate = new Vector2((Mathf.Abs(TargetSpeed(movement, speed).x) > 0.01f) ? runAccelAmount : runDeccelAmount,
                (Mathf.Abs(TargetSpeed(movement, speed).y) > 0.01f) ? runAccelAmount : runDeccelAmount);
            Vector2 speedDif = SpeedDifference(TargetSpeed(movement, speed), rb2d.velocity);

            rb2d.AddForce(ApplyAcceleration(speedDif, accelRate), ForceMode2D.Force);
        }
        
        public static Vector2 ApplyAcceleration(Vector2 targetSpeed, Vector2 acceleration)
        {
            return new Vector2(targetSpeed.x * acceleration.x, targetSpeed.y * acceleration.y);
        }
        
        public static Vector2 SpeedDifference(Vector2 targetSpeed, Vector2 velocity)
        {
            return new Vector2(targetSpeed.x - velocity.x, targetSpeed.y - velocity.y);
        }

        public static Vector2 TargetSpeed(Vector2 movement, float speed)
        {
            return new Vector2(movement.x, movement.y).normalized * speed;
        }

        private void CheckFaceDirection()
        {
            if(movement != Vector2.zero)
            {
                faceDirection.x = movement.x;
                faceDirection.y = movement.y;
            }

        }

        private void PlayerAnimation(Animator animator)
        {
            if (movement.magnitude != 0)
            {
                animator.SetFloat("Horizontal", movement.x);
                animator.SetFloat("Vertical", movement.y);
                animator.Play("Movement");
            }
            else
            {
                animator.Play("IdleBlend");
            }
        }

        private void SpritePlayerOn(GameObject spriteOn, GameObject spriteOff)
        {
            spriteOn.SetActive(true);
            spriteOff.SetActive(false);
        }

        private void PlayerAnimationController()
        {
            if (isDay)
            {
                PlayerAnimation(dayAnim);
                SpritePlayerOn(dayPlayer, nigthPlayer);
            }
            else
            {
                PlayerAnimation(nigthAnim);
                SpritePlayerOn(nigthPlayer, dayPlayer);
            }
        }

        public void ChangePlayerSprite()
        {
            isDay =! isDay;
        }
        
    }
}
