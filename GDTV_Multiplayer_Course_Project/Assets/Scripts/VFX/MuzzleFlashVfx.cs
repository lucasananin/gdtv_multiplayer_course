using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MuzzleFlashVfx : NetworkBehaviour
{
    [SerializeField] ProjectileLauncher _projectileLauncher = null;
    [SerializeField] SpriteRenderer _renderer = null;
    [SerializeField] float _duration = 0.1f;
    private float _timer = 0f;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        _renderer.enabled = false;
        //if (!IsOwner) return;
        _projectileLauncher.OnShoot += PlayAll;
        //_projectileLauncher.OnShoot += Fodase;
        //_projectileLauncher.OnShoot += PlayClientRpc;
        //_projectileLauncher.OnShoot += PlayServerRpc;
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();
        //if (!IsOwner) return;
        _projectileLauncher.OnShoot -= PlayAll;
        //_projectileLauncher.OnShoot -= Fodase;
        //_projectileLauncher.OnShoot -= PlayClientRpc;
        //_projectileLauncher.OnShoot -= PlayServerRpc;
    }

    //private void Update()
    //{
    //    _timer += Time.deltaTime;

    //    if (_timer > _duration && _renderer.enabled)
    //    {
    //        _renderer.enabled = false;
    //    }
    //}

    private void PlayAll()
    {
        Play();
        PlayServerRpc();
        PlayClientRpc();
    }

    public void Fodase()
    {
        _timer = 0f;
        _renderer.enabled = true;
        //PlayServerRpc();
        //PlayClientRpc();
    }

    [ServerRpc]
    public void PlayServerRpc()
    {
        //Fodase();
        Play();
    }

    [ClientRpc]
    private void PlayClientRpc()
    {
        //Fodase();
        Play();
    }

    private void Play()
    {
        _renderer.enabled = true;
        StopAllCoroutines();
        StartCoroutine(Play_routine());
    }

    private IEnumerator Play_routine()
    {
        yield return new WaitForSeconds(_duration);
        _renderer.enabled = false;
    }
}
