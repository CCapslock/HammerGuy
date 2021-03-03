using UnityEngine;

public class ParticleSystemDestroyer : MonoBehaviour
{
    public void DestroyParticles()
    {
        Invoke("DestroyMethod", 1f);
    }
    private void DestroyMethod()
    {
        Destroy(this.gameObject);
    }
}
