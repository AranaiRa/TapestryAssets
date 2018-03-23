using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TapestryEditor_ComponentCreator : EditorWindow {

    bool
        includeHelpers = true,
        createAtScreenCenter = true,
        createAtOrigin = false,
        useTargetNormalAxis = true;
    readonly int
        buttonWidth = 104;

    [MenuItem("Tapestry/Component Creator")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(TapestryEditor_ComponentCreator), true, "Tapestry Component Creator", true);
    }

    private void OnGUI()
    {
        DrawInitPanel();
        DrawOptionsPanel();
        DrawButtonsPanel();
        DrawNonWorldObjectsPanel();
    }

    private void DrawInitPanel()
    {
        GUILayout.BeginVertical("box");
        GUILayout.BeginHorizontal();

        GUILayout.FlexibleSpace();
        DrawInitButton();
        GUILayout.FlexibleSpace();

        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
    }

    private void DrawOptionsPanel()
    {
        GUILayout.BeginVertical("box");
        GUILayout.BeginHorizontal();

        includeHelpers = EditorGUILayout.Toggle(includeHelpers, GUILayout.Width(12));
        GUILayout.Label("Include Helpers?");
        GUILayout.FlexibleSpace();

        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();

        GUILayout.Label("Create Objects At:");
        GUILayout.FlexibleSpace();

        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();

        GUILayout.Space(40);
        createAtScreenCenter = EditorGUILayout.Toggle(createAtScreenCenter, GUILayout.Width(12));
        GUILayout.Label("Screen Center");
        if (createAtScreenCenter)
            createAtOrigin = false;
        if (!createAtScreenCenter && !createAtOrigin)
            createAtScreenCenter = true;
        GUILayout.FlexibleSpace();
        if (createAtScreenCenter)
        {
            useTargetNormalAxis = EditorGUILayout.Toggle(useTargetNormalAxis, GUILayout.Width(12));
            GUILayout.Label("Use Normal as Rotation?");
            GUILayout.FlexibleSpace();
        }

        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();

        GUILayout.Space(40);
        createAtOrigin = EditorGUILayout.Toggle(createAtOrigin, GUILayout.Width(12));
        if (createAtOrigin)
            createAtScreenCenter = false;
        if (!createAtScreenCenter && !createAtOrigin)
            createAtOrigin = true;
        GUILayout.Label("World Origin");

        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
    }

    private void DrawButtonsPanel()
    {
        GUILayout.BeginVertical("box");

        GUILayout.BeginHorizontal();

        GUILayout.FlexibleSpace();
        DrawPropButton();
        GUILayout.FlexibleSpace();

        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();

        GUILayout.FlexibleSpace();
        DrawDoorButton();
        GUILayout.FlexibleSpace();
        DrawSwitchButton();
        GUILayout.FlexibleSpace();
        DrawContactSwitchButton();
        GUILayout.FlexibleSpace();

        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();

        GUILayout.FlexibleSpace();
        DrawContainerButton();
        GUILayout.FlexibleSpace();
        DrawItemSourceButton();
        GUILayout.FlexibleSpace();
        DrawAnimatedLightButton();
        GUILayout.FlexibleSpace();

        GUILayout.EndHorizontal();

        GUILayout.EndVertical();
    }

    private void DrawInitButton()
    {
        if (GUILayout.Button("Initialize Scene", GUILayout.Width(buttonWidth*2)))
        {

        }
    }

    private void DrawPropButton()
    {
        if (GUILayout.Button("Prop", GUILayout.Width(buttonWidth)))
        {

        }
    }

    private void DrawNonWorldObjectsPanel()
    {
        GUILayout.BeginVertical("box");

        GUILayout.BeginHorizontal();

        GUILayout.FlexibleSpace();
        DrawItemButton();
        GUILayout.FlexibleSpace();
        DrawKeyButton();
        GUILayout.FlexibleSpace();

        GUILayout.EndHorizontal();

        GUILayout.EndVertical();
    }

    private void DrawDoorButton()
    {
        if (GUILayout.Button("Door", GUILayout.Width(buttonWidth)))
        {
            GameObject main = new GameObject();
            main.name = "T_Door";
            if(includeHelpers)
            {
                string msg = "Be aware that any objects with colliders underneath this object in the hierarchy will allow the player to target the door.";
                main.AddComponent<Tapestry_InspectorHelper>();
                main.GetComponent<Tapestry_InspectorHelper>().helpMessage = msg;
            }

            GameObject pivot = new GameObject();
            pivot.name = "T_Pivot";
            pivot.transform.SetParent(main.transform);
            pivot.transform.localPosition = new Vector3(0, 0, 0.64f);
            if(includeHelpers)
            {
                string msg = "Place meshes for any moving parts (including a collider!) underneath this object in the hierarchy.\nMake sure the pivot point is set where you want the mesh to be rotating from.\n\nPlease be aware that all Tapestry gizmos (such as the \"pointHelperGizmo\" underneath this object in the hierarchy) can be safely deleted once you no longer need them.";
                pivot.AddComponent<Tapestry_InspectorHelper>();
                pivot.GetComponent<Tapestry_InspectorHelper>().helpMessage = msg;
            }

            GameObject gizmo = (GameObject)Instantiate(Resources.Load("Technical/pointHelperGizmo"));
            gizmo.transform.SetParent(pivot.transform);
            gizmo.name = "Point Helper Gizmo";
            gizmo.transform.localPosition = Vector3.zero;

            GameObject door = GameObject.CreatePrimitive(PrimitiveType.Cube);
            door.name = "Cube";
            door.transform.SetParent(pivot.transform);
            door.transform.localScale = new Vector3(0.08f, 2.2f, 1.28f);
            door.transform.localPosition = new Vector3(0, 1.1f, -0.64f);
            if (includeHelpers)
            {
                string msg = "Replace me with your door's mesh!";
                door.AddComponent<Tapestry_InspectorHelper>();
                door.GetComponent<Tapestry_InspectorHelper>().helpMessage = msg;
            }

            GameObject emitter = new GameObject();
            emitter.name = "T_Emitter";
            emitter.transform.SetParent(main.transform);
            emitter.transform.localPosition = new Vector3(0, 1.1f, 0);

            if (createAtScreenCenter)
                TransformViaRay(main.transform);
            else if (createAtOrigin)
                main.transform.position = Vector3.zero;

            main.AddComponent<Tapestry_Door>();
        }
    }

    private void DrawSwitchButton()
    {
        if (GUILayout.Button("Switch", GUILayout.Width(buttonWidth)))
        {

        }
    }
    
    private void DrawContactSwitchButton()
    {
        if (GUILayout.Button("Contact Switch", GUILayout.Width(buttonWidth)))
        {

        }
    }

    private void DrawContainerButton()
    {
        if (GUILayout.Button("Container", GUILayout.Width(buttonWidth)))
        {

        }
    }

    private void DrawItemSourceButton()
    {
        if (GUILayout.Button("Item Source", GUILayout.Width(buttonWidth)))
        {

        }
    }

    private void DrawAnimatedLightButton()
    {
        if (GUILayout.Button("Animated Light", GUILayout.Width(buttonWidth)))
        {
            GameObject main = new GameObject();
            main.name = "T_AnimatedLight";
            if (includeHelpers)
            {
                string msg = "Be aware that any objects with colliders underneath this object in the hierarchy will allow the player to target the light.\n\nRemember to set the light parameters in the T_Light object underneath this object in the hierarchy.";
                main.AddComponent<Tapestry_InspectorHelper>();
                main.GetComponent<Tapestry_InspectorHelper>().helpMessage = msg;
            }

            GameObject emissive = new GameObject();
            emissive.name = "T_EmissiveMesh";
            emissive.transform.SetParent(main.transform);
            if (includeHelpers)
            {
                string msg = "Make sure any part of the light that needs to have emissive (glowing) properties is underneath this object in the hierarchy.";
                emissive.AddComponent<Tapestry_InspectorHelper>();
                emissive.GetComponent<Tapestry_InspectorHelper>().helpMessage = msg;
            }

            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.SetParent(emissive.transform);
            cube.transform.localScale = new Vector3(0.1f, 0.2f, 0.1f);
            cube.transform.localPosition = new Vector3(0, 0.1f, 0);
            if (includeHelpers)
            {
                string msg = "Replace me with your emissive mesh!";
                cube.AddComponent<Tapestry_InspectorHelper>();
                cube.GetComponent<Tapestry_InspectorHelper>().helpMessage = msg;
            }

            GameObject particles = new GameObject();
            particles.name = "T_Particles";
            particles.transform.SetParent(main.transform);
            particles.transform.localPosition = new Vector3(0, 0.22f, 0);
            if (includeHelpers)
            {
                string msg = "Make sure to edit the particle systems under this object in the hierarchy if your light uses particles.";
                particles.AddComponent<Tapestry_InspectorHelper>();
                particles.GetComponent<Tapestry_InspectorHelper>().helpMessage = msg;
            }

            GameObject gizmo = (GameObject)Instantiate(Resources.Load("Technical/pointHelperGizmo"));
            gizmo.name = "Point Helper Gizmo";
            gizmo.transform.SetParent(particles.transform);
            gizmo.transform.localPosition = Vector3.zero;

            GameObject light = new GameObject();
            light.name = "T_Light";
            light.transform.SetParent(main.transform);
            light.transform.localPosition = new Vector3(0, 0.22f, 0);
            if (includeHelpers)
            {
                string msg = "Remember to set thie emissive color (if your object glows at all) in the Jitter controls in the parent object.";
                light.AddComponent<Tapestry_InspectorHelper>();
                light.GetComponent<Tapestry_InspectorHelper>().helpMessage = msg;
            }

            GameObject emitter = new GameObject();
            emitter.name = "T_Emitter";
            emitter.transform.SetParent(main.transform);
            emitter.transform.localPosition = new Vector3(0, 0.22f, 0);

            if (createAtScreenCenter)
                TransformViaRay(main.transform);
            else if (createAtOrigin)
                main.transform.position = Vector3.zero;

            main.AddComponent<Tapestry_AnimatedLight>();
        }
    }

    private void DrawItemButton()
    {
        if (GUILayout.Button("Item", GUILayout.Width(buttonWidth)))
        {

        }
    }

    private void DrawKeyButton()
    {
        if (GUILayout.Button("Key", GUILayout.Width(buttonWidth)))
        {

        }
    }

    private void TransformViaRay(Transform t)
    {
        Debug.Log("running method");

        RaycastHit hit;
        Physics.Raycast(SceneView.lastActiveSceneView.camera.transform.position, SceneView.lastActiveSceneView.camera.transform.forward, out hit, 50);

        Debug.Log("hit " + hit.point);

        if (hit.point != Vector3.zero)
            t.position = hit.point;
        else
            t.position = SceneView.lastActiveSceneView.camera.transform.position + SceneView.lastActiveSceneView.camera.transform.forward * 3;
        if (useTargetNormalAxis)
        {
            //t.rotation = Quaternion.LookRotation(hit.normal, hit.tang
        }
    }
}
