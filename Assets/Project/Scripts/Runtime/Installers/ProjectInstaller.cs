using OMG.Configs;
using OMG.Models;
using OMG.Presenters;
using OMG.Services;
using OMG.Views;
using UnityEngine;
using Zenject;

namespace OMG.Installers
{
    public sealed class ProjectInstaller : MonoInstaller
    {
        [Header("Views")]
        [SerializeField] private PuzzleGalleryView _galleryView;
        [SerializeField] private PuzzleDialogView _dialogView;

        [Header("Config")]
        [SerializeField] private PuzzleStartPolicy _puzzleStartPolicyAsset;

        public override void InstallBindings()
        {
            // Services
            Container.Bind<IPreviewService>().To<MockPreviewService>().AsSingle();
            Container.Bind<ILoadingService>().To<MockLoadingService>().AsTransient();
            Container.Bind<IAdsService>().To<MockAdsService>().AsSingle();

            // Models
            Container.BindInterfacesAndSelfTo<Wallet>().AsSingle();
            Container.Bind<PuzzleStartPolicy>().FromInstance(_puzzleStartPolicyAsset).AsSingle();
            Container.Bind<IStartCostPolicy>().FromInstance(new PuzzleStartPolicyAdapter(_puzzleStartPolicyAsset)).AsSingle();
            
            // Views
            Container.Bind<IPuzzleGalleryView>().FromInstance(_galleryView).AsSingle();
            Container.Bind<IPuzzleDialogView>().FromInstance(_dialogView).AsSingle();

            // Presenters
            Container.Bind<GalleryPresenter>().AsSingle().NonLazy();
            Container.BindFactory<PuzzleMeta, PuzzleDialogPresenter, PuzzleDialogPresenter.Factory>()
                .FromFactory<PuzzleDialogPresenter.FactoryImpl>();
        }
    }
}