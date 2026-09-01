using UnityEngine;
using Atlas.DB;
using Atlas.Utility;
using Atlas.Systems;
using Atlas.UI;
using System.Collections;
using DG.Tweening;

namespace Atlas.Core 
{
    public class Gamemaster : PersistentSingleton<Gamemaster>
    {
        private StagingSystem _stagingSystem;
        private GameStateSystem _gameStateSystem;
        private TransitionView _transitionView;

        private bool _isLoaded;

        public TransitionView TransitionView { get => _transitionView; set => _transitionView = value; }

        protected override void Awake()
        {
            base.Awake();

            _stagingSystem = GetComponent<StagingSystem>();
            _transitionView = GetComponent<TransitionView>();
            if(_gameStateSystem == null)
                _gameStateSystem = GetComponent<GameStateSystem>();
        }

        public void LoadGameScene(string sceneName)
        {
            if(sceneName == "BattleSystem")
            {
                _gameStateSystem.SetState(EGameState.Campfire);
            }
            else if(sceneName == "CharacterCreator")
            {
                _gameStateSystem.SetState(EGameState.CharacterCreator);
            }
            else
            {
                _gameStateSystem.SetState(EGameState.MainMenu);
            }
        }

        public IEnumerator RunGame(bool isLoaded)
        {
            yield return _transitionView.FadeIn(0).WaitForCompletion();
            _isLoaded = isLoaded;
            _stagingSystem.StageScene(1);
        }

        public void GoToCharacterCreator()
        {
            _stagingSystem.StageScene(3);
        }

        public void EnterCampsite()
        {
            FindAnyObjectByType<RestSystem>().EnterCampsite();
        }

        public void EnterBattle()
        {
            // FindAnyObjectByType<AtlasBattleSystem>().RunBattle();
        }

        public void ExitGame()
        {
            _stagingSystem.StageScene(0);
        }

        public void GameOver()
        {
            _stagingSystem.StageScene(2);
        }

        public void GameInitialize()
        {
        }

        public void TitleScreenInitialize()
        {
            _transitionView.FadeOut();

            FindAnyObjectByType<TitleScreenView>().Initialize();
        }

        public void QuitGame()
        {
            Application.Quit();
        }

        private void OnEnable()
        {
            _stagingSystem.OnGameSceneStaged.AddListener(LoadGameScene);
        }
    }
}