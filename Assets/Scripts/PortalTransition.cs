using UnityEngine;

   public class PortalTransition: MonoBehaviour
   {
       public int nextSceneIndex = 1; // Индекс следующей сцены (из Build Settings)

       private void OnTriggerEnter2D(Collider2D other)
       {
           if (other.CompareTag("Player"))
           {
               // Условие: игрок касается двери
               SceneManager.Instance.LoadScene(nextSceneIndex);
           }
       }
   }