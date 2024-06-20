public abstract class GMBaseState 
{
    public abstract void OnEnterState(GameManager manger);

    public abstract void OnUpdateState(GameManager manager);

    public abstract void OnExitState(GameManager manager);
}


public class LoadDataState : GMBaseState
{
    public override void OnEnterState(GameManager manger)
    {
        manger.DataLoadStateOnEnterState();
    }

    public override void OnUpdateState(GameManager manager)
    {
        manager.DataLoadStateOnUpdateState();
    }

    public override void OnExitState(GameManager manager)
    {
        manager.DataLoadStateOnExitState();
    }
}

public class InitState : GMBaseState
{
    public override void OnEnterState(GameManager manger)
    {
        manger.InitStateOnEnterState();
    }

    public override void OnUpdateState(GameManager manager)
    {
        manager.InitStateOnUpdateState();
    }

    public override void OnExitState(GameManager manager)
    {
        manager.InitStateOnExitState();
    }

}

public class GameLoopState : GMBaseState
{
    public override void OnEnterState(GameManager manger)
    {
        manger.GameLoopOnEnterState();
    }

    public override void OnUpdateState(GameManager manager)
    {
        manager.OnGameLoopOnUpdateState();
    }

    public override void OnExitState(GameManager manager)
    {
        manager.OnGameLoopExitState();
    } 
}

public class PlayerDeathSequence : GMBaseState
{
    public override void OnEnterState(GameManager manger)
    {
        manger.OnEnterPlayerDeathSequenceState();
    }

    public override void OnUpdateState(GameManager manager)
    {
        manager.OnUpdatePlayerDeathSequenceState();
    }

    public override void OnExitState(GameManager manager)
    {
        manager.OnExitPlayerDeathSequenceState();
    }
}

public class Level1RobotBattle : GMBaseState
{
    public override void OnEnterState(GameManager manger)
    {
        manger.OnEnterLevel1RobotBattle();
    }

    public override void OnUpdateState(GameManager manager)
    {
        manager.OnUpdateLevel1RobotBattle();
    }

    public override void OnExitState(GameManager manager)
    {
        manager.OnExitLevel1RobotBattle();
    }
}

