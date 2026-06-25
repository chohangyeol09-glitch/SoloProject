namespace _02.Scripts.CoreSystem.ServiceLocatorSystem.Interfaces
{
    public interface IAudioService
    {
        void PlaySFX(string clipName);
        void PlayBGM(string clipName);
        void StopBGM();
    }
}