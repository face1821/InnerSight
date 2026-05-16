using System;
using UnityEngine;

public class LockPauseMenuWhenActive : MonoBehaviour
{
    private void OnEnable() { PauseMenu.IsLocked = true; }

    private void OnDisable() { PauseMenu.IsLocked = false; }
}