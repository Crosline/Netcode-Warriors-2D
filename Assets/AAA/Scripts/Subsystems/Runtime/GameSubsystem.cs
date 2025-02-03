using System;
using Serializables;

namespace Subsystems
{
    public abstract class GameSubsystem<T> : GameSubsystem where T : GameSubsystem<T>
    {
        public static T Instance { get; private set; }

        public override void Initialize()
        {
            Instance ??= this as T;
        }

        public override void Shutdown()
        {
            if (Instance != this) return;

            Instance = null;
        }


        public override void OnAwake()
        {
        }

        public override void OnDestroy()
        {
        }

        public override void OnApplicationQuit()
        {
        }

        public override void OnApplicationFocus(bool hasFocus)
        {
        }

        public override void OnApplicationPause(bool hasPaused)
        {
        }
    }

    [Serializable]
    public class GameSubsystem
    {
        public virtual void Initialize()
        {
        }

        public virtual void Shutdown()
        {
        }

        public virtual void OnAwake()
        {
        }

        public virtual void OnDestroy()
        {
        }

        public virtual void OnApplicationQuit()
        {
        }

        public virtual void OnApplicationFocus(bool hasFocus)
        {
        }

        public virtual void OnApplicationPause(bool hasPaused)
        {
        }
    }
}