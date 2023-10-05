using Core.Env;
using Game.Settings;
using UnityEngine;

namespace Game
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private Environment.Env _env;
        [SerializeField] private bool debuglog;

        private void Awake()
        {
            SpecifyEnviroment();
            SpecifySystemsSetting();
        }
        private void SpecifyEnviroment()
        {
            Environment.SetEnvironment(_env);
            if (Environment.ENV == Environment.Env.PROD)
            {
                Debug.unityLogger.logEnabled = debuglog;
            }
        }
        private static void SpecifySystemsSetting()
        {
            Vibration.SetState(SettingManager.Instance.GameSettings.IsVibrationOn);
            Application.targetFrameRate = 60;
            //FirebaseHandler.Init();
        }
    }
}