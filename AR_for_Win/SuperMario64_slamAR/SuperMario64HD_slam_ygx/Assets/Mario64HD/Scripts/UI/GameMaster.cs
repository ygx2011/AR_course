/*************************************************************************************

 * 游研堂: www.gamedev3d.com

 *（1）本站致力于为广大的游戏从业者提供相关的资源素材与资讯。

 *（2）本站会持续更新更多相关的资源素材，为游戏领域开发者提供更好的资讯与灵感！

 *（3）本站所有资源素材仅供学习参考，切勿用作商业用途，并请在下载后的24小时内进行删除，

 *     否则由此引发的法律纠纷及连带责任本站和发布者概不承担。
 
*************************************************************************************/
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameMaster : MonoBehaviour {

    public bool DebugGUI;

    public float FadeInTime = 0.3f;
    public float ExitFadeOutTime = 0.5f;

    public AudioSource MusicSource;
    public AudioSource SoundSource;

    public MarioInput MarioIn;
    public KeybindingInputHandler Handler;

    public GameObject DefaultInputs;

    public GameObject PauseMenu;
    public GameObject PauseCamera;
    public AudioClip CoinSound;
    public AudioClip PauseSound;
    public AudioClip BowserLaugh;
    public MatteFade WhiteMatte;
    public MatteFade BlackMatte;
    public BowserMask DeathMask;
    public Text FramerateText;
    public int GoldMarioCoinAmount;

    private int currentCoins;
    private CoinTextHandler coinTextHandler;

    private bool paused;

    private InputManager inputManager;

    private Camera mainCamera;

    void Awake()
    {
        Application.targetFrameRate = 150;

        // There can be only one!
        if (GameObject.FindObjectsOfType<GameMaster>().Length > 1)
        {
            Debug.LogError("Multiple GameMaster components detected in the scene. Limit 1 GameMaster per scene");
        }

        coinTextHandler = GetComponent<CoinTextHandler>();

        coinTextHandler.UpdateValue(currentCoins);

        inputManager = GameObject.FindObjectOfType<InputManager>();

        if (inputManager == null)
        {
            inputManager = ((GameObject)Instantiate(DefaultInputs, Vector3.zero, Quaternion.identity)).GetComponent<InputManager>();
        }
        
        MarioIn.input = inputManager;
        Handler.input = inputManager;

        PauseMenu.SetActive(false);
        PauseCamera.SetActive(false);

        mainCamera = Camera.main;

        WhiteMatte.FadeIn(FadeInTime);

        if (!DebugGUI)
            FramerateText.gameObject.SetActive(false);

        MusicSource.timeSamples = (int)(1 * MusicSource.clip.frequency);
    }

    private float lastFrameRateUpdate;

    void Update()
    {
        if (inputManager.PauseDown())
        {
            if (paused)
                Unpause();
            else
                Pause();
        }

        if ((float)MusicSource.timeSamples / (float)MusicSource.clip.frequency > 140.7f)
        {
            MusicSource.timeSamples = (int)(72.775f * MusicSource.clip.frequency);
        }

        if (DebugGUI && SuperMath.Timer(lastFrameRateUpdate, 0.25f))
        {
            lastFrameRateUpdate = Time.time;
            FramerateText.text = (1.0f / Time.deltaTime).ToString("F0");
        }
    }

    bool[] wasPlaying;

    private void Pause()
    {
        AudioSource[] allSources = GameObject.FindObjectsOfType<AudioSource>();

        wasPlaying = new bool[allSources.Length];

        for (int i = 0; i < allSources.Length; i++)
        {
            var source = allSources[i];

            if (source.isPlaying)
            {
                wasPlaying[i] = true;
                source.Pause();
            }
        }

        MarioIn.enabled = false;

        Time.timeScale = 0;
        paused = true;
        PauseMenu.SetActive(true);
        PauseCamera.SetActive(true);
        mainCamera.enabled = false;
        inputManager.UpdateKeyBindings();

        SoundSource.PlayOneShot(PauseSound);
    }

    private void Unpause()
    {
        AudioSource[] allSources = GameObject.FindObjectsOfType<AudioSource>();

        for (int i = 0; i < allSources.Length; i++)
        {
            var source = allSources[i];

            if (wasPlaying[i])
            {
                source.Play();
            }
        }

        MarioIn.enabled = true;

        Time.timeScale = 1;
        paused = false;
        PauseMenu.SetActive(false);
        PauseCamera.SetActive(false);
        mainCamera.enabled = true;

        SoundSource.PlayOneShot(PauseSound);
    }

    public void ClosePauseMenu()
    {
        Unpause();
    }

    public void ExitToMainMenu()
    {
        StartCoroutine(ExitToMenu());
    }

    public void GameOver()
    {
        StartCoroutine(EndGame());
    }

    IEnumerator EndGame()
    {
        MarioIn.enabled = false;

        yield return new WaitForSeconds(1.0f);

        SoundSource.PlayOneShot(BowserLaugh);

        DeathMask.PlayMask(1.5f);

        StartCoroutine(FadeOutMusic(1.7f));

        yield return new WaitForSeconds(1.5f);

        BlackMatte.gameObject.SetActive(true);

        yield return new WaitForSeconds(1.0f);

        Application.LoadLevel(0);
    }

    IEnumerator FadeOutMusic(float time)
    {
        float i = 0;

        float initialVolume = MusicSource.volume;

        while (i < 1)
        {
            MusicSource.volume = Mathf.Lerp(initialVolume, 0, i);

            i += Time.deltaTime / time;

            yield return 0;
        }
    }

    public void AddCoin()
    {
        currentCoins = Mathf.Clamp(currentCoins + 1, 0, 999);

        SoundSource.PlayOneShot(CoinSound);

        coinTextHandler.UpdateValue(currentCoins);

        if (currentCoins == GoldMarioCoinAmount)
        {
            GameObject.FindObjectOfType<MarioMachine>().GoldMarioUpgrade();
        }
    }

    public void AddCoin(int coins)
    {
        StartCoroutine(AddMultipleCoins(coins));
    }

    public void FadeWhiteMatteOut(float time)
    {
        WhiteMatte.FadeOut(time);
    }

    public void FadeWhiteMatteIn(float time)
    {
        WhiteMatte.FadeIn(time);
    }

    IEnumerator ExitToMenu()
    {
        Time.timeScale = 1;

        SoundSource.PlayOneShot(PauseSound);

        BlackMatte.FadeOut(ExitFadeOutTime);

        yield return new WaitForSeconds(ExitFadeOutTime);

        Application.LoadLevel("StarMenu");
    }

    IEnumerator AddMultipleCoins(int coins)
    {
        int remainingCoins = coins;

        float delay = 0.02f;

        float i = 1.1f;

        while (remainingCoins > 0)
        {
            while (i < 1.0f)
            {
                i += Time.deltaTime / delay;

                yield return 0;
            }

            SoundSource.PlayOneShot(CoinSound);

            remainingCoins--;
            currentCoins = Mathf.Clamp(currentCoins + 1, 0, 999);

            coinTextHandler.UpdateValue(currentCoins);

            if (currentCoins == GoldMarioCoinAmount)
            {
                GameObject.FindObjectOfType<MarioMachine>().GoldMarioUpgrade();
            }

            i = 0;
        }
    }
}
