using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;
namespace Base {
    public static class B_ExtentionFunctions {

        #region Transform Extentions

        public static void ResizeObject(this Transform objToEnlarge, float Size) {
            objToEnlarge.localScale = new Vector3(Size, Size, Size);
        }

        public static IEnumerable GetAllChilrenOnTransform(this Transform transform) {
            List<Transform> transforms = new List<Transform>();
            foreach (Transform item in transform) {
                transforms.Add(item);
            }
            return transforms;
        }
        
        public static void DestroyAllChildren(this Transform transform) {
            if (transform.childCount <= 0) return;
            for (int i = transform.childCount - 1; i >= 0; i--) {
                #if UNITY_EDITOR
                if (!Application.isPlaying)
                    GameObject.DestroyImmediate(transform.GetChild(i).gameObject);
                #endif
                if (Application.isPlaying)
                    GameObject.Destroy(transform.GetChild(i).gameObject);
            }
        }

        #endregion Transform Extentions
        
        #region Recttransform Extentions

        //Use this to move Pesky uı objects
        public static void MoveUIObject(this RectTransform rectTransform, Vector2 vector2) {
            rectTransform.offsetMax = vector2;
            rectTransform.offsetMin = vector2;
        }

        #endregion Recttransform Extentions

        #region Vector3 Extentions

