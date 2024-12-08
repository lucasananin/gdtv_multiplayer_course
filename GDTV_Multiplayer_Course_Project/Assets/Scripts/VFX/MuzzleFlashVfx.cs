using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MuzzleFlashVfx : NetworkBehaviour
{
    [SerializeField] ProjectileLauncher _projectileLauncher = null;
    [SerializeField] SpriteRenderer _renderer = null;
    [SerializeField] float _duration = 0.1f;

    private void OnEnable()
    {
        _renderer.enabled = false;
        _projectileLauncher.OnShoot += PlayAll;
    }

    private void OnDisable()
    {
        _projectileLauncher.OnShoot -= PlayAll;
    }

    private void PlayAll()
    {
        Play();
        Play_ServerRpc();
        Play_ClientRpc();
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

    [ServerRpc]
    public void Play_ServerRpc()
    {
        Play();
    }

    [ClientRpc]
    private void Play_ClientRpc()
    {
        Play();
    }
}
