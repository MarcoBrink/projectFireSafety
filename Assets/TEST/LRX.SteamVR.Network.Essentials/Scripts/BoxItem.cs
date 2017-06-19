using System;
using UnityEngine;
using UnityEngine.Networking;

public class BoxItem : NetworkBehaviour, IUsable {

    public void StartUsing(NetworkInstanceId handId)
    {
        if(!isServer)
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
        ParticleSystem ps = GetComponent<ParticleSystem>();
        ps.Play();
    }

    [ClientRpc]
    void RpcStopEmitting()
    {
        ParticleSystem ps = GetComponent<ParticleSystem>();
        ps.Stop();
    }
}
