/*
 * @Basile Barnola
 * @WeaponBuilderEditor.cs
 * @email : barnola.b@gmail.com
 */


using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

public class WeaponBuilderEditor : EditorWindow
{
    GameObject WeaponPreview = null;
    WeaponBuilder Instance_WeaponBuilder = null;

    Editor previewEditor;

    Texture2D texturBG;

    #region Index Asset & memeber
    //Weapon :
    //Mesh :
    int slcWeapIndex = 0;
    int slcWeapMember = -1;
    //Texture :
    int slcTextWpIndex = 0;
    int slcTextWpMember = -1;

    //Butt :
    //Mesh :
    int slcButtIndex = 0;
    int slcButtMember = -1;
    //Texture :
    int slcTextBtIndex = 0;
    int slcTextBtMember = -1;

    //Cannon :
    //Mesh :
    int slcCannonIndex = 0;
    int slcCannonMember = -1;
    //Texture :
    int slcTextCnIndex = 0;
    int slcTextCnMember = -1;

    //Accessory :
    //Mesh :
    int slcAccessoryIndex = 0;
    int slcAccessoryMember = -1;
    //Texture :
    int slcTextAcIndex = 0;
    int slcTextAcMember = -1;

    //Sight :
    //Mesh :
    int slcSightIndex = 0;
    int slcSightMember = -1;
    //Texture :
    int slcTextSgIndex = 0;
    int slcTextSgMember = -1;
    #endregion

    string prefabName = string.Empty;

    bool needReload = false;
    int spaceSize = 18;

    string PackagePath;


   [MenuItem("Weapon Modular/BuilderPreview")]
    static void ShowWindow()
    {
        EditorWindow window = GetWindow<WeaponBuilderEditor>("Weapon Builder", true);
        window.maxSize = new Vector3(585, 855);
        window.minSize = new Vector3(580, 850);
        window.ShowAuxWindow();
    }

