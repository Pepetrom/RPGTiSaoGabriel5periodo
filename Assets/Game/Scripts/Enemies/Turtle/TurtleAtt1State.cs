public class TurtleAtt1State : ITurtleStateMachine
{
    TurtleStateMachine controller;

    public TurtleAtt1State(TurtleStateMachine controller)
    {
        this.controller = controller;
    }

    public void OnEnter()
    {
        controller.SortNumber();
        controller.damage = 30;
        controller.attRate += 0.25f;
        controller.rb.isKinematic = true;
    }

    public void OnExit()
    {
        controller.playerHit = false;
        controller.antecipation = false;
        controller.active = false;
        controller.end = false;
        controller.combo = false;
        controller.hashitted = false;
    }

    public void OnUpdate()
    {
        if (controller.playerHit)
        {
            controller.SetState(new TurtleStunState(controller));
            return;
        }
        if (!controller.antecipation)
        {
            controller.RotateTowardsPlayer(8);
        }
        if (controller.active)
        {
            controller.rightHand.enabled = true;
            controller.rb.isKinematic = false;
            controller.agent.enabled = false;
            controller.KB(40);
        }
        else
        {
            controller.rightHand.enabled = false;
            controller.rb.isKinematic = true;
            controller.agent.enabled = true;
        }

        if (controller.sortedNumber < 0.8f)
        {
            if (controller.combo)
            {
                if(controller.TargetDir().magnitude <= controller.meleeRange + 4)
                {
                    controller.animator.SetBool("att2", true);
                    controller.SetState(new TurtleAtt2State(controller));
                }
                else
                {
                    controller.animator.SetBool("att1", false);
                    controller.SetState(new TurtleCombatIdleState(controller));
                }
            }
        }
        else
        {
            if (controller.end)
            {
                controller.animator.SetBool("att1", false);
                controller.SetState(new TurtleCombatIdleState(controller));
            }
        }
    }
}