        public static Vector3 GetWorldPosition(Ray ray, LayerMask Mask) {
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, Mask)) return hit.point;
            return Vector3.zero;
        }

        public static Vector3 GetWorldPosition(this Vector3 vector3, Camera cam, LayerMask Mask) {
            var ray = cam.ScreenPointToRay(vector3);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, Mask)) return hit.point;
            return Vector3.zero;
        }

        public static Transform GetWorldObject(this Vector3 vec3, Camera cam, LayerMask mask) {
            var ray = cam.ScreenPointToRay(vec3);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask)) return hit.collider.transform;
            return null;
        }

        public static Vector3 GetHitPosition(this Vector3 mainObj, Vector3 objectToPush, float yMinus, float force) {
            var _temp = mainObj;
            _temp.y -= yMinus;
            return (objectToPush - _temp) * force;
        }

        public static Vector3 FindCenterInGroup<T>(this IEnumerable<T> ObjectGroup) where T : MonoBehaviour {
            MonoBehaviour[] objects = ObjectGroup.ToArray();
            if (objects.Length == 0)
                return Vector3.zero;
            if (objects.Length == 1)
                return objects[0].transform.position;
            var bounds = new Bounds(objects[0].transform.position, Vector3.zero);
            for (var i = 1; i < objects.Length; i++) {
                if (objects[i] != null)
                    bounds.Encapsulate(objects[i].transform.position);
            }
            return bounds.center;
        }

        #endregion Vector3 Extentions

        #region Math Extentions

        public static float Round(float value, int digits) {
            var mult = Mathf.Pow(10.0f, digits);
            return Mathf.Round(value * mult) / mult;
        }

        public static float Multi(this float value, float multiplier) {
            return value * multiplier;
        }

        public static float Remap(this float value, float from1, float to1, float from2, float to2) {
            return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
        }
        
        public static float Remap(this int value, float from1, float to1, float from2, float to2) {
            return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
        }
        
        public static float CRemap(this float value, float from1, float to1, float from2, float to2) {
            float x = (value - from1) / (to1 - from1) * (to2 - from2) + from2;
            x = Mathf.Clamp(x, to1, to2);
            return x;
        }
        
        public static float CRemap(this int value, float from1, float to1, float from2, float to2) {
            float x = (value - from1) / (to1 - from1) * (to2 - from2) + from2;
            x = Mathf.Clamp(x, to1, to2);
            return x;
        }

        public static float ClampAngle(float angle, float min, float max) {
            angle = Mathf.Repeat(angle, 360);
            min = Mathf.Repeat(min, 360);
            max = Mathf.Repeat(max, 360);
            var inverse = false;
            var tmin = min;
            var tangle = angle;
            if (min > 180) {
                inverse = !inverse;
                tmin -= 180;
            }
            if (angle > 180) {
                inverse = !inverse;
                tangle -= 180;
            }
            var result = !inverse ? tangle > tmin : tangle < tmin;
            if (!result)
                angle = min;

            inverse = false;
            tangle = angle;
            var tmax = max;
            if (angle > 180) {
                inverse = !inverse;
                tangle -= 180;
            }
            if (max > 180) {
                inverse = !inverse;
                tmax -= 180;
            }

            result = !inverse ? tangle < tmax : tangle > tmax;
            if (!result)
                angle = max;
            return angle;
        }

        #endregion Math Extentions

        #region Gameobject Extentions

        public static T InstantiateB<T>(this T obj) where T : MonoBehaviour {
            T returnobj = GameObject.Instantiate(obj, B_LevelControl.CurrentLevelObject);
            return returnobj;
        }
        
        public static T InstantiateB<T>(this T obj, Vector3 position) where T : MonoBehaviour {
            T returnobj = GameObject.Instantiate(obj, B_LevelControl.CurrentLevelObject);
            returnobj.transform.position = position;
            return returnobj;
        }
        
        public static T InstantiateB<T>(this T obj, Vector3 position, Quaternion rotation) where T : MonoBehaviour {
            T returnobj = GameObject.Instantiate(obj, B_LevelControl.CurrentLevelObject);
            returnobj.transform.position = position;
            returnobj.transform.rotation = rotation;
            return returnobj;
        }

        #endregion
        
        #region String Extentions

        public static bool IsAllLetters(this string s) {
            foreach (var c in s)
                if (!char.IsLetter(c))
                    return false;
            return true;
        }

        public static bool IsAllDigits(this string s) {
            foreach (var c in s)
                if (!char.IsDigit(c))
                    return false;
            return true;
        }

        public static bool IsAllLettersOrDigits(this string s) {
            foreach (var c in s)
                if (!char.IsLetterOrDigit(c))
                    return false;
            return true;
        }

        public static float IsFloat(this string s) {
            if (s.IsAllDigits())
                return float.Parse(s);
            return 0;
        }

        public enum SaveNameViabilityStatus { Viable, Null, Incomplete, HasDigits }
        public static SaveNameViabilityStatus IsVaibleForSave(this string obj) {

            if (obj.Length <= 3 && !(obj == null || obj == "Null" || string.IsNullOrEmpty(obj))) return SaveNameViabilityStatus.Incomplete;
            if (obj == null || obj == "Null" || string.IsNullOrEmpty(obj)) return SaveNameViabilityStatus.Null;
            if (obj.Any(char.IsDigit)) return SaveNameViabilityStatus.HasDigits;
            // if(obj.Any(char.spa))
            return SaveNameViabilityStatus.Viable;
        }

        public static string MakeViable(this string obj) {
            switch (obj.IsVaibleForSave()) {
                case SaveNameViabilityStatus.Viable:
                    return obj;
                case SaveNameViabilityStatus.Null:
                    Debug.Log("Name was " + obj.IsVaibleForSave());
                    return "";
                case SaveNameViabilityStatus.Incomplete:
                    Debug.Log(obj + " Was " + obj.IsVaibleForSave());
                    return obj + "_Completed_Part";
                case SaveNameViabilityStatus.HasDigits:
                    Debug.Log(obj + " " + obj.IsVaibleForSave());
                    var newObj = obj.Where(t => !char.IsDigit(t)).ToArray();
                    var newName = new string(newObj);
                    if (newName.Where(t => char.IsLetter(t)).ToArray().Length <= 3) newName = "";
                    return newName;
            }
            return null;
        }

        #endregion String Extentions

        #region AssetDatabase Extentions

        /// <summary>
        /// Only works on editor
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<T> FindAssetsByType<T>() where T : UnityEngine.Object {
            #if UNITY_EDITOR
            List<T> assets = new List<T>();
            string[] guids = AssetDatabase.FindAssets(string.Format("t:{0}", typeof(T)));
            for (int i = 0; i < guids.Length; i++) {
                string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
                T asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);
                if (asset != null) {
                    assets.Add(asset);
                }
            }
            return assets;
            #else
            return null;
            #endif
        }

        public static List<string> FindAssetsPathByType<T>() where T : UnityEngine.Object {
            #if UNITY_EDITOR
            List<string> assets = new List<string>();
            string[] guids = AssetDatabase.FindAssets(string.Format("t:{0}", typeof(T)));
            for (int i = 0; i < guids.Length; i++) {
                string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
                T asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);
                if (asset != null) {
                    assets.Add(assetPath);
                }
            }
            return assets;
            #else
            return null;
            #endif
        }

        public static string FindAssetParenthPath(this string originalPath) {
            #if UNITY_EDITOR
            return Directory.GetParent(originalPath).ToString().Replace("\\", "/");
            #else
            return null;
            #endif
        }

        public static string FindAssetPath<T>() where T : UnityEngine.Object {
            #if UNITY_EDITOR

            string assetPath = "";
            string[] guids = AssetDatabase.FindAssets(string.Format("t:{0}", typeof(T)));
            for (int i = 0; i < guids.Length; i++) {
                assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
                T asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);
                if (asset != null) {
                    assetPath = assetPath.Replace("\\", "/");
                }
            }
            return assetPath;
            
            #else
            return null;
            #endif
        }

        #endregion

        #region Scriptable Object Extentions

        #region Save Extentions

        public static void SaveScriptableObject(this ScriptableObjectSaveInfo obj) {
            string path = $"{Application.persistentDataPath}/{obj.foldername}";
            if (!Directory.Exists(path)) {
                Directory.CreateDirectory(path);
            }
            path += $"/{obj.filename}.scs";
            string saveData = JsonUtility.ToJson(obj.obj, true);
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(path);
            bf.Serialize(file, saveData);
            file.Close();
        }

        public static void LoadScriptableObject(this ScriptableObjectSaveInfo obj) {
            if (obj.SaveExists()) {
                BinaryFormatter bf = new BinaryFormatter();
                string path = $"{Application.persistentDataPath}/{obj.foldername}/{obj.filename}.scs";
                FileStream file = File.Open(path, FileMode.Open);
                JsonUtility.FromJsonOverwrite(bf.Deserialize(file).ToString(), obj.obj);
                file.Close();
            }
        }

        public static bool SaveExists(this ScriptableObjectSaveInfo obj) {
            return File.Exists($"{Application.persistentDataPath}/{obj.foldername}/{obj.filename}.scs");
        }

        public static void ClearScriptableObject(this ScriptableObjectSaveInfo obj) {
            if (obj.SaveExists()) {
                string path = $"{Application.persistentDataPath}/{obj.foldername}/{obj.filename}.scs";
                File.Delete(path);
            }
        }

        #endregion

        #endregion

        #region Coroutine Extentions

        /// <summary>
        /// Simply Runs the enumarator without any return
        /// </summary>
        /// <param name="enumerator"></param>
        public static Coroutine RunCoroutine(this IEnumerator enumerator) {
            return B_CoroutineControl.Queue.RunCoroutine(enumerator);
        }

        /// <summary>
        /// Runs the enumarator with delay without any return
        /// </summary>
        /// <param name="enumerator"></param>
        /// <param name="delay"></param>
        public static Coroutine RunCoroutine(this IEnumerator enumerator, float delay) {
            return B_CoroutineControl.Queue.RunCoroutine(enumerator, delay);
        }

        /// <summary>
        /// Runs the enumarator
        /// </summary>
        /// <param name="enumerator"></param>
        /// <param name="coroutine"></param>
        public static void RunCoroutine(this IEnumerator enumerator, Coroutine coroutine) {
            B_CoroutineControl.Queue.RunCoroutine(enumerator, coroutine);
        }

        /// <summary>
        /// Runs the enumarator with delay
        /// </summary>
        /// <param name="enumerator"></param>
        /// <param name="coroutine"></param>
        /// <param name="delay"></param>
        public static void RunCoroutine(this IEnumerator enumerator, Coroutine coroutine, float delay) {
            B_CoroutineControl.Queue.RunCoroutine(enumerator, coroutine, delay);
        }

        /// <summary>
        /// Runs the function in a coroutine
        /// </summary>
        /// <param name="enumerator"></param>
        /// <param name="delay"></param>
        public static Coroutine RunWithDelay(Action method, float delay) {
            return B_CoroutineControl.Queue.RunFunctionWithDelay(method, delay);
        }
        
        public static Coroutine StopCoroutine(this Coroutine coroutine) {
            return B_CoroutineControl.Queue.StopCoroutine(coroutine);
        }
        
        #endregion
        
        

        #region Collider Extentions

        public static Vector3 GetRandomPoint(this Collider collider) {
            return new Vector3(
                Random.Range(collider.bounds.min.x, collider.bounds.max.x),
                Random.Range(collider.bounds.min.y, collider.bounds.max.y),
                Random.Range(collider.bounds.min.z, collider.bounds.max.z)
            );
        }
        
        public static Vector3 GetRandomPoint(this Collider collider, float extends) {
            return new Vector3(
                Random.Range(collider.bounds.min.x, collider.bounds.max.x) * extends,
                Random.Range(collider.bounds.min.y, collider.bounds.max.y) * extends,
                Random.Range(collider.bounds.min.z, collider.bounds.max.z) * extends
            );
        }

        #endregion
        
    }
}

[Serializable]
public class ScriptableObjectSaveInfo {
    [HideInInspector] public ScriptableObject obj;
    public string foldername, filename;
    public ScriptableObjectSaveInfo(ScriptableObject obj, string foldername, string filename) {
        this.obj = obj;
        this.foldername = foldername;
        this.filename = filename;
    }

    public void ModifyInfo(ScriptableObject obj, [CanBeNull] string foldername, [CanBeNull] string filename) {
        if (obj) this.obj = obj;
        if (foldername.Length > 0) this.foldername = foldername;
        if (filename.Length > 0) this.filename = filename;
    }

    public ScriptableObjectSaveInfo(ScriptableObject scriptableObject) : this(scriptableObject, null, null) { }
    public ScriptableObjectSaveInfo(ScriptableObject scriptableObject, string filename) : this(scriptableObject, null, filename) { }
}