    void OnGUI()
    {
        #region Ressource Name Var :

        // Auto Localisation variables :
        MonoScript script = MonoScript.FromScriptableObject(this);
        string EditorPath = AssetDatabase.GetAssetPath(script);
        PackagePath = GetLocalEditorPath(EditorPath);

        // Ressource searching variables
        string WeaponsPath = PackagePath + "/Weapon/";
        string[] WeaponNames = null;
        string[] FullTextureWeaponNames = null;
        string[] TextureWeaponNames = null;

        string ButtPath = PackagePath + "/Butt/";
        string[] ButtNames = null;
        string[] FullTextureButtNames = null;
        string[] TextureButtNames = null;

        string CannonPath = PackagePath + "/Cannon/";
        string[] CannonNames = null;
        string[] FullTextureCannonNames = null;
        string[] TextureCannonNames = null;

        string AccessoryPath = PackagePath + "/Accessory/";
        string[] AccessoryNames = null;
        string[] FullTextureAccessoryNames = null;
        string[] TextureAccessoryNames = null;

        string SightPath = PackagePath + "/Sight/";
        string[] SightNames = null;
        string[] FullTextureSightNames = null;
        string[] TextureSightNames = null;

        
        #endregion

        // Searching ressources and creat UI :

        // Select Weapon :
        if (Directory.Exists(Application.dataPath + WeaponsPath))
        {
            // Mesh :
            GUILayout.Space(10);
            WeaponNames = GetSubFolderNames(Application.dataPath + WeaponsPath);
            slcWeapIndex = EditorGUILayout.Popup("Weapon : ", slcWeapIndex, WeaponNames);

            //Selection Change : Update preview
            if (slcWeapMember != slcWeapIndex)
            {
                slcWeapMember = slcWeapIndex;

                if (slcWeapIndex != 0)
                {
                    if (WeaponPreview != null)
                        DestroyImmediate(WeaponPreview.gameObject);

                    string[] fbxPath = FindFilesInFolder(WeaponsPath + WeaponNames[slcWeapIndex], ".fbx");
                    if (fbxPath.Length > 0)
                    {
                        GameObject weapon = AssetDatabase.LoadAssetAtPath(fbxPath[0], typeof(GameObject)) as GameObject;
                        CreatWeaponPreview();
                        Instance_WeaponBuilder.Init(weapon);

                        previewEditor = Editor.CreateEditor(WeaponPreview);

                        needReload = true;
                    }
                    else
                    {
                        EditorUtility.DisplayDialog("Warning", "No \".fbx\" file find in selected weapon's folder.", "OK");
                        slcWeapIndex = 0;
                    }
                }
                else if (WeaponPreview != null)
                {
                    DestroyImmediate(WeaponPreview);
                }
            }
            if (WeaponPreview != null)
            {
                //Texture :
                FullTextureWeaponNames = FindFilesInFolder(WeaponsPath + WeaponNames[slcWeapIndex], ".mat");
                TextureWeaponNames = GetFilesName(FullTextureWeaponNames);
                if (TextureWeaponNames != null && TextureWeaponNames.Length > 0)
                {

                    slcTextWpIndex = EditorGUILayout.Popup("Material : ", slcTextWpIndex, TextureWeaponNames);

                    if (slcTextWpMember != slcTextWpIndex || needReload)
                    {
                        slcTextWpMember = slcTextWpIndex;

                        Material material = AssetDatabase.LoadAssetAtPath(FullTextureWeaponNames[slcTextWpIndex], typeof(Material)) as Material;
                        if (material != null)
                        {
                            Instance_WeaponBuilder.Set_Weapon_Mat(material);
                            previewEditor.ReloadPreviewInstances();
                        }
                        else
                        {
                            Debug.Log("Weapon : Warning no Material");
                        }
                    }
                }
                else
                {
                    //No material find.
                    EditorGUILayout.LabelField("No material available in the selected weapon's folder.");
                }


                //Select Butt : 
                if (Directory.Exists(Application.dataPath + ButtPath))
                {
                    //Mesh :
                    GUILayout.Space(10);
                    ButtNames = GetSubFolderNames(Application.dataPath + ButtPath);
                    slcButtIndex = EditorGUILayout.Popup("Butt : ", slcButtIndex, ButtNames);


                    if (ButtNames.Length > 0 && slcButtMember != slcButtIndex || needReload)
                    {
                        slcButtMember = slcButtIndex;

                        if (slcButtIndex != 0)
                        {
                            string[] fbxPath = FindFilesInFolder(ButtPath + ButtNames[slcButtIndex], ".fbx");
                            if (fbxPath.Length > 0)
                            {
                                GameObject butt = AssetDatabase.LoadAssetAtPath(fbxPath[0], typeof(GameObject)) as GameObject;
                                Instance_WeaponBuilder.Add_Butt(butt);

                                previewEditor.ReloadPreviewInstances();

                                needReload = true;
                            }
                            else
                            {
                                EditorUtility.DisplayDialog("Warning", "No \".fbx\" file find in selected butt's folder.", "OK");
                                Instance_WeaponBuilder.Add_Butt(null);
                                previewEditor.ReloadPreviewInstances();

                                slcButtIndex = slcButtMember = 0;
                            }
                        }
                        else
                        {
                            Instance_WeaponBuilder.Add_Butt(null);
                            previewEditor.ReloadPreviewInstances();
                        }
                    }
                    //Texture :
                    if (slcButtIndex != 0)
                    {
                        FullTextureButtNames = FindFilesInFolder(ButtPath + ButtNames[slcButtIndex], ".mat");
                        TextureButtNames = GetFilesName(FullTextureButtNames);

                        if (TextureButtNames != null && TextureButtNames.Length > 0)
                        {
                            slcTextBtIndex = EditorGUILayout.Popup("Material : ", slcTextBtIndex, TextureButtNames);

                            if (slcTextBtMember != slcTextBtIndex || needReload)
                            {
                                slcTextBtMember = slcTextBtIndex;

                                Material material = AssetDatabase.LoadAssetAtPath(FullTextureButtNames[slcTextBtIndex], typeof(Material)) as Material;
                                if (material != null)
                                {
                                    Instance_WeaponBuilder.Set_Butt_Mat(material);
                                    previewEditor.ReloadPreviewInstances();
                                }
                                else
                                    Debug.Log("Butt : Warning no Material");
                            }
                        }
                        else
                        {
                            //No material find.
                            EditorGUILayout.LabelField("No material available in the selected butt's folder.");
                        }
                    }
                    else
                    {
                        GUILayout.Space(spaceSize);
                    }
                }
                
                else
                {
                    //No Folder find.
                    EditorGUILayout.LabelField("Warning : Can't find the butt's folder.");
                }

                //Select Cannon : 
                if (Directory.Exists(Application.dataPath + CannonPath))
                {
                    //Mesh :
                    GUILayout.Space(10);
                    CannonNames = GetSubFolderNames(Application.dataPath + CannonPath);
                    slcCannonIndex = EditorGUILayout.Popup("Cannon : ", slcCannonIndex, CannonNames);


                    if (CannonNames.Length > 0 && (slcCannonMember != slcCannonIndex || needReload))
                    {
                        slcCannonMember = slcCannonIndex;

                        if (slcCannonIndex != 0)
                        {
                            string[] fbxPath = FindFilesInFolder(CannonPath + CannonNames[slcCannonIndex], ".fbx");
                            if (fbxPath.Length > 0)
                            {
                                GameObject cannon = AssetDatabase.LoadAssetAtPath(fbxPath[0], typeof(GameObject)) as GameObject;
                                Instance_WeaponBuilder.Add_Cannon(cannon);

                                 previewEditor.ReloadPreviewInstances();

                                needReload = true;
                            }
                            else if (!needReload)
                            {
                                EditorUtility.DisplayDialog("Warning", "No \".fbx\" file find in selected cannon's folder.", "OK");

                                Instance_WeaponBuilder.Add_Cannon(null);
                                 previewEditor.ReloadPreviewInstances();

                                slcCannonIndex = slcCannonMember = 0;
                            }
                        }
                        else
                        {
                            Instance_WeaponBuilder.Add_Cannon(null);
                             previewEditor.ReloadPreviewInstances();
                        }
                    }
                    if (slcCannonIndex != 0)
                    {
                        //Texture :
                        FullTextureCannonNames = FindFilesInFolder(CannonPath + CannonNames[slcCannonIndex], ".mat");
                        TextureCannonNames = GetFilesName(FullTextureCannonNames);

                        if (TextureCannonNames != null && TextureCannonNames.Length > 0)
                        {
                            slcTextCnIndex = EditorGUILayout.Popup("Material : ", slcTextCnIndex, TextureCannonNames);

                            if (slcTextCnMember != slcTextCnIndex || needReload)
                            {
                                slcTextCnMember = slcTextCnIndex;

                                Material material = AssetDatabase.LoadAssetAtPath(FullTextureCannonNames[slcTextCnIndex], typeof(Material)) as Material;
                                if (material != null)
                                {
                                    Instance_WeaponBuilder.Set_Cannon_Mat(material);
                                     previewEditor.ReloadPreviewInstances();
                                }
                                else
                                    Debug.Log("Cannon : Warning no Material");
                            }
                        }
                        else
                        {
                            //No material find.
                            EditorGUILayout.LabelField("No material available in the selected cannon's folder.");
                        }
                    }
                    else
                    {
                        GUILayout.Space(spaceSize);
                    }

                }
                else
                {
                    //No Folder find.
                    EditorGUILayout.LabelField("Warning : Can't find the cannon's folder.");
                }

                //Select Accessory : 
                if (Directory.Exists(Application.dataPath + AccessoryPath))
                {
                    GUILayout.Space(10);
                    AccessoryNames = GetSubFolderNames(Application.dataPath + AccessoryPath);
                    slcAccessoryIndex = EditorGUILayout.Popup("Accessory : ", slcAccessoryIndex, AccessoryNames);

                    if (AccessoryNames.Length > 0 && (slcAccessoryMember != slcAccessoryIndex || needReload))
                    {
                        slcAccessoryMember = slcAccessoryIndex;

                        if (slcAccessoryIndex != 0)
                        {
                            string[] fbxPath = FindFilesInFolder(AccessoryPath + AccessoryNames[slcAccessoryIndex], ".fbx");
                            if (fbxPath.Length > 0)
                            {
                                GameObject accessory = AssetDatabase.LoadAssetAtPath(fbxPath[0], typeof(GameObject)) as GameObject;
                                Instance_WeaponBuilder.Add_Accessory(accessory);

                                 previewEditor.ReloadPreviewInstances();

                                needReload = true;
                            }
                            else if (!needReload)
                            {
                                EditorUtility.DisplayDialog("Warning", "No \".fbx\" file find in selected accesory's folder.", "OK");
                                Instance_WeaponBuilder.Add_Accessory(null);
                                 previewEditor.ReloadPreviewInstances();

                                slcAccessoryIndex = slcAccessoryMember = 0;
                            }
                        }

                        else
                        {
                            Instance_WeaponBuilder.Add_Accessory(null);
                             previewEditor.ReloadPreviewInstances();
                        }
                    }

                    if (slcAccessoryIndex != 0)
                    {
                        //Texture :
                        FullTextureAccessoryNames = FindFilesInFolder(AccessoryPath + AccessoryNames[slcAccessoryIndex], ".mat");
                        TextureAccessoryNames = GetFilesName(FullTextureAccessoryNames);

                        if (TextureAccessoryNames != null && TextureAccessoryNames.Length > 0)
                        {
                            slcTextAcIndex = EditorGUILayout.Popup("Material : ", slcTextAcIndex, TextureAccessoryNames);

                            if (slcTextAcMember != slcTextAcIndex || needReload)
                            {
                                slcTextAcMember = slcTextAcIndex;

                                Material material = AssetDatabase.LoadAssetAtPath(FullTextureAccessoryNames[slcTextAcIndex], typeof(Material)) as Material;
                                if (material != null)
                                {
                                    Instance_WeaponBuilder.Set_Accessory_Mat(material);
                                     previewEditor.ReloadPreviewInstances();
                                }
                                else
                                    Debug.Log("Accessory : Warning no Material");
                            }
                        }
                        else
                        {
                            //No material find.
                            EditorGUILayout.LabelField("No material available in the selected accesory's folder.");
                        }
                    }
                    else
                    {
                        GUILayout.Space(spaceSize);
                    }
                }
                
                else
                {
                    //No Folder find.
                    EditorGUILayout.LabelField("Warning : Can't find the accesory's folder.");
                }

                //Select Sight : 
                if (Directory.Exists(Application.dataPath + SightPath))
                {
                    GUILayout.Space(10);
                    SightNames = GetSubFolderNames(Application.dataPath + SightPath);
                    slcSightIndex = EditorGUILayout.Popup("Sight : ", slcSightIndex, SightNames);


                    if (SightNames.Length > 0 && (slcSightMember != slcSightIndex || needReload))
                    {
                        slcSightMember = slcSightIndex;

                        if (slcSightIndex != 0)
                        {
                            string[] fbxPath = FindFilesInFolder(SightPath + SightNames[slcSightIndex], ".fbx");
                            if (fbxPath.Length > 0)
                            {
                                GameObject sight = AssetDatabase.LoadAssetAtPath(fbxPath[0], typeof(GameObject)) as GameObject;
                                Instance_WeaponBuilder.Add_Sight(sight);

                                 previewEditor.ReloadPreviewInstances();

                                needReload = true;
                            }
                            else if (!needReload)
                            {
                                EditorUtility.DisplayDialog("Warning", "No \".fbx\" file find in selected sight's folder.", "OK");
                                Instance_WeaponBuilder.Add_Sight(null);
                                 previewEditor.ReloadPreviewInstances();

                                slcSightIndex = slcSightMember = 0;
                            }
                        }
                        else
                        {
                            Instance_WeaponBuilder.Add_Sight(null);
                             previewEditor.ReloadPreviewInstances();
                        }
                    }

                    if (slcSightIndex != 0)
                    {
                        //Texture :
                        FullTextureSightNames = FindFilesInFolder(SightPath + SightNames[slcSightIndex], ".mat");
                        TextureSightNames = GetFilesName(FullTextureSightNames);

                        GUILayout.Width(10);
                        if (TextureSightNames != null && TextureSightNames.Length > 0)
                        {
                            slcTextSgIndex = EditorGUILayout.Popup("Material : ", slcTextSgIndex, TextureSightNames);

                            if (slcTextSgMember != slcTextSgIndex || needReload)
                            {
                                slcTextSgMember = slcTextSgIndex;

                                Material material = AssetDatabase.LoadAssetAtPath(FullTextureSightNames[slcTextSgIndex], typeof(Material)) as Material;
                                if (material != null)
                                {
                                    Instance_WeaponBuilder.Set_Sight_Mat(material);
                                     previewEditor.ReloadPreviewInstances();
                                }
                                else
                                    Debug.Log("Sight : Warning no Material");
                            }
                        }
                        else
                        {
                            //No material find.
                            EditorGUILayout.LabelField("No material available in the selected sight's folder.");
                        }
                    }
                    else
                    {
                        GUILayout.Space(spaceSize);
                    }
                }
                
                else
                {
                    //No Folder find.
                    EditorGUILayout.LabelField("Warning : Can't find the sight's folder.");
                }

                needReload = false;

                // Show and gestion of the savefield :
                GUILayout.Space(20);
                GUILayout.Label("Save Weapon Prefab :");
                GUILayout.BeginHorizontal();
                
                prefabName = EditorGUILayout.TextField(prefabName);

                if (GUILayout.Button("Create Prefab") && prefabName != string.Empty)
                {
                    if (CreatWeaponPrefab(prefabName, PackagePath))
                    {
                        EditorUtility.DisplayDialog("Succes", "Prefab created at path : \"Assets" + PackagePath + "/WeaponPrefab/" + prefabName + ".prefab\"", "OK");
                       prefabName = string.Empty;
                    }
                }
                GUILayout.EndHorizontal();
                GUILayout.Space(20);

                // Preview :
                if (previewEditor != null)
                {
                    UpdateWeaponPreview();
                }
            }
            
        }
        else
        {
            //No Folder find.
            EditorGUILayout.LabelField("Warning : Can't find the weapon's folder.");
        }
    }

