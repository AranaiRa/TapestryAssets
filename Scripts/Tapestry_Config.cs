using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Tapestry_Config {

    public static bool
        InvertPlayerCameraX = false,
        InvertPlayerCameraY = true;
    public static float
        BaseEntityWalkSpeed = 2.0f,
        BaseEntityRunSpeed = 4.4f,
        PlayerCameraSensitivityX = 2.0f,
        PlayerCameraSensitivityY = 1.0f,
        EntityActivationDistance = 2.0f;
    public static Tapestry_Attribute pushLiftAttribute = Tapestry_Attribute.Strength;
    public static Tapestry_Skill lockBypassSkill = Tapestry_Skill.Larceny;
    public static KeyCode
        KeyboardInput_Fwd           = KeyCode.W,
        KeyboardInput_Left          = KeyCode.A,
        KeyboardInput_Back          = KeyCode.S,
        KeyboardInput_Right         = KeyCode.D,
        KeyboardInput_Activate      = KeyCode.E,
        KeyboardInput_Jump          = KeyCode.Space,
        KeyboardInput_CamTypeSwitch = KeyCode.Tab,
        KeyboardInput_Crouch        = KeyCode.LeftControl,
        KeyboardInput_PushLift      = KeyCode.R,
        KeyboardInput_RunWalkToggle = KeyCode.Z;
    //TODO: Figure out actual controller input
    public static KeyCode
        ControllerInput_Fwd           = KeyCode.Joystick1Button0,
        ControllerInput_Left          = KeyCode.Joystick1Button0,
        ControllerInput_Back          = KeyCode.Joystick1Button0,
        ControllerInput_Right         = KeyCode.Joystick1Button0,
        ControllerInput_Activate      = KeyCode.Joystick1Button0,
        ControllerInput_Jump          = KeyCode.Joystick1Button0,
        ControllerInput_CamTypeSwitch = KeyCode.Joystick1Button0,
        ControllerInput_Crouch        = KeyCode.Joystick1Button0,
        ControllerInput_PushLift      = KeyCode.Joystick1Button0,
        ControllerInput_RunWalkToggle = KeyCode.Joystick1Button0;
}
