using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InputManager
{
    public static float GetAxis(string name) {
        return Input.GetAxis(name);
    }

    public static float GetAxisRaw(string name) {
        return Input.GetAxisRaw(name);
    }

    public static bool GetButton(string name) {
        return Input.GetButton(name);
    }

    public static bool GetButtonDown(string name) {
        return Input.GetButtonDown(name);
    }

    public static bool GetButtonUp(string name) {
        return Input.GetButtonUp(name);
    }

    public static Vector3 MousePosition() {
        return Input.mousePosition;
    }

}
