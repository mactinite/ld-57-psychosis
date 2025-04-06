using System;
using Framework.Singleton;
using UnityEngine;
using UnityEngine.UI;


public class CursorManager : SingletonBehaviour<CursorManager>
{
    public enum CursorType
    {
        Default,
        Talk,
        Grab,
        Point,
        Targeting
    }
    
    public Sprite defaultCursor;
    public Sprite talkCursor;
    public Sprite grabCursor;
    public Sprite pointingCursor;
    public Sprite targetingCursor;
    
    public Image cursorSpriteRenderer;
    public Canvas CursorCanvas;
    
    private CursorType currentCursorType = CursorType.Default;
    private void Start()
    {
    }

    private void Update()
    {
        if (CursorCanvas != null)
        {

            if (Cursor.lockState == CursorLockMode.Locked)
            {
                cursorSpriteRenderer.transform.localPosition = Vector3.zero;
                return;
            }
            Vector2 mousePos = Input.mousePosition;
            cursorSpriteRenderer.transform.position = mousePos;

        }
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus)
        {
            _SetCursor(currentCursorType);
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public static void SetCursor(CursorType cursorType)
    {
        Instance._SetCursor(cursorType);
    }
    private void _SetCursor(CursorType cursorType)
    {
        
        currentCursorType = cursorType;
        switch (cursorType)
        {
            case CursorType.Default:
                SetCursorSprite(defaultCursor);
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                break;
            case CursorType.Point:
                SetCursorSprite(pointingCursor);
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.None;
                break;
            case CursorType.Grab:
                SetCursorSprite(grabCursor);
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                break;
            case CursorType.Talk:
                SetCursorSprite(talkCursor);
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                break;
            case CursorType.Targeting:
                SetCursorSprite(targetingCursor);
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Confined;
                break;
        }
    }

    private void SetCursorSprite(Sprite cursor)
    {
        cursorSpriteRenderer.sprite = cursor;
        cursorSpriteRenderer.rectTransform.pivot = cursor.pivot / cursor.rect.size;
    }
}