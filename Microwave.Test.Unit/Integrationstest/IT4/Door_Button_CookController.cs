using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;
using NSubstitute;
using NUnit.Framework;
using Timer = MicrowaveOvenClasses.Boundary.Timer;

namespace Microwave.Test.Unit.Integrationstest.IT4
{
    [TestFixture]
    class Door_Button_CookController
    {
        
        
        private ITimer _timer;
        private IPowerTube _powerTube;
        private ILight _light;
        private IDisplay _display;
        private CookController _cookController;
        private UserInterface _userInterface;
        private Door _door;
        private Button _startCancelButton;
        private Button _timerButton;
        private Button _powerButton;

        [SetUp]
        public void Setup()
        {
            _startCancelButton = new Button();
            _powerButton = new Button();
            _timerButton = new Button();
            _door = new Door();
            _timer = new Timer();
            _powerTube = Substitute.For<IPowerTube>();
            _light = Substitute.For<ILight>();
            _display = Substitute.For<IDisplay>();
            _cookController = new CookController(_timer, _display, _powerTube);
            _userInterface = new UserInterface(_powerButton, _timerButton, _startCancelButton, _door, _display, _light, _cookController);
            _cookController.UI = _userInterface;
        }

        [Test]
        public void OnTimerExpired_TurnOffInPowertubeClassCalled_Once()
        {
            _door.Open();
            _door.Close();
            _powerButton.Press();
            _timerButton.Press();
            _startCancelButton.Press();
            Thread.Sleep(61000);
            _powerTube.Received(1).TurnOff();


        }

        [Test]
        public void OnTimerTick_ShowTimeInDisplayClass_IsCalled_SixtyOneTimes()
        {
            _door.Open();
            _door.Close();
            _powerButton.Press();
            _timerButton.Press();
            _startCancelButton.Press();
            Thread.Sleep(61000);
            //showTime is called 2 times before it is called in OnTimerTick
            //Its is called 59 times in OntimerTick
            _display.Received(61).ShowTime(Arg.Any<int>(), Arg.Any<int>());
        }

        [Test]
        public void OnTimerTick_ShowTimeInDisplayClassIsCalled_withZeroMinutesAndFiftyNineSeconds()
        {
            _door.Open();
            _door.Close();
            _powerButton.Press();
            _timerButton.Press();
            _startCancelButton.Press();
            Thread.Sleep(61000);
            _display.Received(1).ShowTime(Arg.Is(0),Arg.Is(59));
        }

        [Test]
        public void OnTimerTick_ShowTimeInDisplayClassIsCalled_WithZeroMinutesAndZeroSeconds()
        {
            _door.Open();
            _door.Close();
            _powerButton.Press();
            _timerButton.Press();
            _startCancelButton.Press();
            Thread.Sleep(61000);
            _display.Received(1).ShowTime(Arg.Is(0), Arg.Is(0));
        }

        public void OntimerTick_ShowTimeInDisplayClassIsCalled_WithOneMinuteAndZeroSeconds()
        {

        }
    }
    
}
