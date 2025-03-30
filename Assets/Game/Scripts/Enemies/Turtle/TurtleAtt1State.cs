public class TurtleAtt1State : ITurtleStateMachine
{
    TurtleStateMachine controller;
    private bool impulseApplied = false;

    public TurtleAtt1State(TurtleStateMachine controller)
    {
        this.controller = controller;
    }

    public void OnEnter()
    {
        impulseApplied = false;
        controller.attackSpeed = 400f;
        controller.animator.SetBool("Attack1", true);
        controller.SortNumber();
        impulseApplied = false;
        controller.damage = 30;
        controller.rb.isKinematic = false;
    }

    public void OnExit()
    {
        controller.impulse = false;
        controller.antecipation = false;
        controller.attIdle = false;
        controller.combo = false;
        controller.hashitted = false;
    }

    public void OnUpdate()
    {
        if (controller.playerHit)
        {
            controller.SetState(new TurtleStunState(controller));
            controller.playerHit = false;
            return;
        }
        if (controller.active)
        {
            controller.rightHand.gameObject.SetActive(true);
        }
        else
        {
            controller.rightHand.gameObject.SetActive(false);
        }

        if (controller.impulse && !impulseApplied)
        {
            //controller.Impulse();
            impulseApplied = true;
        }
        if (!controller.antecipation)
        {
            controller.RotateTowardsPlayer();
        }

        if (controller.sortedNumber < 0.8f)
        {
            if (controller.combo && controller.TargetDir().magnitude <= controller.meleeRange + 4)
            {
                controller.combed = true;
                controller.animator.SetBool("att1att2", true);
                controller.SetState(new TurtleAtt2State(controller));
            }
            else if (controller.combo && controller.TargetDir().magnitude >= controller.meleeRange + 4)
            {
                controller.animator.SetBool("Attack1", false);
                controller.SetState(new TurtleCombatIdleState(controller));
            }
        }
        else
        {
            if (controller.attIdle)
            {
                controller.animator.SetBool("Attack1", false);
                controller.SetState(new TurtleCombatIdleState(controller));
            }
        }
    }
}
