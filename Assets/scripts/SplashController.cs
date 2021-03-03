using UnityEngine;

public class SplashController : MonoBehaviour
{
    public float SplashLifeTime;

    private Vector3 SplashScale;
    private float timeCounter;
   public void ActivateSplash()
    {
        for (int i = 0; i < 100; i++)
        {
            timeCounter = SplashLifeTime + i * 0.01f;
            Invoke("MakeSplashSmaller", timeCounter);
        }
        Invoke("DestroySplash", timeCounter);
    }
    private void MakeSplashSmaller()
    {
        SplashScale = transform.localScale;
        SplashScale.x -= 0.01f;
        SplashScale.y -= 0.01f;
        SplashScale.z -= 0.01f;
        transform.localScale = SplashScale;
    }
    private void DestroySplash()
    {
        Destroy(this.gameObject);
    }
}
