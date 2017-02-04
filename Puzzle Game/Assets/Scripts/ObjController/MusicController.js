#pragma strict
import UnityEngine.SceneManagement;
public class MusicController extends MonoBehaviour
 {
     public static var instance : MusicController;
     
     public var musicMainMenu : AudioClip;
     public var musicLevel : AudioClip;

     private var prevSceneName : String;
     function Awake() 
     {
         if ( instance != null && instance != this ) 
         {
             Destroy( this.gameObject );
             return;
         } 
         else 
         {
             instance = this;
         }
         
         DontDestroyOnLoad( this.gameObject );
     }

    function OnEnable() {
      SceneManager.sceneLoaded += OnSceneLoaded;
 		 }
 
  	function OnDisable() {
 	     SceneManager.sceneLoaded -= OnSceneLoaded;
  	}
 
 	 private function OnSceneLoaded(scene : Scene, mode : LoadSceneMode) {
   		 if ( SceneManager.GetActiveScene().name == "Startpage")
         {
         	 prevSceneName = "Start";
             GetComponent.<AudioSource>().Stop();
             GetComponent.<AudioSource>().clip = musicMainMenu;
             GetComponent.<AudioSource>().Play();
         }
          else if ( prevSceneName == "Stage" && SceneManager.GetActiveScene().name == "StageSelection" ){
          	 prevSceneName = "StageSelection";
         	 GetComponent.<AudioSource>().Stop();
             GetComponent.<AudioSource>().clip = musicMainMenu;
             GetComponent.<AudioSource>().Play();
         }
          else if (SceneManager.GetActiveScene().name == "StageSelection")
         {
         	prevSceneName = "StageSelection";
         }
         else if (prevSceneName == "StageSelection")
         {
         	 prevSceneName = "Stage";
             GetComponent.<AudioSource>().Stop();
             GetComponent.<AudioSource>().clip = musicLevel;
             GetComponent.<AudioSource>().Play();
         }
         else if ( prevSceneName == "Stage" && SceneManager.GetActiveScene().name != "StageSelection" ){

         }
        
         else {
         	 prevSceneName = "Stage";
         	 GetComponent.<AudioSource>().Stop();
             GetComponent.<AudioSource>().clip = musicMainMenu;
             GetComponent.<AudioSource>().Play();
         }
  	}

     public static function GetInstance() : MusicController
     {
         return instance;
     }
     
 }    
