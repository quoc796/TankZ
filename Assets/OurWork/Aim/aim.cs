using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class aim : MonoBehaviour
{
    public Texture2D cursor;

    private void Awake()
    {
        changeCursor(cursor);
    }

    
    void changeCursor(Texture2D aimSprite)
    {
        Vector2 hotspot = new Vector2(aimSprite.width / 2, aimSprite.height / 2);
        Cursor.SetCursor(aimSprite, hotspot, CursorMode.Auto);
    }
    void Update()
    {
        // Check for user input to toggle cursor lock state
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleCursorLockState();
        }
    }

    void ToggleCursorLockState()
    {
        Cursor.lockState = Cursor.lockState == CursorLockMode.Locked ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = Cursor.lockState != CursorLockMode.Locked;
    }
}
