using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Classe PlayerFreeLookState: stato in cui il giocatore si muove liberamente senza target
public class PlayerFreeLookState : PlayerBaseState
{
    //Hash della blend tree di movimento libero
    private readonly int FreeLookBlendTreeHash = Animator.StringToHash("FreeLookBlendTree");
    //Hash per controllare la velocità nella blend tree
    private readonly int FreeLookSpeedHash = Animator.StringToHash("FreeLookSpeed");

    //Tempi di damping e transizione dell'animazione
    private const float AnimatorDampTime = 0.1f;
    private const float CrossFadeDuration = 0.1f;

    public PlayerFreeLookState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    //Funzione chiamata quando il giocatore entra nello stato
    public override void Enter()
    {
        //Sottoscrive eventi di input per target e salto
        stateMachine.InputReader.TargetEvent += OnTarget;
        stateMachine.InputReader.JumpEvent += OnJump;

        //Avvia l'animazione di movimento libero
        stateMachine.Animator.CrossFadeInFixedTime(FreeLookBlendTreeHash, CrossFadeDuration);
    }

    //Funzione chiamata ogni frame
    public override void Tick(float deltaTime)
    {
        //Se il giocatore attacca, passa allo stato di attacco
        if (stateMachine.InputReader.IsAttacking)
        {
            stateMachine.SwitchState(new PlayerAttackingState(stateMachine, 0));
            return;
        }

        //Calcola il movimento in base all'input del giocatore
        Vector3 movement = CalculateMovement();

        //Applica il movimento con la velocità di FreeLook
        Move(movement * stateMachine.FreeLookMovementSpeed, deltaTime);

        //Gestione animazione: se fermo, velocità 0; altrimenti 1
        if (stateMachine.InputReader.MovementValue == Vector2.zero)
        {
            stateMachine.Animator.SetFloat(FreeLookSpeedHash, 0, AnimatorDampTime, deltaTime);
            return;
        }
        stateMachine.Animator.SetFloat(FreeLookSpeedHash, 1, AnimatorDampTime, deltaTime);

        //Ruota il personaggio nella direzione del movimento
        FaceMovementDirection(movement, deltaTime);
    }

    //Funzione chiamata quando si esce dallo stato
    public override void Exit()
    {
        //Rimuove le sottoscrizioni agli eventi di input
        stateMachine.InputReader.TargetEvent -= OnTarget;
        stateMachine.InputReader.JumpEvent -= OnJump;
    }

    //Gestione evento target
    private void OnTarget()
    {
        if (!stateMachine.Targeter.SelectTarget()) return;

        //Se c'è un target valido, passa allo stato Targeting
        stateMachine.SwitchState(new PlayerTargetingState(stateMachine));
    }

    //Gestione evento salto
    private void OnJump()
    {
        stateMachine.SwitchState(new PlayerJumpingState(stateMachine));
    }

    //Calcola la direzione del movimento in base all'input e alla camera
    private Vector3 CalculateMovement()
    {
        Vector3 forward = stateMachine.MainCameraTransform.forward;
        Vector3 right = stateMachine.MainCameraTransform.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        return forward * stateMachine.InputReader.MovementValue.y
            + right * stateMachine.InputReader.MovementValue.x;
    }

    //Ruota il personaggio nella direzione del movimento
    private void FaceMovementDirection(Vector3 movement, float deltaTime)
    {
        stateMachine.transform.rotation = Quaternion.Lerp(
            stateMachine.transform.rotation,
            Quaternion.LookRotation(movement),
            deltaTime * stateMachine.RotationDamping
        );
    }
}
