using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Serializables;
using Object = UnityEngine.Object;

namespace Subsystems.Core
{
    public sealed class SubsystemManager
    {
        private static SubsystemManagerMonoHelper _monoHelper;

        private static SubsystemManagerSettings _settings;

        private static CancellationTokenSource _cancellationTokenSource;

        private static HashSet<GameSubsystem> _subsystems;
        internal static SubsystemManager Instance { get; private set; }


        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void OnBeforeSceneLoad()
        {
            if (Instance != null) return;

            Application.quitting += OnApplicationQuit;
            Initialize();
        }

        private static void OnApplicationQuit()
        {
            Application.quitting -= OnApplicationQuit;
            __OnApplicationQuit();
            Shutdown();
        }

        private static void Initialize()
        {
            Instance = new SubsystemManager();
            _cancellationTokenSource = new CancellationTokenSource();
            _settings = Resources.Load<SubsystemManagerSettings>(nameof(SubsystemManagerSettings));

            _subsystems = new HashSet<GameSubsystem>();

            foreach (var gameSubsystem in _settings.Subsystems)
            {
                if (gameSubsystem.Type == null) continue;

                if (Activator.CreateInstance(gameSubsystem.Type) is not GameSubsystem singletonObject)
                    continue;

                if (!_subsystems.Add(singletonObject)) continue;

                singletonObject.Initialize();
            }

            _monoHelper = new GameObject(nameof(SubsystemManagerMonoHelper))
                .AddComponent<SubsystemManagerMonoHelper>();
        }

        private static void Shutdown()
        {
            Instance = null;

            if (_cancellationTokenSource != null)
            {
                _cancellationTokenSource.Cancel(true);
                _cancellationTokenSource.Dispose();
                _cancellationTokenSource = null;
            }

            foreach (var singleton in _subsystems) singleton.Shutdown();

            _subsystems.Clear();
            _subsystems = null;

            if (_monoHelper != null)
                Object.Destroy(_monoHelper.gameObject);
        }

        internal static void OnAwake()
        {
            foreach (var singleton in _subsystems)
            {
                _cancellationTokenSource.Token.ThrowIfCancellationRequested();
                singleton.OnAwake();
            }
        }

        internal static void OnDestroy()
        {
            foreach (var singleton in _subsystems)
            {
                _cancellationTokenSource.Token.ThrowIfCancellationRequested();
                singleton.OnDestroy();
            }
        }

        internal static void OnApplicationFocus(bool hasFocus)
        {
            foreach (var singleton in _subsystems)
            {
                _cancellationTokenSource.Token.ThrowIfCancellationRequested();
                singleton.OnApplicationFocus(hasFocus);
            }
        }

        internal static void OnApplicationPause(bool hasPaused)
        {
            foreach (var singleton in _subsystems)
            {
                _cancellationTokenSource.Token.ThrowIfCancellationRequested();
                singleton.OnApplicationPause(hasPaused);
            }
        }

        private static void __OnApplicationQuit()
        {
            foreach (var singleton in _subsystems)
            {
                _cancellationTokenSource.Token.ThrowIfCancellationRequested();
                singleton.OnApplicationQuit();
            }
        }
    }
}