using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
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
        DrawAnimatedLightButton(); //DrawItemGeneratorButton();
        GUILayout.FlexibleSpace();

        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();

        GUILayout.FlexibleSpace();
        DrawEffectZoneButton();
        GUILayout.FlexibleSpace();
        //
        GUILayout.FlexibleSpace();
        //
        GUILayout.FlexibleSpace();

        GUILayout.EndHorizontal();

        GUILayout.EndVertical();
    }

    private void DrawInitButton()
    {
        if (GUILayout.Button("Initialize Scene", GUILayout.Width(buttonWidth*2)))
        {
            GameObject level = new GameObject();
            level.name = "LEVEL";
            level.transform.localPosition = Vector3.zero;

            GameObject lighting = new GameObject();
            lighting.name = "LIGHTING";
            lighting.transform.SetParent(level.transform);
            lighting.transform.localPosition = Vector3.zero;

            GameObject mainLight = (GameObject)Instantiate(Resources.Load("Technical/T_Level"));
            mainLight.transform.SetParent(lighting.transform);
            mainLight.transform.localPosition = new Vector3(0,10,0);

            GameObject geo = new GameObject();
            geo.name = "GEO";
            geo.transform.SetParent(level.transform);
            geo.transform.localPosition = Vector3.zero;

            GameObject interaction = new GameObject();
            interaction.name = "INTERACTION";
            interaction.transform.SetParent(level.transform);
            interaction.transform.localPosition = Vector3.zero;

            GameObject clutter = new GameObject();
            clutter.name = "CLUTTER";
            clutter.transform.SetParent(level.transform);
            clutter.transform.localPosition = Vector3.zero;

            GameObject canvas = (GameObject)Instantiate(Resources.Load("Technical/T_Canvas"));
            canvas.name = "Canvas";

            GameObject floor = GameObject.CreatePrimitive(PrimitiveType.Cube);
            floor.name = "FLOOR";
            floor.transform.SetParent(geo.transform);
            floor.transform.localScale = new Vector3(100, 0.5f, 100);
            floor.transform.localPosition = new Vector3(0, -0.25f, 0);
            floor.GetComponent<Renderer>().material = (Material)Resources.Load("Technical/mat_init_floor");

            GameObject player = (GameObject)Instantiate(Resources.Load("Technical/T_Player"));
            player.name = "T_Player";

            canvas.GetComponent<Tapestry_UI_HUD>().player = player.GetComponent<Tapestry_Player>();
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

        GUILayout.BeginVertical("box");
        DrawTiledAssetGeneratorButton();
        GUILayout.EndVertical();
    }

    private void DrawPropButton()
    {
        if (GUILayout.Button("Prop", GUILayout.Width(buttonWidth)))
        {
            GameObject main = new GameObject();
            main.name = "T_Prop";

            GameObject intact = new GameObject();
            intact.name = "T_Intact";
            intact.transform.SetParent(main.transform);
            intact.transform.localPosition = Vector3.zero;
            if (includeHelpers)
            {
                string msg = "Place the model for the Prop when unbroken underneath this object in the hierarchy.\n\nEven if your Prop isn't destructable, it's still good practice to keep it here for organization purposes.";
                intact.AddComponent<Tapestry_InspectorHelper>();
                intact.GetComponent<Tapestry_InspectorHelper>().helpMessage = msg;
            }

            GameObject broken = new GameObject();
            broken.name = "T_Broken";
            broken.transform.SetParent(main.transform);
            broken.transform.localPosition = Vector3.zero;
            broken.SetActive(false);
            if (includeHelpers)
            {
                string msg = "Place the model for the Prop when broken underneath this object in the hierarchy.\n\nIf your Prop isn't destructable, you can safely ignore this object.";
                broken.AddComponent<Tapestry_InspectorHelper>();
                broken.GetComponent<Tapestry_InspectorHelper>().helpMessage = msg;
            }

            GameObject bc01 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            bc01.name = "Cube";
            bc01.transform.SetParent(broken.transform);
            bc01.transform.localPosition = new Vector3(0.5f, 0.5f, 0.5f);
            bc01.transform.localRotation = Quaternion.Euler(-0.6f, 1.6f, -1.3f);

            GameObject bc02 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            bc02.name = "Cube";
            bc02.transform.SetParent(broken.transform);
            bc02.transform.localPosition = new Vector3(-0.5f, 0.5f, 0.5f);
            bc02.transform.localRotation = Quaternion.Euler(-0.3f, 2.6f, -1.8f);

            GameObject bc03 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            bc03.name = "Cube";
            bc03.transform.SetParent(broken.transform);
            bc03.transform.localPosition = new Vector3(0.5f, 0.5f, -0.5f);
            bc03.transform.localRotation = Quaternion.Euler(2.2f, -1.8f, 1.6f);

            GameObject bc04 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            bc04.name = "Cube";
            bc04.transform.SetParent(broken.transform);
            bc04.transform.localPosition = new Vector3(-0.5f, 0.5f, -0.5f);
            bc04.transform.localRotation = Quaternion.Euler(-2.4f, 1.9f, -1.3f);

            GameObject bc05 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            bc05.name = "Cube";
            bc05.transform.SetParent(broken.transform);
            bc05.transform.localPosition = new Vector3(0.5f, 1.5f, 0.5f);
            bc05.transform.localRotation = Quaternion.Euler(-2.6f, -0.4f, 2.4f);

            GameObject bc06 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            bc06.name = "Cube";
            bc06.transform.SetParent(broken.transform);
            bc06.transform.localPosition = new Vector3(-0.5f, 1.5f, 0.5f);
            bc06.transform.localRotation = Quaternion.Euler(0.8f, 3.2f, -2.4f);

            GameObject bc07 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            bc07.name = "Cube";
            bc07.transform.SetParent(broken.transform);
            bc07.transform.localPosition = new Vector3(0.5f, 1.5f, -0.5f);
            bc07.transform.localRotation = Quaternion.Euler(-1.4f, 0.2f, 1.3f);

            GameObject bc08 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            bc08.name = "Cube";
            bc08.transform.SetParent(broken.transform);
            bc08.transform.localPosition = new Vector3(-0.5f, 1.5f, -0.5f);
            bc08.transform.localRotation = Quaternion.Euler(-4.0f, 0.0f, -2.4f);

            GameObject destroyed = new GameObject();
            destroyed.name = "T_Destroyed";
            destroyed.transform.SetParent(main.transform);
            destroyed.transform.localPosition = Vector3.zero;
            destroyed.SetActive(false);
            if (includeHelpers)
            {
                string msg = "Place the model for the Prop when destroyed underneath this object in the hierarchy.\n\nIf your Prop isn't destructable, you can safely ignore this object.";
                destroyed.AddComponent<Tapestry_InspectorHelper>();
                destroyed.GetComponent<Tapestry_InspectorHelper>().helpMessage = msg;
            }

            GameObject dc01 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            dc01.name = "Cube";
            dc01.transform.SetParent(destroyed.transform);
            dc01.transform.localPosition = new Vector3(0.5f, 0.5f, 0.5f);
            dc01.transform.localRotation = Quaternion.Euler(-0.6f, 1.6f, -1.3f);
            dc01.AddComponent<Rigidbody>();

            GameObject dc02 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            dc02.name = "Cube";
            dc02.transform.SetParent(destroyed.transform);
            dc02.transform.localPosition = new Vector3(-0.5f, 0.5f, 0.5f);
            dc02.transform.localRotation = Quaternion.Euler(-0.3f, 2.6f, -1.8f);
            dc02.AddComponent<Rigidbody>();

            GameObject dc03 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            dc03.name = "Cube";
            dc03.transform.SetParent(destroyed.transform);
            dc03.transform.localPosition = new Vector3(0.5f, 0.5f, -0.5f);
            dc03.transform.localRotation = Quaternion.Euler(2.2f, -1.8f, 1.6f);
            dc03.AddComponent<Rigidbody>();

            GameObject dc04 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            dc04.name = "Cube";
            dc04.transform.SetParent(destroyed.transform);
            dc04.transform.localPosition = new Vector3(-0.5f, 0.5f, -0.5f);
            dc04.transform.localRotation = Quaternion.Euler(-2.4f, 1.9f, -1.3f);
            dc04.AddComponent<Rigidbody>();

            GameObject dc05 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            dc05.name = "Cube";
            dc05.transform.SetParent(destroyed.transform);
            dc05.transform.localPosition = new Vector3(0.5f, 1.5f, 0.5f);
            dc05.transform.localRotation = Quaternion.Euler(-2.6f, -0.4f, 2.4f);
            dc05.AddComponent<Rigidbody>();

            GameObject dc06 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            dc06.name = "Cube";
            dc06.transform.SetParent(destroyed.transform);
            dc06.transform.localPosition = new Vector3(-0.5f, 1.5f, 0.5f);
            dc06.transform.localRotation = Quaternion.Euler(0.8f, 3.2f, -2.4f);
            dc06.AddComponent<Rigidbody>();

            GameObject dc07 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            dc07.name = "Cube";
            dc07.transform.SetParent(destroyed.transform);
            dc07.transform.localPosition = new Vector3(0.5f, 1.5f, -0.5f);
            dc07.transform.localRotation = Quaternion.Euler(-1.4f, 0.2f, 1.3f);
            dc07.AddComponent<Rigidbody>();

            GameObject dc08 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            dc08.name = "Cube";
            dc08.transform.SetParent(destroyed.transform);
            dc08.transform.localPosition = new Vector3(-0.5f, 1.5f, -0.5f);
            dc08.transform.localRotation = Quaternion.Euler(-4.0f, 0.0f, -2.4f);
            dc08.AddComponent<Rigidbody>();

            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.name = "Cube";
            cube.transform.SetParent(intact.transform);
            cube.transform.localScale = new Vector3(2, 2, 2);
            cube.transform.localPosition = new Vector3(0, 1, 0);
            if (includeHelpers)
            {
                string msg = "Replace me with the mesh for the Prop when intact!";
                cube.AddComponent<Tapestry_InspectorHelper>();
                cube.GetComponent<Tapestry_InspectorHelper>().helpMessage = msg;
            }

            GameObject attaches = new GameObject();
            attaches.name = "T_AttachPoints";
            attaches.transform.SetParent(main.transform);
            attaches.transform.localPosition = new Vector3(0, 0.945f, 0);
            if (includeHelpers)
            {
                string msg = "The objects underneath this object in the hierarchy determine what points the player can grab onto for Pushing and Lifting.";
                attaches.AddComponent<Tapestry_InspectorHelper>();
                attaches.GetComponent<Tapestry_InspectorHelper>().helpMessage = msg;
            }

            GameObject n = new GameObject();
            n.name = "P_Attach";
            n.transform.SetParent(attaches.transform);
            n.transform.localPosition = new Vector3(0, 0, -1.28f);

            GameObject nGiz = (GameObject)Instantiate(Resources.Load("Technical/facingHelperGizmo"));
            nGiz.name = "Facing Helper Gizmo";
            nGiz.transform.SetParent(n.transform);
            nGiz.transform.localPosition = Vector3.zero;
            nGiz.transform.localRotation = Quaternion.identity;

            GameObject e = new GameObject();
            e.name = "P_Attach";
            e.transform.SetParent(attaches.transform);
            e.transform.localPosition = new Vector3(-1.28f, 0, 0);
            e.transform.localRotation = Quaternion.Euler(0, 90, 0);

            GameObject eGiz = (GameObject)Instantiate(Resources.Load("Technical/facingHelperGizmo"));
            eGiz.name = "Facing Helper Gizmo";
            eGiz.transform.SetParent(e.transform);
            eGiz.transform.localPosition = Vector3.zero;
            eGiz.transform.localRotation = Quaternion.identity;

            GameObject s = new GameObject();
            s.name = "P_Attach";
            s.transform.SetParent(attaches.transform);
            s.transform.localPosition = new Vector3(0, 0, 1.28f);
            s.transform.localRotation = Quaternion.Euler(0, 180, 0);

            GameObject sGiz = (GameObject)Instantiate(Resources.Load("Technical/facingHelperGizmo"));
            sGiz.name = "Facing Helper Gizmo";
            sGiz.transform.SetParent(s.transform);
            sGiz.transform.localPosition = Vector3.zero;
            sGiz.transform.localRotation = Quaternion.identity;

            GameObject w = new GameObject();
            w.name = "P_Attach";
            w.transform.SetParent(attaches.transform);
            w.transform.localPosition = new Vector3(1.28f, 0, 0);
            w.transform.localRotation = Quaternion.Euler(0, 270, 0);

            GameObject wGiz = (GameObject)Instantiate(Resources.Load("Technical/facingHelperGizmo"));
            wGiz.name = "Facing Helper Gizmo";
            wGiz.transform.SetParent(w.transform);
            wGiz.transform.localPosition = Vector3.zero;
            wGiz.transform.localRotation = Quaternion.identity;

            if (createAtScreenCenter)
                TransformViaRay(main.transform);
            else if (createAtOrigin)
                main.transform.position = Vector3.zero;

            main.AddComponent<Tapestry_Prop>();
        }
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
            if (includeHelpers)
            {
                string msg = "Place any moving parts of your door (including a collider!) underneath this object in the hierarchy.\n\nThis object also determines how the object transitions from its off state to its on state.\n\nOnce your objects are in position, go back to the root object and click the \"Bake Closed Transform\" button under the States tab. Come back to this object, change this object's transform to how you want the object when the switch is active, and then click the \"Bake Open Transform\" button under the States tab.\n\nPlease be aware that all Tapestry gizmos (such as the \"Point Helper Gizmo\" underneath this object in the hierarchy) can be safely deleted once you no longer need them.";
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

            Tapestry_Door d = main.AddComponent<Tapestry_Door>();
            d.displayName = "Unnamed Door";
            d.BakeClosedState();
            pivot.transform.localRotation = Quaternion.Euler(0, -72, 0);
            d.BakeOpenState();
            d.Close(true);
        }
    }

    private void DrawSwitchButton()
    {
        if (GUILayout.Button("Switch", GUILayout.Width(buttonWidth)))
        {
            GameObject main = new GameObject();
            main.name = "T_Switch";
            if (includeHelpers)
            {
                string msg = "Be aware that any objects with colliders underneath this object in the hierarchy will allow the player to target the switch.";
                main.AddComponent<Tapestry_InspectorHelper>();
                main.GetComponent<Tapestry_InspectorHelper>().helpMessage = msg;
            }

            GameObject cube1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube1.name = "Cube - Switch Casing";
            cube1.transform.SetParent(main.transform);
            cube1.transform.localPosition = new Vector3(0, 0.05f, 0);
            cube1.transform.localScale = new Vector3(0.5f, 0.1f, 0.5f);
            if (includeHelpers)
            {
                string msg = "Replace me with any parts of your Switch that don't move!\n\nIf you have multiple objects that don't move, consider creating an empty game object to use as a folder.";
                cube1.AddComponent<Tapestry_InspectorHelper>();
                cube1.GetComponent<Tapestry_InspectorHelper>().helpMessage = msg;
            }

            GameObject pivot = new GameObject();
            pivot.name = "T_Pivot";
            pivot.transform.SetParent(main.transform);
            pivot.transform.localPosition = new Vector3(0, 0, 0);
            if (includeHelpers)
            {
                string msg = "Place any moving parts of your switch (including a collider!) underneath this object in the hierarchy.\n\nThis object also determines how the object transitions from its off state to its on state.\n\nOnce your objects are in position, go back to the root object and click the \"Bake Closed Transform\" button under the States tab. Come back to this object, change this object's transform to how you want the object when the switch is active, and then click the \"Bake Open Transform\" button under the States tab.\n\nPlease be aware that all Tapestry gizmos (such as the \"Point Helper Gizmo\" underneath this object in the hierarchy) can be safely deleted once you no longer need them.";
                pivot.AddComponent<Tapestry_InspectorHelper>();
                pivot.GetComponent<Tapestry_InspectorHelper>().helpMessage = msg;
            }

            GameObject cube2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube2.name = "Cube - Switch Button";
            cube2.transform.SetParent(pivot.transform);
            cube2.transform.localPosition = new Vector3(0, 0.125f, 0);
            cube2.transform.localScale = new Vector3(0.3f, 0.05f, 0.3f);
            if (includeHelpers)
            {
                string msg = "Replace me with any moving parts of your switch!";
                cube2.AddComponent<Tapestry_InspectorHelper>();
                cube2.GetComponent<Tapestry_InspectorHelper>().helpMessage = msg;
            }

            GameObject gizmo = (GameObject)Instantiate(Resources.Load("Technical/pointHelperGizmo"));
            gizmo.transform.SetParent(pivot.transform);
            gizmo.name = "Point Helper Gizmo";
            gizmo.transform.localPosition = Vector3.zero;

            if (createAtScreenCenter)
                TransformViaRay(main.transform);
            else if (createAtOrigin)
                main.transform.position = Vector3.zero;

            Tapestry_Switch s = main.AddComponent<Tapestry_Switch>();
            s.displayName = "Unnamed Switch";
            s.pingPong = true;
            s.BakeClosedState();
            pivot.transform.localPosition = new Vector3(0, -.045f, 0);
            s.BakeOpenState();
            s.SwitchOff(true);
        }
    }
    
    private void DrawContactSwitchButton()
    {
        if (GUILayout.Button("Contact Switch", GUILayout.Width(buttonWidth)))
        {
            GameObject main = new GameObject();
            main.name = "T_ContactSwitch";
            if (includeHelpers)
            {
                string msg = "The collider on this object determines the area that can activate this switch.\n\nIf you change the type of collider, make sure to flag the new one as a Trigger!";
                main.AddComponent<Tapestry_InspectorHelper>();
                main.GetComponent<Tapestry_InspectorHelper>().helpMessage = msg;
            }

            GameObject cube1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube1.name = "Cube - Switch Casing";
            cube1.transform.SetParent(main.transform);
            cube1.transform.localPosition = new Vector3(0, 0.05f, 0);
            cube1.transform.localScale = new Vector3(1.5f, 0.1f, 1.5f);
            DestroyImmediate(cube1.gameObject.GetComponent<BoxCollider>());
            if (includeHelpers)
            {
                string msg = "Replace me with any parts of your Switch that don't move!\n\nIf you have multiple objects that don't move, consider creating an empty game object to use as a folder.";
                cube1.AddComponent<Tapestry_InspectorHelper>();
                cube1.GetComponent<Tapestry_InspectorHelper>().helpMessage = msg;
            }

            GameObject pivot = new GameObject();
            pivot.name = "T_Pivot";
            pivot.transform.SetParent(main.transform);
            pivot.transform.localPosition = new Vector3(0, 0, 0);
            if (includeHelpers)
            {
                string msg = "Place any moving parts of your switch underneath this object in the hierarchy.\n\nThis object also determines how the object transitions from its off state to its on state.\n\nOnce your objects are in position, go back to the root object and click the \"Bake Closed Transform\" button under the States tab. Come back to this object, change this object's transform to how you want the object when the switch is active, and then click the \"Bake Open Transform\" button under the States tab.\n\nPlease be aware that all Tapestry gizmos (such as the \"Point Helper Gizmo\" underneath this object in the hierarchy) can be safely deleted once you no longer need them.";
                pivot.AddComponent<Tapestry_InspectorHelper>();
                pivot.GetComponent<Tapestry_InspectorHelper>().helpMessage = msg;
            }

            GameObject cube2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube2.name = "Cube - Switch Button";
            cube2.transform.SetParent(pivot.transform);
            cube2.transform.localPosition = new Vector3(0, 0.125f, 0);
            cube2.transform.localScale = new Vector3(1.3f, 0.05f, 1.3f);
            if (includeHelpers)
            {
                string msg = "Replace me with any moving parts of your switch!";
                cube2.AddComponent<Tapestry_InspectorHelper>();
                cube2.GetComponent<Tapestry_InspectorHelper>().helpMessage = msg;
            }

            DestroyImmediate(cube2.GetComponent<BoxCollider>());

            GameObject gizmo = (GameObject)Instantiate(Resources.Load("Technical/pointHelperGizmo"));
            gizmo.transform.SetParent(pivot.transform);
            gizmo.name = "Point Helper Gizmo";
            gizmo.transform.localPosition = Vector3.zero;

            if (createAtScreenCenter)
                TransformViaRay(main.transform);
            else if (createAtOrigin)
                main.transform.position = Vector3.zero;

            Tapestry_SwitchContact s = main.AddComponent<Tapestry_SwitchContact>();
            s.pingPong = false;
            s.isInteractable = false;
            s.displayNameWhenUnactivatable = true;
            s.displayName = "Unnamed Switch";

            BoxCollider bc = main.AddComponent<BoxCollider>();
            bc.center = new Vector3(0, 0.25f, 0);
            bc.size = new Vector3(1.3f, 0.5f, 1.3f);
            bc.isTrigger = true;
            s.BakeClosedState();
            pivot.transform.localPosition = new Vector3(0, -.045f, 0);
            s.BakeOpenState();
            s.SwitchOff(true);
        }
    }

    private void DrawContainerButton()
    {
        if (GUILayout.Button("Container", GUILayout.Width(buttonWidth)))
        {
            GameObject main = new GameObject();
            main.name = "T_Container";

            GameObject pivot = new GameObject();
            pivot.name = "T_Pivot";
            pivot.transform.SetParent(main.transform);
            pivot.transform.localPosition = new Vector3(0, 0.7f, -0.5f);
            if (includeHelpers)
            {
                string msg = "Place any moving parts of your door (including a collider!) underneath this object in the hierarchy.\n\nThis object also determines how the object transitions from its off state to its on state.\n\nOnce your objects are in position, go back to the root object and click the \"Bake Closed Transform\" button under the States tab. Come back to this object, change this object's transform to how you want the object when the switch is active, and then click the \"Bake Open Transform\" button under the States tab.\n\nPlease be aware that all Tapestry gizmos (such as the \"Point Helper Gizmo\" underneath this object in the hierarchy) can be safely deleted once you no longer need them.";
                pivot.AddComponent<Tapestry_InspectorHelper>();
                pivot.GetComponent<Tapestry_InspectorHelper>().helpMessage = msg;
            }

            GameObject lid = GameObject.CreatePrimitive(PrimitiveType.Cube);
            lid.name = "Cube - Lid";
            lid.transform.SetParent(pivot.transform);
            lid.transform.localScale = new Vector3(1, 0.3f, 1);
            lid.transform.localPosition = new Vector3(0, 0.15f, 0.5f);
            if (includeHelpers)
            {
                string msg = "Replace me with any moving parts of your container!";
                lid.AddComponent<Tapestry_InspectorHelper>();
                lid.GetComponent<Tapestry_InspectorHelper>().helpMessage = msg;
            }

            GameObject latch = GameObject.CreatePrimitive(PrimitiveType.Cube);
            latch.name = "Cube - Latch";
            latch.transform.SetParent(lid.transform);
            latch.transform.localScale = new Vector3(0.1f, 0.6f, 0.05f);
            latch.transform.localPosition = new Vector3(0, -0.35f, 0.525f);

            GameObject gizmo = (GameObject)Instantiate(Resources.Load("Technical/pointHelperGizmo"));
            gizmo.name = "Point Helper Gizmo";
            gizmo.transform.SetParent(pivot.transform);
            gizmo.transform.localPosition = Vector3.zero;

            GameObject intact = new GameObject();
            intact.name = "T_Intact";
            intact.transform.SetParent(main.transform);
            intact.transform.localPosition = Vector3.zero;
            if (includeHelpers)
            {
                string msg = "Place the model for the Intact when unbroken underneath this object in the hierarchy.\n\nEven if your Intact isn't destructable, it's still good practice to keep it here for organization purposes.";
                intact.AddComponent<Tapestry_InspectorHelper>();
                intact.GetComponent<Tapestry_InspectorHelper>().helpMessage = msg;
            }

            GameObject body = GameObject.CreatePrimitive(PrimitiveType.Cube);
            body.name = "Cube - Body";
            body.transform.SetParent(intact.transform);
            body.transform.localScale = new Vector3(1, 0.7f, 1);
            body.transform.localPosition = new Vector3(0, 0.35f, 0);
            if (includeHelpers)
            {
                string msg = "Replace me with any non-moving parts of your container!";
                body.AddComponent<Tapestry_InspectorHelper>();
                body.GetComponent<Tapestry_InspectorHelper>().helpMessage = msg;
            }

            GameObject broken = new GameObject();
            broken.name = "T_Broken";
            broken.transform.SetParent(main.transform);
            broken.transform.localPosition = Vector3.zero;
            broken.SetActive(false);
            if (includeHelpers)
            {
                string msg = "Place the model for the Container when broken underneath this object in the hierarchy.\n\nIf your Container isn't destructable, you can safely ignore this object.";
                broken.AddComponent<Tapestry_InspectorHelper>();
                broken.GetComponent<Tapestry_InspectorHelper>().helpMessage = msg;
            }

            GameObject destroyed = new GameObject();
            destroyed.name = "T_Destroyed";
            destroyed.transform.SetParent(main.transform);
            destroyed.transform.localPosition = Vector3.zero;
            destroyed.SetActive(false);
            if (includeHelpers)
            {
                string msg = "Place the model for the Container when destroyed underneath this object in the hierarchy.\n\nIf your Container isn't destructable, you can safely ignore this object.";
                destroyed.AddComponent<Tapestry_InspectorHelper>();
                destroyed.GetComponent<Tapestry_InspectorHelper>().helpMessage = msg;
            }

            GameObject attaches = new GameObject();
            attaches.name = "T_AttachPoints";
            attaches.transform.SetParent(main.transform);
            attaches.transform.localPosition = new Vector3(0, 0.95f, 0);

            GameObject n = new GameObject();
            n.name = "P_Attach";
            n.transform.SetParent(attaches.transform);
            n.transform.localPosition = new Vector3(0, 0, -0.77f);

            GameObject nGiz = (GameObject)Instantiate(Resources.Load("Technical/facingHelperGizmo"));
            nGiz.name = "Facing Helper Gizmo";
            nGiz.transform.SetParent(n.transform);
            nGiz.transform.localPosition = Vector3.zero;
            nGiz.transform.localRotation = Quaternion.identity;

            GameObject e = new GameObject();
            e.name = "P_Attach";
            e.transform.SetParent(attaches.transform);
            e.transform.localPosition = new Vector3(-0.77f, 0, 0);
            e.transform.localRotation = Quaternion.Euler(0, 90, 0);

            GameObject eGiz = (GameObject)Instantiate(Resources.Load("Technical/facingHelperGizmo"));
            eGiz.name = "Facing Helper Gizmo";
            eGiz.transform.SetParent(e.transform);
            eGiz.transform.localPosition = Vector3.zero;
            eGiz.transform.localRotation = Quaternion.identity;

            GameObject s = new GameObject();
            s.name = "P_Attach";
            s.transform.SetParent(attaches.transform);
            s.transform.localPosition = new Vector3(0, 0, 0.77f);
            s.transform.localRotation = Quaternion.Euler(0, 180, 0);

            GameObject sGiz = (GameObject)Instantiate(Resources.Load("Technical/facingHelperGizmo"));
            sGiz.name = "Facing Helper Gizmo";
            sGiz.transform.SetParent(s.transform);
            sGiz.transform.localPosition = Vector3.zero;
            sGiz.transform.localRotation = Quaternion.identity;

            GameObject w = new GameObject();
            w.name = "P_Attach";
            w.transform.SetParent(attaches.transform);
            w.transform.localPosition = new Vector3(0.77f, 0, 0);
            w.transform.localRotation = Quaternion.Euler(0, 270, 0);

            GameObject wGiz = (GameObject)Instantiate(Resources.Load("Technical/facingHelperGizmo"));
            wGiz.name = "Facing Helper Gizmo";
            wGiz.transform.SetParent(w.transform);
            wGiz.transform.localPosition = Vector3.zero;
            wGiz.transform.localRotation = Quaternion.identity;

            GameObject emitter = new GameObject();
            emitter.name = "T_Emitter";
            emitter.transform.SetParent(main.transform);
            emitter.transform.localPosition = new Vector3(0, 0.5f, 0);

            if (createAtScreenCenter)
                TransformViaRay(main.transform);
            else if (createAtOrigin)
                main.transform.position = Vector3.zero;

            Tapestry_Container c = main.AddComponent<Tapestry_Container>();

            c.displayName = "Unnamed Container";
            c.isInteractable = true;
            c.isAnimated = true;
            c.BakeClosedState();
            pivot.transform.localRotation = Quaternion.Euler(-10, 0, 0);
            c.BakeOpenState();
            c.Close(true);
        }
    }

    private void DrawItemSourceButton()
    {
        if (GUILayout.Button("Item Source", GUILayout.Width(buttonWidth)))
        {
            GameObject main = new GameObject();
            main.name = "T_ItemSource";
            if (includeHelpers)
            {
                string msg = "Be aware that any objects with colliders underneath this object in the hierarchy will allow the player to activate the object.";
                main.AddComponent<Tapestry_InspectorHelper>();
                main.GetComponent<Tapestry_InspectorHelper>().helpMessage = msg;
            }

            GameObject on = new GameObject();
            on.name = "T_Collection_On";
            on.transform.SetParent(main.transform);
            if (includeHelpers)
            {
                string msg = "Place the mesh for the part that disappears when the player collects the item underneath this object in the hierarchy.";
                on.AddComponent<Tapestry_InspectorHelper>();
                on.GetComponent<Tapestry_InspectorHelper>().helpMessage = msg;
            }

            GameObject cube1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube1.name = "Cube";
            cube1.transform.SetParent(on.transform);
            cube1.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            cube1.transform.localPosition = new Vector3(0, 0.45f, 0);

            GameObject off = new GameObject();
            off.name = "T_Collection_Off";
            off.transform.SetParent(main.transform);
            if (includeHelpers)
            {
                string msg = "Place the mesh for the part that is always visible when the player collects the item underneath this object in the hierarchy.";
                off.AddComponent<Tapestry_InspectorHelper>();
                off.GetComponent<Tapestry_InspectorHelper>().helpMessage = msg;
            }

            GameObject cube2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube2.name = "Cube";
            cube2.transform.SetParent(off.transform);
            cube2.transform.localScale = new Vector3(0.1f, 0.3f, 0.1f);
            cube2.transform.localPosition = new Vector3(0, 0.15f, 0);

            GameObject gizmo = (GameObject)Instantiate(Resources.Load("Technical/locationHelperGizmo"));
            gizmo.transform.SetParent(off.transform);
            gizmo.name = "Location Helper Gizmo";
            gizmo.transform.localPosition = Vector3.zero;

            if (createAtScreenCenter)
                TransformViaRay(main.transform);
            else if (createAtOrigin)
                main.transform.position = Vector3.zero;

            Tapestry_ItemSource i = main.AddComponent<Tapestry_ItemSource>();
            i.displayName = "Unnamed Item Source";
        }
    }

    private void DrawItemGeneratorButton()
    {
        if (GUILayout.Button("Item Generator", GUILayout.Width(buttonWidth)))
        {
            Debug.Log("TODO: ItemGenerator");
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

            Tapestry_AnimatedLight l = main.AddComponent<Tapestry_AnimatedLight>();
            l.toggleOnActivate = true;
            l.displayName = "Unnamed Light";
        }
    }

    private void DrawEffectZoneButton()
    {
        if (GUILayout.Button("Effect Zone", GUILayout.Width(buttonWidth)))
        {
            GameObject main = new GameObject();
            main.name = "T_EffectZone";
            /*if (includeHelpers)
            {
                string msg = "Be aware that any objects with colliders underneath this object in the hierarchy will allow the player to target the door.";
                main.AddComponent<Tapestry_InspectorHelper>();
                main.GetComponent<Tapestry_InspectorHelper>().helpMessage = msg;
            }*/

            GameObject gizmo = (GameObject)Instantiate(Resources.Load("Technical/pointHelperGizmo"));
            gizmo.name = "Point Helper Gizmo";
            gizmo.transform.SetParent(main.transform);
            gizmo.transform.localPosition = Vector3.zero;

            if (createAtScreenCenter)
                TransformViaRay(main.transform);
            else if (createAtOrigin)
                main.transform.position = Vector3.zero;

            Tapestry_EffectZone ez = main.AddComponent<Tapestry_EffectZone>();
            BoxCollider bc = main.AddComponent<BoxCollider>();
            bc.center = new Vector3(0, 1, 0);
            bc.size = new Vector3(2, 2, 2);
            bc.isTrigger = true;
        }
    }

    private void DrawItemButton()
    {
        if (GUILayout.Button("Item", GUILayout.Width(buttonWidth)))
        {
            GameObject main = new GameObject();
            main.name = "T_Item";

            GameObject gizmo = (GameObject)Instantiate(Resources.Load("Technical/locationHelperGizmo"));
            gizmo.name = "Location Helper Gizmo";
            gizmo.transform.SetParent(main.transform);
            
            if (createAtScreenCenter)
                TransformViaRay(main.transform);
            else if (createAtOrigin)
                main.transform.position = Vector3.zero;

            Tapestry_Item i = main.AddComponent<Tapestry_Item>();
            i.displayName = "Unnamed Item";
        }
    }

    private void DrawKeyButton()
    {
        if (GUILayout.Button("Key", GUILayout.Width(buttonWidth)))
        {
            GameObject main = new GameObject();
            main.name = "T_Item";

            GameObject gizmo = (GameObject)Instantiate(Resources.Load("Technical/locationHelperGizmo"));
            gizmo.name = "Location Helper Gizmo";
            gizmo.transform.SetParent(main.transform);

            if (createAtScreenCenter)
                TransformViaRay(main.transform);
            else if (createAtOrigin)
                main.transform.position = Vector3.zero;

            Tapestry_ItemKey i = main.AddComponent<Tapestry_ItemKey>();
            i.displayName = "Unnamed Key";
        }
    }

    private void DrawTiledAssetGeneratorButton()
    {
        if (GUILayout.Button("Tiled Asset Generator"))
        {
            GameObject main = new GameObject();
            main.name = "T_TiledAssetGenerator";

            GameObject gizmo = (GameObject)Instantiate(Resources.Load("Technical/locationHelperGizmo"));
            gizmo.name = "Location Helper Gizmo";
            gizmo.transform.SetParent(main.transform);

            if (createAtScreenCenter)
                TransformViaRay(main.transform);
            else if (createAtOrigin)
                main.transform.position = Vector3.zero;

            main.AddComponent<Tapestry_TiledAssetGenerator>();
        }
    }

    private void TransformViaRay(Transform t)
    {
        RaycastHit hit;
        Physics.Raycast(SceneView.lastActiveSceneView.camera.transform.position, SceneView.lastActiveSceneView.camera.transform.forward, out hit, 50);
        
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
