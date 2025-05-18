using UnityEngine;

     public class DontDestroyCanvas : MonoBehaviour
     {
         void Awake()
         {
             DontDestroyOnLoad(gameObject);
         }
     }