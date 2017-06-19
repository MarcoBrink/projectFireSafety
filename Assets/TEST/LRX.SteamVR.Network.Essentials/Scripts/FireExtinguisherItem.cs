using UnityEngine;
using UnityEngine.Networking;

public class FireExtinguisherItem : NetworkBehaviour, IUsable {

    public void StartUsing(NetworkInstanceId handId)
    {
        if (!isServer)
        {
            return;
        }
        RpcStartEmitting();
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
