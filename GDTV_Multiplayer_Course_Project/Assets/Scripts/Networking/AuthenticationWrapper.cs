using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

public static class AuthenticationWrapper
{
    public static AuthState AuthState { get; private set; } = default;

    public static async Task<AuthState> DoAuth(int _maxRetries = 5)
    {
        if (AuthState == AuthState.Authenticated)
        {
            return AuthState;
        }

        if (AuthState == AuthState.Authenticating)
        {
            Debug.LogWarning($"Already authenticating!");
            await Authenticating();
            return AuthState;
        }

        await SignInAnonymously_Async(_maxRetries);

        return AuthState;
    }

    private static async Task<AuthState> Authenticating()
    {
        if (AuthState == AuthState.Authenticating || AuthState == AuthState.NotAuthenticated)
        {
            await Task.Delay(200);
        }

        return AuthState;
    }

    private static async Task SignInAnonymously_Async(int _maxRetries)
    {
        AuthState = AuthState.Authenticating;
        int _retries = 0;

        while (AuthState == AuthState.Authenticating && _retries < _maxRetries)
        {
            try
            {
                var _authService = AuthenticationService.Instance;

                await _authService.SignInAnonymouslyAsync();

                if (_authService.IsSignedIn && _authService.IsAuthorized)
                {
                    AuthState = AuthState.Authenticated;
                    break;
                }
            }
            catch (AuthenticationException _authEx)
            {
                AuthState = AuthState.Error;
                throw _authEx;
            }
            catch (RequestFailedException _requestEx)
            {
                AuthState = AuthState.Error;
                throw _requestEx;
            }

            _retries++;
            await Task.Delay(1000);
        }

        if (AuthState != AuthState.Authenticated)
        {
            AuthState = AuthState.TimeOut;
            Debug.LogWarning($"AuthState = TimeOut after {_retries} retries!");
        }
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