    /// <summary>
    /// Creat the gameObject used for the weapon's preview.
    /// </summary>
    /// <param name="_path"></param>
    /// <returns></returns>
    void CreatWeaponPreview()
    {
        WeaponPreview = EditorUtility.CreateGameObjectWithHideFlags("Weapon_Preview", HideFlags.DontSave); 
        Instance_WeaponBuilder = WeaponPreview.AddComponent<WeaponBuilder>();
    }

    /// <summary>
    /// Update and gestion of the Preview
    /// </summary>
    /// <param name="_path"></param>
    /// <returns></returns>
    private void UpdateWeaponPreview()
    {
        if (previewEditor != null)
        {
            Texture2D used = null;
            if (texturBG == null)
                texturBG = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets" + PackagePath + "/hudpreviewMSFW.png");
            if (texturBG != null)
                used = texturBG;
            else
                used = EditorGUIUtility.whiteTexture;

            GUIStyle bgColor = new GUIStyle();
            bgColor.normal.background = used;

            previewEditor.OnInteractivePreviewGUI(GUILayoutUtility.GetRect(512, 512), bgColor);    
        }
    }

    /// <summary>
    /// Just clear the scene before closing the window.
    /// </summary>
    /// <param name="_path"></param>
    /// <returns></returns>
    private void OnDisable()
    {
        if (WeaponPreview != null)
            DestroyImmediate(WeaponPreview.gameObject);
    }

