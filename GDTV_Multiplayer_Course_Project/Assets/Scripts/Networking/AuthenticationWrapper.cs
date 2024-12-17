using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using UnityEngine;

public static class AuthenticationWrapper
{
    public static AuthState AuthState { get; private set; } = default;

    public static async Task<AuthState> DoAuth(int _maxTries = 5)
    {
        if (AuthState == AuthState.Authenticated)
        {
            return AuthState;
        }

        AuthState = AuthState.Authenticating;
        int _tries = 0;

        while (AuthState == AuthState.Authenticating && _tries < _maxTries)
        {
            var _authService = AuthenticationService.Instance;

            await _authService.SignInAnonymouslyAsync();

            if (_authService.IsSignedIn && _authService.IsAuthorized)
            {
                AuthState = AuthState.Authenticated;
                break;
            }

            _tries++;
            await Task.Delay(1000);
        }

        return AuthState;
    }
}

public enum AuthState
{
    NotAuthenticated,
    Authenticating,
    Authenticated,
    Error,
    TimeOut,
}