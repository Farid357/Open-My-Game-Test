using NSubstitute;
using NUnit.Framework;
using OMG.Configs;
using OMG.Presenters;
using OMG.Models;
using OMG.Services;
using OMG.Tests.TestHelpers;

namespace OMG.Tests.Presenters
{
    public class PuzzleDialogPresenterTests
    {
        [Test]
        public void Presenter_ShowsNotEnoughCoins_WhenWalletInsufficient()
        {
            var view = new TestPuzzleDialogView();

            var wallet = Substitute.For<IWallet>();
            wallet.Money.Returns(new UniRx.ReactiveProperty<int>(10));
            wallet.CanSpend(Arg.Any<int>()).Returns(false);

            var loader = Substitute.For<ILoadingService>();

            var policy = Substitute.For<IPuzzleStartCostPolicy>();
            policy.GetStartModeForPieces(Arg.Any<int>()).Returns(PuzzleStartMode.Coins);
            policy.GetPuzzleCost().Returns(25);

            var coinsStrategy = new CoinsStartStrategy(loader, wallet, view, policy);
            IPuzzleStartStrategy[] strategies = new IPuzzleStartStrategy[] { coinsStrategy };

            var meta = new PuzzleMeta("id","title","url");
            var presenter = new PuzzleDialogPresenter(view, policy, strategies, meta);

            view.TriggerPieces(36);
            view.TriggerStart();

            // Soft assertions: check that the UI was informed about insufficient funds
            Assert.IsTrue(!string.IsNullOrEmpty(view.LastStatus) && view.LastStatus.ToLower().Contains("not enough"),
                $"Expected status to contain 'not enough', actual='{view.LastStatus}'");

            // Ensure ResetSelectedPieces or Close may or may not be called for this failure path,
            // but for soft checking assert we did not call wallet.Spend
            wallet.DidNotReceive().Spend(Arg.Any<int>());
        }
    }
}