    /// <summary>
    /// Return names of all folders in the folder wich tie in to the given path 
    /// </summary>
    /// <param name="_path"></param>
    /// <returns></returns>
    string[] GetSubFolderNames(string _path)
    {
        string[] folderPaths = Directory.GetDirectories(_path);
        string[] folderNames = new string[folderPaths.Length + 1];

        folderNames[0] = "None";

        for (int i = 1; i < folderPaths.Length + 1; i++)
        {
            string[] splitedPath = folderPaths[i - 1].Split('/');
            folderNames[i] = splitedPath[splitedPath.Length - 1];
        }

        return folderNames;
    }

    /// <summary>
    /// Return names of all files wich corresponding to the given extention, in the folder wich tie in to the given path.
    /// </summary>
    /// <param name="_path"></param>
    /// <returns></returns>
    private string[] FindFilesInFolder(string _path, string _extention)
    {
        List<string> returnedFiles = new List<string>();
        string[] filesPath = Directory.GetFiles(Application.dataPath + _path);

        foreach(string filePath in filesPath)
        {
            if (!filePath.EndsWith(".meta"))
            {
                if (_extention == string.Empty)
                {     
                    returnedFiles.Add("Assets/" + filePath.Replace('\\', '/').Remove(0, Application.dataPath.Length));
                }
                else if (filePath.EndsWith(_extention))
                {
                    returnedFiles.Add("Assets" + filePath.Replace('\\', '/').Remove(0, Application.dataPath.Length));
                }
            }
        }
        return returnedFiles.ToArray();
    }

