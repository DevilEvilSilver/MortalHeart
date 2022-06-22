using Sirenix.OdinInspector;
using UnityEngine.InputSystem;

public abstract class BaseEnemyState : BaseState
{
#if UNITY_EDITOR
    [ReadOnly]
    public BaseEnemyController actorControllerForEditor;

    protected string[] AllAnimations
    {
        get
        {
            return actorControllerForEditor.allAnimations;
        }
    }

#endif

    protected BaseEnemyController actorController;

    public virtual void OnInit(BaseEnemyController controller)
    {
        actorController = controller;
    }
}