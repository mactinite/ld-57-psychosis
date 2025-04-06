using Framework.Singleton;
using StarterAssets;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : SingletonBehaviour<GameManager>
{
    public FirstPersonController playerController;
    public PlayerInput playerInput;
    public StarterAssetsInputs starterAssetsInputs;
    public CinemachineCamera lookAtCamera;
    public CinemachineCamera followCamera;
    public int playerHealth = 4;
    public int playerMaxHealth = 4;

    public static void SetPlayerController(FirstPersonController controller)
    {
        Instance.playerController = controller;
    }
    
    public static FirstPersonController GetPlayerController()
    {
        return Instance.playerController;
    }

    public static PlayerInput GetPlayerInput()
    {
        return Instance.playerInput;
    }
    
    public static StarterAssetsInputs GetInputs()
    {
        return Instance.starterAssetsInputs;
    }
    
    public static void SetPlayerPosition(Vector3 position)
    {
        Instance.playerController.transform.position = position;
    }
    
    public static Vector3 GetPlayerPosition()
    {
        return Instance.playerController.transform.position;
    }
    
    public static void SetPlayerRotation(Quaternion rotation)
    {
        Instance.playerController.transform.rotation = rotation;
    }
    
    public static Quaternion GetPlayerRotation()
    {
        return Instance.playerController.transform.rotation;
    }
    
    public static void SetPlayerActive(bool isActive)
    {
        Instance.playerController.gameObject.SetActive(isActive);
    }
    
    public static bool IsPlayerActive()
    {
        return Instance.playerController.gameObject.activeSelf;
    }

    public static void FreezePlayer()
    {
        // disable player input
        var inputs = GameManager.GetInputs();
        if (inputs != null)
        {
            inputs.allowMovement = false;
            CursorManager.SetCursor(CursorManager.CursorType.Point);
            InteractionManager.DeactivateInteractions();
        }
    }

    public static void UnfreezePlayer()
    {
        var inputs = GameManager.GetInputs();
        if (inputs != null)
        {
            inputs.allowMovement = true;
            CursorManager.SetCursor(CursorManager.CursorType.Default);
            InteractionManager.ActivateInteractions();
        }
    }

    public static void PlayerLookAt(Transform LookAt)
    {
        Instance.lookAtCamera.LookAt = LookAt;
        Instance.followCamera.enabled = false;
    }

    public static void StopPlayerLookAt()
    {
        Instance.followCamera.enabled = true;
    }
}