    /// <summary>
    /// Return the name of a file wich tie in to the given path.
    /// </summary>
    /// <param name="_path"></param>
    /// <returns></returns>
    private string GetFileName(string _path)
    {
        if (_path != null)
        {
            string[] subString = _path.Split('/');

            if (subString.Length > 0)
            {
                subString = subString[subString.Length - 1].Split('.');

                if (subString.Length > 0)
                {
                    return subString[0];
                }
            }
        }
        return string.Empty;
    }

    /// <summary>
    /// Return names of all files in the folder wich tie in to the given path.
    /// </summary>
    /// <param name="_path"></param>
    /// <returns></returns>
    private string[] GetFilesName(string[] _paths)
    {
        if (_paths.Length > 0)
        {
            List<string> lstString = new List<string>();
            foreach (string path in _paths)
                lstString.Add(GetFileName(path));
            return lstString.ToArray();
        }
        return null;
    }

    /// <summary>
    /// Return the path of this sript in the asset folder project.
    /// </summary>
    /// <param name="_path"></param>
    /// <returns></returns>
    private string GetLocalEditorPath(string _path)
    {
        if (_path != null)
        {
            string[] subPath = _path.Split('/');
            string toReturn = string.Empty;

            foreach (string sub in subPath)
            {
                if (sub != "Assets")
                {
                    if (sub != "Editor")
                        toReturn += "/" + sub;
                    else
                        return toReturn;
                }
            }
            return toReturn;
        }
        return string.Empty;
    }

    /// <summary>
    /// Creat a copy prefab of the previewed object in the "WeaponPrefab" folder of the package.
    /// </summary>
    /// <param name="_path"></param>
    /// <returns></returns>
    private bool CreatWeaponPrefab(string _name, string _subFolderName)
    {
        if (WeaponPreview != null && _name != null && _name != string.Empty)
        {
            string folderPath = "/WeaponPrefab/";
            string completPath = "Assets" + _subFolderName + folderPath + _name + ".prefab";

            //if (!Directory.Exists(Application.dataPath + folderPath))
            //    Directory.CreateDirectory(Application.dataPath + folderPath);

            WeaponPreview.gameObject.hideFlags = HideFlags.None;
            PrefabUtility.CreatePrefab(AssetDatabase.GenerateUniqueAssetPath(completPath), WeaponPreview);
            WeaponPreview.gameObject.hideFlags = HideFlags.HideAndDontSave;

            return true;
        }
        return false;
    }


}