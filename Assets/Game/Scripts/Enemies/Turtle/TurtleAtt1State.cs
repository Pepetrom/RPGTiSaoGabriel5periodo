public class TurtleAtt1State : ITurtleStateMachine
{
    TurtleStateMachine controller;

    public TurtleAtt1State(TurtleStateMachine controller)
    {
        this.controller = controller;
    }

    public void OnEnter()
    {
        controller.animator.SetBool("Attack1", true);
        controller.SortNumber();
        controller.damage = 30;
        controller.rb.isKinematic = true;
        controller.agent.speed = 0;
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
        if (!controller.antecipation)
        {
            controller.RotateTowardsPlayer(8);
        }
        if (controller.active)
        {
            controller.rightHand.gameObject.SetActive(true);
            controller.rb.isKinematic = false;
            controller.agent.enabled = false;
            controller.KB(200);
        }
        else
        {
            controller.rightHand.gameObject.SetActive(false);
            controller.rb.isKinematic = true;
            controller.agent.enabled = true;
        }

        if (controller.sortedNumber < 0.8f)
        {
            if (controller.combo)
            {
                if(controller.TargetDir().magnitude <= controller.meleeRange + 6)
                {
                    controller.combed = true;
                    controller.animator.SetBool("att1att2", true);
                    controller.SetState(new TurtleAtt2State(controller));
                }
                else
                {
                    controller.animator.SetBool("Attack1", false);
                    controller.SetState(new TurtleCombatIdleState(controller));

                }
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
