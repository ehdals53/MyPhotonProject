using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Fusion.Sockets;

public class WeaponHandler : NetworkBehaviour
{
    [Networked(OnChanged =nameof(OnFireChanged))]
    public bool isFiring { get; set; }

    public ParticleSystem fireParticleSystem;

    float lastTimeFired = 0;
    // Start is called before the first frame update
    public override void FixedUpdateNetwork()
    {
        if(GetInput(out NetworkInputData networkInputData))
        {
            if (networkInputData.isFirePressed)
            {
                Fire(networkInputData.aimForwardVector);
            }
        }
    }
    void Fire(Vector3 aimForwardVector)
    {
        if (Time.time - lastTimeFired < 0.15f)
            return;

        StartCoroutine(FireEffectCO());
        lastTimeFired = Time.time;
    }
    IEnumerator FireEffectCO()
    {
        isFiring = true;

        fireParticleSystem.Play();

        yield return new WaitForSeconds(0.09f);

        isFiring = false;
    }
    static void OnFireChanged(Changed<WeaponHandler> changed)
    {
        Debug.Log($"{Time.time} OnFireChanged value {changed.Behaviour.isFiring}");

        bool isFireingCurrent = changed.Behaviour.isFiring;
        changed.LoadOld();

        bool isFiringOld = changed.Behaviour.isFiring;

        if (isFireingCurrent && isFiringOld)
            changed.Behaviour.OnFireRemote();
    }
    void OnFireRemote()
    {
        if (!Object.HasInputAuthority)
            fireParticleSystem.Play();

    }
}
