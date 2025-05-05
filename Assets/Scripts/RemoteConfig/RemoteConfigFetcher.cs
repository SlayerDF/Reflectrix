using System;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using Unity.Services.RemoteConfig;
using UnityEngine;

namespace Reflectrix.RemoteConfig
{
    public static class RemoteConfigFetcher
    {
        private static bool isFetching;
        private static bool isFetched;
        private static bool isSubscribed;

        public static RemoteConfig Config { get; private set; } = new()
        {
            deviceQuantity = 3,
            rayIntersectionEnabled = true,
            zoomSpeed = 0.3f,
            panSpeed = 0.03f,
            winTitle = "Objective Accomplished!",
            loseTitle = "Mission Failed!"
        };


        public static event Action<RemoteConfig> OnConfigFetched;
        public static event Action OnFetchFailed;

        public static async Task FetchConfig()
        {
            if (isFetching || isFetched)
            {
                return;
            }

            isFetching = true;

            if (!isSubscribed)
            {
                RemoteConfigService.Instance.FetchCompleted += response =>
                {
                    isFetching = false;
                    ApplyRemoteSettings(response);
                };

                isSubscribed = true;
            }

            if (Unity.Services.RemoteConfig.Utilities.CheckForInternetConnection())
            {
                await InitializeRemoteConfigAsync();
            }

            RemoteConfigService.Instance.FetchConfigs(new UserAttributes(), new AppAttributes());
        }

        private static async Task InitializeRemoteConfigAsync()
        {
            InitializationOptions options = new();

            if (Debug.isDebugBuild || Application.isEditor)
            {
                options.SetEnvironmentName("development");
            }

            await UnityServices.InitializeAsync(options);

            if (!AuthenticationService.Instance.IsSignedIn)
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
            }
        }

        private static void ApplyRemoteSettings(ConfigResponse configResponse)
        {
            if (configResponse.status == ConfigRequestStatus.Success)
            {
                Config = new RemoteConfig
                {
                    deviceQuantity = RemoteConfigService.Instance.appConfig.GetInt("deviceQuantity"),
                    rayIntersectionEnabled = RemoteConfigService.Instance.appConfig.GetBool("rayIntersectionEnabled"),
                    zoomSpeed = RemoteConfigService.Instance.appConfig.GetFloat("zoomSpeed"),
                    panSpeed = RemoteConfigService.Instance.appConfig.GetFloat("panSpeed"),
                    winTitle = RemoteConfigService.Instance.appConfig.GetString("winTitle"),
                    loseTitle = RemoteConfigService.Instance.appConfig.GetString("loseTitle")
                };

                isFetched = true;

                OnConfigFetched?.Invoke(Config);
            }
            else
            {
                OnFetchFailed?.Invoke();
            }
        }

        #region Nested type: ${0}

        private struct UserAttributes
        {
        }

        private struct AppAttributes
        {
        }

        #endregion
    }
}
