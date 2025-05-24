using TestGame.Managers;

namespace TestGame.Core;

public class CriticalErrorHandler
{
    public CriticalErrorHandler(CollisionMonitor monitor, SoundManager soundManager)
    {
        monitor.OnCriticalError += () =>
        {
            soundManager.PlaySound("CriticalError", volume: 0.5f);
            System.Threading.Tasks.Task.Delay(10000).ContinueWith(_ =>
            {
                CriticalErrorDialog.Show();
            });
        };
    }
}