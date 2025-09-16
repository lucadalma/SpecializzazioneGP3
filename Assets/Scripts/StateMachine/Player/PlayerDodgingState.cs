using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Classe PlayerDodgingState: stato in cui il giocatore esegue un dash/dodge
public class PlayerDodgingState : PlayerBaseState
{
    //Hash delle animazioni per il dodge
    private readonly int DodgeBlendTreeHash = Animator.StringToHash("DodgeBlendTree");
    private readonly int DodgeForwardHash = Animator.StringToHash("DodgeForward");
    private readonly int DodgeRightHash = Animator.StringToHash("DodgeRight");

    //Tempo rimanente del dodge
    private float remainingDodgeTime;

    //Direzione del dodge fornita dall'input del giocatore
    private Vector3 dodgingDirectionInput;

    //Durata della transizione verso l'animazione di dodge
    private const float CrossFadeDuration = 0.1f;

    //Costruttore con direzione di movimento per il dodge
    public PlayerDodgingState(PlayerStateMachine stateMachine, Vector3 dodgingDirectionInput) : base(stateMachine)
    {
        this.dodgingDirectionInput = dodgingDirectionInput;
    }

    //Funzione chiamata quando il giocatore entra nello stato
    public override void Enter()
    {
        remainingDodgeTime = stateMachine.DodgeDuration;

        //Setta i parametri della blend tree in base all'input
        stateMachine.Animator.SetFloat(DodgeForwardHash, dodgingDirectionInput.y);
        stateMachine.Animator.SetFloat(DodgeRightHash, dodgingDirectionInput.x);
        stateMachine.Animator.CrossFadeInFixedTime(DodgeBlendTreeHash, CrossFadeDuration);

        //Il giocatore diventa invulnerabile durante il dodge
        stateMachine.Health.SetInvulnerable(true);
    }

    //Funzione chiamata ogni frame
    public override void Tick(float deltaTime)
    {
        //Calcola il movimento del dodge
        Vector3 movement = new Vector3();
        movement += stateMachine.transform.right * dodgingDirectionInput.x * stateMachine.DodgeLenght / stateMachine.DodgeDuration;
        movement += stateMachine.transform.forward * dodgingDirectionInput.y * stateMachine.DodgeLenght / stateMachine.DodgeDuration;

        //Applica il movimento
        Move(movement, deltaTime);

        //Guarda verso il target corrente
        FaceTarget();

        //Aggiorna il timer del dodge
        remainingDodgeTime -= deltaTime;

        //Se il dodge è terminato, torna allo stato di targeting
        if (remainingDodgeTime <= 0)
        {
            stateMachine.SwitchState(new PlayerTargetingState(stateMachine));
        }
    }

    //Funzione chiamata quando il giocatore esce dallo stato
    public override void Exit()
    {
        //Rimuove l'invulnerabilità
        stateMachine.Health.SetInvulnerable(false);
    }
}
