using OMG.Configs;
using OMG.Models;
using OMG.Presenters;
using OMG.Services;
using OMG.Views;
using UnityEngine;
using Zenject;

namespace OMG.Installers
{
    public sealed class GameInstaller : MonoInstaller
    {
        [Header("Views")]
        [SerializeField] private PuzzleGalleryView _galleryView;
        [SerializeField] private PuzzleDialogView _dialogView;
        [SerializeField] private WalletView _walletView;

        [Header("Config")]
        [SerializeField] private PuzzleStartPolicy _puzzleStartPolicyAsset;

        public override void InstallBindings()
        {
            // Services
            Container.Bind<IPreviewService>().To<MockPreviewService>().AsSingle();
            Container.Bind<ILoadingService>().To<MockLoadingService>().AsSingle();
            Container.Bind<IAdsService>().To<MockAdsService>().AsSingle();

            // Models
            IPuzzleStartCostPolicy puzzleStartPolicyAdapter = new PuzzleStartPolicyAdapter(_puzzleStartPolicyAsset);
            
            Container.BindInterfacesAndSelfTo<Wallet>().AsSingle();
            Container.Bind<PuzzleStartPolicy>().FromInstance(_puzzleStartPolicyAsset).AsSingle();
            Container.Bind<IPuzzleStartCostPolicy>().FromInstance(puzzleStartPolicyAdapter).AsSingle();
            
            // Views
            Container.Bind<IPuzzleDialogView>().FromInstance(_dialogView).AsSingle();
            Container.Bind<IPuzzleGalleryView>().FromInstance(_galleryView).AsSingle();
            Container.BindInstance(_walletView).AsSingle();

            // Presenters
            Container.BindInterfacesAndSelfTo<WalletPresenter>().AsSingle();
            Container.BindInterfacesAndSelfTo<GalleryPresenter>().AsSingle().NonLazy();

            Container.Bind<IPuzzleStartStrategy>().To<FreeStartStrategy>().AsTransient();
            Container.Bind<IPuzzleStartStrategy>().To<CoinsStartStrategy>().AsTransient();
            Container.Bind<IPuzzleStartStrategy>().To<AdsStartStrategy>().AsTransient();

            Container.BindFactory<PuzzleMeta, PuzzleDialogPresenter, PuzzleDialogPresenter.Factory>()
                .FromFactory<PuzzleDialogPresenter.FactoryImpl>();
        }
    }
}