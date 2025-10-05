// using System;
// using NUnit.Framework;
// using NSubstitute;
// using OMG.Presenters;
// using OMG.Models;
// using OMG.Views;
// using OMG.Services;
// using Cysharp.Threading.Tasks;
// using NSubstitute.Extensions;
// using UniRx;
//
// namespace OMG.Tests
// {
//     public class PuzzleStartPresenterTests
//     {
//         private IPuzzleStartView _view;
//         private IAdsService _ads;
//         private IWallet _wallet;
//         private ITextureService _texture;
//         private PuzzleStartModel _model;
//         private PuzzleStartPresenter _presenter;
//
//         [SetUp]
//         public void Setup()
//         {
//             _view = Substitute.For<IPuzzleStartView>();
//             _ads = Substitute.For<IAdsService>();
//             _wallet = Substitute.For<IWallet>();
//             _texture = Substitute.For<ITextureService>();
//             _model = new PuzzleStartModel();
//             _presenter = new PuzzleStartPresenter(_model, _view, _ads, _wallet, _texture);
//         }
//
//         [TearDown]
//         public void Teardown() => _presenter.Dispose();
//
//         [Test]
//         public async void TestStartWithCoins_Success()
//         {
//             _wallet.CanSpend(Arg.Any<int>()).Returns(true);
//            // _currency.Money.Returns();
//             // Simulate view event
//             _view.OnStartWithCoins.ReturnsForAll(Observable.Return(Unit.Default));
//             // Wait short time for async to process
//             await UniTask.Delay(50);
//             // Assert coins updated on shop model
//          //   Assert.AreEqual(80, _shop.Coins.Value);
//         }
//     }
// }