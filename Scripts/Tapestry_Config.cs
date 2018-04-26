using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class Tapestry_Config {

    public static bool
        InvertPlayerCameraX = false,
        InvertPlayerCameraY = true;
    public static float
        ClockProgressionFactor = 30.0f,
        BaseEntityWalkSpeed = 2.0f,
        BaseEntityRunSpeed = 4.4f,
        PlayerCameraSensitivityX = 2.0f,
        PlayerCameraSensitivityY = 1.0f,
        EntityActivationDistance = 2.0f,
        SoundVolumeMaster = 1.0f,
        SoundVolumeAmbient = 1.0f,
        SoundVolumeMusic = 1.0f,
        SoundVolumeSFX = 1.0f,
        DefaultSoundRadius = 4.0f,
        ItemDropDistance = 0.7f,
        InventoryItemHideTime = 0.4f;
    public static Tapestry_Attribute pushLiftAttribute = Tapestry_Attribute.Strength;
    public static Tapestry_Skill lockBypassSkill = Tapestry_Skill.Larceny;
    public static KeyCode
        KeyboardInput_Fwd = KeyCode.W,
        KeyboardInput_Left = KeyCode.A,
        KeyboardInput_Back = KeyCode.S,
        KeyboardInput_Right = KeyCode.D,
        KeyboardInput_Activate = KeyCode.E,
        KeyboardInput_Jump = KeyCode.Space,
        KeyboardInput_CamTypeSwitch = KeyCode.Tab,
        KeyboardInput_Crouch = KeyCode.LeftControl,
        KeyboardInput_Push = KeyCode.R,
        KeyboardInput_Lift = KeyCode.T,
        KeyboardInput_RunWalkToggle = KeyCode.Z,
        KeyboardInput_Inventory = KeyCode.I,
        KeyboardInput_InventoryDrop = KeyCode.Q,
        KeyboardInput_Cancel = KeyCode.Escape,
        KeyboardInput_LeftHand = KeyCode.Mouse0,
        KeyboardInput_RightHand = KeyCode.Mouse1,
        KeyboardInput_Swap = KeyCode.F;
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
        ControllerInput_RunWalkToggle = KeyCode.Joystick1Button0,
        ControllerInput_Inventory     = KeyCode.Joystick1Button0,
        ControllerInput_LeftHand      = KeyCode.Joystick1Button0,
        ControllerInput_RightHand     = KeyCode.Joystick1Button0,
        ControllerInput_Swap          = KeyCode.Joystick1Button0;
    public static Color
        SunDayColor          = new Color(1.000f, 0.937f, 0.820f, 1.0f),
        SunTwilightColor     = new Color(0.914f, 0.324f, 0.097f, 1.0f),
        SunNightColor        = new Color(0.000f, 0.000f, 0.000f, 1.0f),
        AmbientDayColor      = new Color(0.216f, 0.314f, 0.382f, 1.0f),
        AmbientTwilightColor = new Color(0.382f, 0.295f, 0.216f, 1.0f),
        AmbientNightColor    = new Color(0.216f, 0.314f, 0.382f, 1.0f);
    public static float
        SunShadowIntensity = 1.0f,
        SunDirectLight = 1.0f,
        SunIndirectLight = 1.0f,
        SunTwilightBleed = 0.15f,
        SunSizeMidday = 0.05f,
        SunSizeTwilight = 0.88f;
    public static Dictionary<string, Type> payloads;
    
    public static Dictionary<string, Type> GetPayloadTypes()
    {
        HandleEffectBuilderClassRegistry();
        return payloads;
    }

    private static void HandleEffectBuilderClassRegistry()
    {
        payloads = new Dictionary<string, Type>();
        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            foreach (var type in assembly.GetTypes())
            {
                if (type.BaseType == typeof(Tapestry_EffectBuilder_Payload))
                {
                    payloads.Add(type.Name.Substring(31, type.Name.Length - 31), type);
                }
            }
        }
    }
}
