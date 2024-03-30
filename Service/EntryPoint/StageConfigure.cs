using AnnulusGames.LucidTools.Audio;
using MessagePipe;
using u1w_2024_3.Src.Model;
using u1w_2024_3.Src.Model.Message;
using u1w_2024_3.Src.Service.Input;
using u1w_2024_3.Src.View;
using u1w_2024_3.Src.View.Camera;
using u1w_2024_3.Src.View.Player;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;
using VContainer.Unity;

namespace u1w_2024_3.Src.Service.EntryPoint
{
    public sealed class StageConfigure : LifetimeScope
    {
        [SerializeField] private PlayerCore _playerCore;
        [SerializeField] private MainCameraMovement _mainCameraMovement;
        [SerializeField] private MainCameraAnimation _mainCameraAnimation;
        [SerializeField] private SoundRepository _soundRepository;
        [SerializeField] private WallRepository _wallRepository;
        [SerializeField] private Wall _wallPrefab;
        [SerializeField] private TutorialSprite _tutorialSprite;
        [SerializeField] private PlayerSpawner _playerSpawner;
        [SerializeField] private CameraMaxMovement _cameraMaxMovement;


        protected override void Configure(IContainerBuilder builder)
        {
            var options = builder.RegisterMessagePipe();

            builder.RegisterMessageBroker<ChangeGravityDirectionEvent>(options);
            builder.RegisterMessageBroker<GameStateEvent>(options);
            builder.RegisterMessageBroker<CamMovementEvent>(options);

            builder.Register<StageField>(Lifetime.Singleton);
            builder.Register<PlayerRepository>(Lifetime.Singleton);
            builder.Register<LevelLoader>(Lifetime.Singleton);
            builder.RegisterEntryPoint<GameInputProvider>().As<IGameInputProvider>();
            builder.RegisterEntryPoint<StageEventService>();
            builder.RegisterEntryPoint<PlayerChangeService>();
            builder.RegisterEntryPoint<UIInputProvider>().As<IUIInputProvider>();

            builder.RegisterComponent(_mainCameraMovement);
            builder.RegisterComponent(_mainCameraAnimation);
            builder.RegisterComponent(_playerSpawner);
            builder.RegisterComponent(_soundRepository);
            builder.RegisterComponent(_wallRepository);
            builder.RegisterComponent(_wallPrefab);
            builder.RegisterComponent(_cameraMaxMovement);

            // playerCoreとplayerMovementのinjectを解決したPrefabを登録する
            builder.RegisterFactory<PlayerInfoByColor, Vector3, PlayerActiveState, PlayerCore>(c =>
                    (info, position, state) =>
                    {
                        var player = c.Instantiate(_playerCore);
                        player.transform.position = position;
                        var playerMovement = player.GetComponent<PlayerMovement>();
                        c.Inject(playerMovement);
                        player.Init(info, new PlayerId(), state);
                        return player;
                    },
                Lifetime.Singleton);

            // sceneの名前から"Stage"を取り除いたものをLevelとして登録する
            if (SceneManager.GetActiveScene().name.Contains("Level"))
            {
                var level = int.Parse(SceneManager.GetActiveScene().name.Replace("Level", ""));
                Level.CurrentLevel = level;
            }

            if (Level.CurrentLevel == 0)
            {
                builder.RegisterComponent(_tutorialSprite);
                builder.RegisterEntryPoint<Tutorial0Service>();
            }
            else if (Level.CurrentLevel == 4)
            {
                builder.RegisterComponent(_tutorialSprite);
                builder.RegisterEntryPoint<Tutorial4Service>();
            }

            if (!Level.IsMaxLevel)
            {
                builder.RegisterEntryPoint<OutOfStageWallDeactivator>();
            }
        }
    }
}