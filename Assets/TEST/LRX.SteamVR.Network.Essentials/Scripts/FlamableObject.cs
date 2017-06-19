using UnityEngine;
using UnityEngine.Networking;

public class FlamableObject : NetworkBehaviour {

    public float Heat;
    public bool OnFire;
    public ParticleSystem.MinMaxCurve Length;

    private void Start()
    {
        
    }

    public void Beingdiffused()
    {
        if (!isServer)
        {
            return;
        }
        ParticleSystem ps = GetComponentInChildren<ParticleSystem>();
        ParticleSystem.MainModule mm = ps.main;
        mm.startSpeed = 1;
    }

    public void StopUsing(NetworkInstanceId handId)
    {
        if (!isServer)
        {
            return;
        }
        RpcStopEmitting();
    }

    [ClientRpc]
    void RpcStartEmitting()
    {

        ParticleSystem ps = GetComponentInChildren<ParticleSystem>();
        ps.Play();
    }

    [ClientRpc]
    void RpcStopEmitting()
    {
        ParticleSystem ps = GetComponentInChildren<ParticleSystem>();
        ps.Stop();
    }
}
