//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;
//using UnityEngine.SceneManagement;
//using DigitalRuby.SimpleLUT;
//
//public class MenuManager : MonoBehaviour {
//
//  //  public bool pause = false;
//    [Header("===SliderSetting===")]
//    public RectTransform bright;
//    public RectTransform contrast;
//    public RectTransform music;
//    public RectTransform sound;
//    public RectTransform mouseX;
//    public RectTransform mouseY;
//    public RectTransform mouseZoom;
//    [Header("===Slider Parameters===")]
//    public float globalVolume;
//    public float brightNess;
//    public float contrastNess;
//    public float musicVolume;
//    public float soundVolume;
//    public float rotateXSpeed;
//    public float rotateYSpeed;
//    public float zoomSpeed;
//    [Header("===ToggleOptions===")]
//    public RectTransform fullScreen;
//    public RectTransform aniso;
//    public RectTransform antiAlias;
//    public RectTransform sync;
//    public RectTransform shadow;
//    public RectTransform quality;
//    public RectTransform resolution;
//    [Header("===ToggleList===")]
//    public string[] textureQuality=new string[4] {"LOW","MEDIUM","HIGH","VERY HIGH" };
//    public string[] screenResolution = new string[4] {"800X600","1024X768","1366X768","1920X1080" };
//    [Header("===SoundSetting===")]
//    public AudioClip click;
//    [SerializeField]private AudioSource voice;
//    public AudioSource[] BGM;
//    public string mainMenu="MainMenu";
//    public RectTransform cur_menu;
//	void Start ()
//    {
//        voice = GetComponent<AudioSource>();
//        if (fullScreen == null || aniso == null || antiAlias == null || sync == null||shadow==null
//            ||click==null ||voice==null)
//            Debug.LogError("null toggles");
//        if (bright == null || contrast == null || music == null || sound == null ||mouseX==null
//            ||mouseY==null||mouseZoom==null)
//            Debug.LogError("null sliders");
//        if (mainMenu == null ||cur_menu==null ) Debug.LogError("null menu component");
//        brightNess = bright.GetComponent<Slider>().value;
//        contrastNess = contrast.GetComponent<Slider>().value;
//        musicVolume = music.GetComponent<Slider>().value;
//        soundVolume = sound.GetComponent<Slider>().value;
//        rotateXSpeed = mouseX.GetComponent<Slider>().value;
//        rotateYSpeed = mouseY.GetComponent<Slider>().value;
//        zoomSpeed = mouseZoom.GetComponent<Slider>().value;
//      //  cur_menu.sizeDelta = new Vector2(1f, 1f) * cursorSize * 0.3f;
//        cur_menu.gameObject.SetActive(true);
//        Cursor.visible = false;
//    }
//	
//	// Update is called once per frame
//	void Update ()
//    {
//
//        //if (pause) Time.timeScale = 0;
//        //else Time.timeScale = 1;
//       // Cursor.visible = false;
//        globalVolume = AudioListener.volume;
//        cur_menu.position = Input.mousePosition;
//    }
//
//
//
//    public void PlayClick()
//    {
//        //if (voice.isPlaying == false)
//            voice.PlayOneShot(click);
//
//    }
//
//    public void DisableMenuCursor()
//    {
//        cur_menu.gameObject.SetActive(false);
//    }
//
//    public void EnableMenuCursor()
//    {
//        cur_menu.gameObject.SetActive(true);
//
//    }
//
//    public void ReturnToMainMenu()
//    {
//
//        SceneManager.LoadScene(mainMenu);
//    }
//
//    public void QuitGame()
//    {
//        Application.Quit();
//    }
//
//
//    public void SetBright()
//    {
//        brightNess = bright.GetComponent<Slider>().value;
//        Camera.main.GetComponent<SimpleLUT>().Brightness = brightNess-1f;
//    }
//
//    public void SetContrast()
//    {
//        contrastNess = contrast.GetComponent<Slider>().value;
//        Camera.main.GetComponent<SimpleLUT>().Contrast = contrastNess - 1f;
//
//    }
//    public void SetMusicVolume()
//    {
//        musicVolume = music.GetComponent<Slider>().value;
//        foreach (var voice in BGM)
//        {
//            voice.volume = musicVolume;
//
//        }
//
//    }
//    public void SetSoundVolume()
//    {
//        soundVolume = sound.GetComponent<Slider>().value;
//        AudioListener.volume = soundVolume;
//
//    }
//    public void SetMouseX()
//    {
//        rotateXSpeed = mouseX.GetComponent<Slider>().value;
//        if (Camera.main.GetComponent<ThirdPersonCamera>()!=null)
//        Camera.main.GetComponent<ThirdPersonCamera>().SetRotateSpeedH(rotateXSpeed);
//
//    }
//
//    public void SetMouseY()
//    {
//        rotateYSpeed = mouseY.GetComponent<Slider>().value;
//        if (Camera.main.GetComponent<ThirdPersonCamera>() != null)
//            Camera.main.GetComponent<ThirdPersonCamera>().SetRotateSpeedV(rotateYSpeed);
//    }
//
//    public void SetMouseZoom()
//    {
//        zoomSpeed = mouseZoom.GetComponent<Slider>().value;
//        if (Camera.main.GetComponent<ThirdPersonCamera>() != null)
//            Camera.main.GetComponent<ThirdPersonCamera>().SetZoomSpeed(zoomSpeed);
//    }
//
//
//    public void ToggleFullScreen()
//    {
//        string toggle = fullScreen.gameObject.GetComponent<Text>().text;
//        if (toggle == "off") fullScreen.gameObject.GetComponent<Text>().text= "on";
//        else fullScreen.gameObject.GetComponent<Text>().text="off";
//        PlayClick();
//    }
//    public void ToggleAniso()
//    {
//        string toggle = aniso.gameObject.GetComponent<Text>().text;
//        if (toggle == "off") aniso.gameObject.GetComponent<Text>().text = "on";
//        else aniso.gameObject.GetComponent<Text>().text = "off";
//        PlayClick();
//    }
//    public void ToggleAntiAlias()
//    {
//        string toggle = antiAlias.gameObject.GetComponent<Text>().text;
//        if (toggle == "off") antiAlias.gameObject.GetComponent<Text>().text = "on";
//        else antiAlias.gameObject.GetComponent<Text>().text = "off";
//        PlayClick();
//    }
//    public void ToggleSync()
//    {
//        string toggle = sync.gameObject.GetComponent<Text>().text;
//        if (toggle == "off") sync.gameObject.GetComponent<Text>().text = "on";
//        else sync.gameObject.GetComponent<Text>().text = "off";
//        PlayClick();
//    }
//    public void ToggleShadow()
//    {
//        string toggle = shadow.gameObject.GetComponent<Text>().text;
//        if (toggle == "off") shadow.gameObject.GetComponent<Text>().text = "on";
//        else shadow.gameObject.GetComponent<Text>().text = "off";
//        PlayClick();
//
//    }
//    public void ToggleQuality()
//    {
//        Text toggle = quality.gameObject.GetComponent<Text>();
//        switch (toggle.text)
//        {
//            case "LOW":
//                toggle.text = textureQuality[1];
//                break;
//            case "MEDIUM":
//                toggle.text = textureQuality[2];
//                break;
//            case "HIGH":
//                toggle.text = textureQuality[3];
//                break;
//            case "VERY HIGH":
//                toggle.text = textureQuality[0];
//                break;
//
//        }
//        PlayClick();
//    }
//
//    public void ToggeResolution()
//    {
//
//        Text toggle = resolution.gameObject.GetComponent<Text>();
//        switch (toggle.text)
//        {
//            case "800X600":
//                toggle.text = screenResolution[1];
//                break;
//            case "1024X768":
//                toggle.text = screenResolution[2];
//                break;
//            case "1366X768":
//                toggle.text = screenResolution[3];
//                break;
//            case "1920X1080":
//                toggle.text = screenResolution[0];
//                break;
//
//        }
//        PlayClick();
//
//    }
//
//}
